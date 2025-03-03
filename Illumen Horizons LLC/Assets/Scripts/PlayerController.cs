using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //GameInfo
    public GameInfo gameInfo;

    //Rigidbody
    private Rigidbody rb;

    //Camera
    public Camera mainCamera;
    private bool camControl = false;

    //InputActions
    public InputActionAsset playerControls;
    private InputAction movement;
    private InputAction sprint;
    private InputAction look;

    //Movement
    private Vector3 moveDirection;
    private float stamina = 100.0f;

    //UI
    public GameObject moveTT;
    public Slider staminaUI;
    public GameObject winPanel;
    public GameObject losePanel;
    public AudioSource goMusic;
    public AudioSource enemyDeath;

    //Enemy
    public GameObject enemyObject;
    
    //Initialization of input actions
    private void Awake() 
    {
        movement = playerControls.FindAction("Move");
        sprint = playerControls.FindAction("Sprint");
        look = playerControls.FindAction("Look");
    }
    private void OnEnable() 
    {
        movement.Enable();
        sprint.Enable();
        look.Enable();
    }
    private void OnDisable() 
    {
        movement.Disable();
        sprint.Disable();
        look.Disable();
    }

    void Start()
    {
        //Get Rigidbody
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false; // Ensure Rigidbody is not kinematic

        //Set speed
        gameInfo.speed = gameInfo.walkSpeed;

        //Set Stamina
        stamina = gameInfo.maxStamina;

        //Freeze rotation
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        //Disable cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        //Move tooltip
        moveTT.SetActive(true);

        //Set NavMesh
        gameInfo.inStart = true;

        //Set Pause
        gameInfo.paused = false;
    }

    void LateUpdate ()
    {
        //Prevent physics interference
        rb.angularVelocity = Vector3.zero;

        //Look
        if (camControl && !gameInfo.paused)
        {
            Vector2 mouseDelta = look.ReadValue<Vector2>();
            Vector3 rotation = transform.rotation.eulerAngles;
            rotation.y += mouseDelta.x * gameInfo.mouseSensitivity;
            transform.rotation = Quaternion.Euler(0, rotation.y, 0);
            mainCamera.transform.Rotate(Vector3.left * mouseDelta.y * gameInfo.mouseSensitivity);
            mainCamera.transform.localEulerAngles = new Vector3(mainCamera.transform.localEulerAngles.x, mainCamera.transform.localEulerAngles.y, 0);
        }

        //Clamp the pitch rotation to prevent the camera from flipping
        Vector3 currentRotation = mainCamera.transform.localEulerAngles;
        if (currentRotation.x > 180f)
        {
            currentRotation.x -= 360f;
        }
        currentRotation.x = Mathf.Clamp(currentRotation.x, -gameInfo.maxPitch, gameInfo.maxPitch);
        mainCamera.transform.localEulerAngles = currentRotation;
    }

    void FixedUpdate()
    {
        //Get input
        Vector2 input = Vector2.zero;
        input = movement.ReadValue<Vector2>();

        //Locked Cam
        if (input.magnitude > 0.1f && !camControl)
        {
            camControl = true;
            
            //Remove tooltip
            StartCoroutine(ToolTip(moveTT));
        }

        //Sprint
        if (sprint.ReadValue<float>() > 0)
        {
            gameInfo.speed = gameInfo.sprintSpeed;
            if (!gameInfo.infStam)
            {
                
                stamina -= gameInfo.staminaDecrease;
                if (stamina < 0)
                {
                    stamina = 0;
                    gameInfo.speed = gameInfo.walkSpeed;
                }
            }
        }
        else
        {
            gameInfo.speed = gameInfo.walkSpeed;
            if (stamina < gameInfo.maxStamina)
            {
                stamina += gameInfo.staminaIncrease;
            }
        }

        //Stamina UI
        staminaUI.value = stamina / gameInfo.maxStamina;

        //Move
        moveDirection = transform.TransformDirection(Vector3.forward * input.y + Vector3.right * input.x) * gameInfo.speed;

        rb.linearVelocity = new Vector3(moveDirection.x, 0, moveDirection.z);
    }

    private IEnumerator ToolTip(GameObject tip)
    {   
        CanvasGroup canvasGroup = tip.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = tip.AddComponent<CanvasGroup>();
        }
        canvasGroup.alpha = 1;

        tip.SetActive(true);
        yield return new WaitForSeconds(gameInfo.ttTime);

        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1, 0, elapsedTime);
            yield return null;
        }
        canvasGroup.alpha = 0;
        tip.SetActive(false);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Start"))
        {
            gameInfo.inStart = false;
        }
    }

    void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Stair 1"))
        {
            transform.position = new Vector3 (-27f, 7.6f, 38.178f);
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        else if (other.CompareTag("Stair 2"))
        {
            transform.position = new Vector3 (24.9f, 7.6f, 38.178f);
            transform.rotation = Quaternion.Euler(0, -90, 0);
        }
        else if (other.CompareTag("Finish"))
        {
            Time.timeScale = 0;
            movement.Disable();
            sprint.Disable();
            look.Disable();

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            goMusic.mute = false;

            winPanel.SetActive(true);
            staminaUI.gameObject.SetActive(false);

            gameInfo.gameOver = true;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !gameInfo.invincible)
        {
            Time.timeScale = 0;
            movement.Disable();
            sprint.Disable();
            look.Disable();

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            losePanel.SetActive(true);
            staminaUI.gameObject.SetActive(false);

            goMusic.mute = false;

            Destroy(collision.gameObject);
            
            gameInfo.gameOver = true;
        }
    }
}