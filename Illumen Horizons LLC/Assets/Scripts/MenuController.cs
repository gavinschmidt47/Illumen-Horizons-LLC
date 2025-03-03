using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    //GameInfo
    public GameInfo gameInfo;

    //UI
    public GameObject mainMenuPanel;
    public GameObject optionsPanel;
    public GameObject aboutPanel;
    public Button backButton;
    public Toggle infToggle;
    public Toggle invToggle;

    private void OnEnable()
    {
        // Ensure the toggle's initial state matches the ScriptableObject's value
        if (infToggle != null && gameInfo != null)
        {
            infToggle.isOn = gameInfo.infStam;
        }

        if (invToggle != null && gameInfo != null)
        {
            invToggle.isOn = gameInfo.invincible;
        }

        // Add listener for when the toggle's value changes
        infToggle.onValueChanged.AddListener(OnInfToggleValueChanged);
        invToggle.onValueChanged.AddListener(OnInvToggleValueChanged);
    }

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Set Panels
        mainMenuPanel.SetActive(true);
        optionsPanel.SetActive(false);
        aboutPanel.SetActive(false);
        backButton.gameObject.SetActive(false);

        gameInfo.gameOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (backButton.gameObject.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            Back();
        }
    }

    public void Play()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main Scene");
    }

    public void Options()
    {
        mainMenuPanel.SetActive(false);
        optionsPanel.SetActive(true);
        aboutPanel.SetActive(false);
        backButton.gameObject.SetActive(true);
    }

    public void About()
    {
        mainMenuPanel.SetActive(false);
        optionsPanel.SetActive(false);
        aboutPanel.SetActive(true);
        backButton.gameObject.SetActive(true);
    }

    public void Back()
    {
        mainMenuPanel.SetActive(true);
        optionsPanel.SetActive(false);
        aboutPanel.SetActive(false);
        backButton.gameObject.SetActive(false);
    }

    public void Exit()
    {
        Application.Quit();
    }

    private void OnInfToggleValueChanged(bool newValue)
    {
        // Update the ScriptableObject's value when the toggle changes
        if (gameInfo != null)
        {
            gameInfo.infStam = newValue;
            Debug.Log($"Toggle value changed to: {newValue}");
        }
    }
    private void OnInvToggleValueChanged(bool newValue)
    {
        // Update the ScriptableObject's value when the toggle changes
        if (gameInfo != null)
        {
            gameInfo.invincible = newValue;
            Debug.Log($"Invincible toggle value changed to: {newValue}");
        }
    }
}
