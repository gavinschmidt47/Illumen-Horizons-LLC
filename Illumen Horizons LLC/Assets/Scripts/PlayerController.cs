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
    private Vector3 moveDirection;
    private Vector2 lookDirection;
    private float inputX;
    private float inputY;
    
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

        //Freeze rotation
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        //Disable cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate ()
    {
        Vector2 mouseDelta = look.ReadValue<Vector2>();
        Vector3 rotation = transform.rotation.eulerAngles;
        rotation.y += mouseDelta.x * gameInfo.mouseSensitivity;
        transform.rotation = Quaternion.Euler(0, rotation.y, 0);
        mainCamera.transform.Rotate(Vector3.left * mouseDelta.y * gameInfo.mouseSensitivity);
        mainCamera.transform.localEulerAngles = new Vector3(mainCamera.transform.localEulerAngles.x, mainCamera.transform.localEulerAngles.y, 0);

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

        // Debug movement input
        Debug.Log($"Movement Input: {input}");

        //Move
        moveDirection = new Vector3(input.x, 0, input.y);
        moveDirection = transform.TransformDirection(Vector3.forward * -input.y + Vector3.right * -input.x) * gameInfo.speed;
        rb.linearVelocity = new Vector3(moveDirection.x, rb.linearVelocity.y, moveDirection.z);
    }
}