using UnityEngine;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
    //GameInfo
    public GameInfo gameInfo;

    //Input in-gamne
    public InputActionAsset playerControls;
    private InputAction pause;

    
    void Awake ()
    {
        pause = playerControls.FindAction("Cancel");
    }

    void OnEnable ()
    {
        pause.Enable();
    }
    void OnDiable ()
    {
        pause.Disable();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //Pause
        if (pause.triggered)
        {
            if (Time.timeScale == 1)
            {
                Time.timeScale = 0;

                //Enable Cursor
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Time.timeScale = 1;

                //Disable Cursor
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
}
