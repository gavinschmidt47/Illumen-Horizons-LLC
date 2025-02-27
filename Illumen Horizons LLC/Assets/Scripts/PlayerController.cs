using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;

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

    //Enemy
    public NavMeshAgent enemy;
    public Vector3[] spawn;
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
        enemyObject.GetComponent<EnemyController>().enabled = false;
        enemy.SetDestination(spawn[UnityEngine.Random.Range(0, 5)]);
        enemy.isStopped = false;
    }

    void LateUpdate ()
    {
        //Prevent physics interference
        rb.angularVelocity = Vector3.zero;

        //Look
        if (camControl)
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
        if (input.magnitude > 1 && !camControl)
        {
            camControl = true;
            
            //Remove tooltip
            StartCoroutine(ToolTip(moveTT));
        }

        //Sprint
        if (sprint.ReadValue<float>() > 0)
        {
            gameInfo.speed = gameInfo.sprintSpeed;
            stamina -= gameInfo.staminaDecrease;
            if (stamina < 0)
            {
                stamina = 0;
                gameInfo.speed = gameInfo.walkSpeed;
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

        //Move
        moveDirection = transform.TransformDirection(Vector3.forward * input.y + Vector3.right * input.x) * gameInfo.speed;

        rb.linearVelocity = new Vector3(moveDirection.x, rb.linearVelocity.y, moveDirection.z);
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

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Start"))
        {
            //Track start
            if (gameInfo.inStart)
            {
                //Track start
                gameInfo.inStart = false;

                //Enable enemy
                enemy.isStopped = false;
                enemyObject.GetComponent<EnemyController>().enabled = true;
            }
            else
            {
                //Track start
                gameInfo.inStart = false;
                
                //Disable enemy
                enemyObject.GetComponent<EnemyController>().enabled = false;
                enemy.SetDestination(spawn[UnityEngine.Random.Range(0, 5)]);
            }
        }
    }
}