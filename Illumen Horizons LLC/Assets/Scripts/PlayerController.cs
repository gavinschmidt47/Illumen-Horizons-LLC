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

    //InputActions
    public InputActionAsset playerControls;
    private InputAction movement;
    private InputAction sprint;
    private InputAction look;

    //Movement
    private Vector2 moveDirection;
    private Vector2 lookDirection;
    
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

        //Disable cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        //Get Input
        moveDirection = movement.ReadValue<Vector2>();
        lookDirection = look.ReadValue<Vector2>();
        Look();
        if (sprint.ReadValue<float>() > 0.5f)
        {
            gameInfo.speed = gameInfo.sprintSpeed;
        }
        else
        {
            gameInfo.speed = gameInfo.walkSpeed;
        }
    }

    void FixedUpdate()
    {
        //Move
        Move();
    }

    void Move()
    {
        //Understand the direction
        Vector3 direction = new Vector3(moveDirection.x, 0, moveDirection.y);
        direction = transform.TransformDirection(direction);
        Vector3 velocity = direction * gameInfo.speed * Time.fixedDeltaTime;

        //Move
        rb.linearVelocity = velocity;
    }

    void Look()
    {
        //Rotate Player
        float yaw = lookDirection.x * gameInfo.yawSpeed * Time.deltaTime;
        rb.angularVelocity = new Vector3(0, yaw, 0);

        //Rotate Camera
        float pitch = lookDirection.y * gameInfo.pitchSpeed * Time.deltaTime;
        Vector3 currentRotation = mainCamera.transform.localEulerAngles;
        float desiredPitch = currentRotation.x - pitch;

        if (desiredPitch > 180) 
            desiredPitch -= 360; // Convert to -180 to 180 range
        desiredPitch = Mathf.Clamp(desiredPitch, -58, 90); // Clamp pitch

        mainCamera.transform.localEulerAngles = new Vector3(desiredPitch, currentRotation.y, currentRotation.z);
    }
}
