using UnityEngine;
using UnityEngine.InputSystem;

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
    }

    void LateUpdate ()
    {
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

        // Debug movement input
        Debug.Log($"Movement Logic: {moveDirection}");
        rb.linearVelocity = new Vector3(moveDirection.x, 0, moveDirection.z);
    }
}