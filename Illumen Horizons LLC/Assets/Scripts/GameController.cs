using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    //GameInfo
    public GameInfo gameInfo;

    //Input in-gamne
    public InputActionAsset playerControls;
    private InputAction pause;

    //UI
    public GameObject pausePanel;
    public GameObject resumeB;
    public GameObject optionsB;
    public Toggle invT;
    public Toggle infT;
    public GameObject backB;
    public GameObject invText;
    public GameObject infText;

    
    void Awake ()
    {
        pause = playerControls.FindAction("Cancel");
    }

    void OnEnable ()
    {
        pause.Enable();
    }
    void OnDisable ()
    {
        pause.Disable();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Set UI
        invText.SetActive(gameInfo.invincible);
        infText.SetActive(gameInfo.infStam);
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

                pausePanel.SetActive(true);
                invText.SetActive(false);
                infText.SetActive(false);

                gameInfo.paused = true;
            }
            else if (invT.gameObject.activeSelf)
            {
                Back();
            }
            else if (!gameInfo.gameOver)
            {
                Time.timeScale = 1;

                //Disable Cursor
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                pausePanel.SetActive(false);
                invText.SetActive(gameInfo.invincible);
                infText.SetActive(gameInfo.infStam);

                gameInfo.paused = false;
            }
            else
            {
                SceneManager.LoadScene("MainMenu");
            }
        }
    }

    public void OptionsOn()
    {
        resumeB.SetActive(false);
        optionsB.SetActive(false);

        invT.gameObject.SetActive(true);
        infT.gameObject.SetActive(true);
        backB.gameObject.SetActive(true);

        invT.isOn = gameInfo.invincible;
        infT.isOn = gameInfo.infStam;
    }

    public void Back()
    {
        resumeB.SetActive(true);
        optionsB.SetActive(true);

        gameInfo.invincible = invT.isOn;
        gameInfo.infStam = infT.isOn;

        invT.gameObject.SetActive(false);
        infT.gameObject.SetActive(false);
        backB.gameObject.SetActive(false);
    }

    public void Resume()
    {
        Time.timeScale = 1;

        //Disable Cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        pausePanel.SetActive(false);

        gameInfo.paused = false;
        invText.SetActive(gameInfo.invincible);
        infText.SetActive(gameInfo.infStam);
    }
}
