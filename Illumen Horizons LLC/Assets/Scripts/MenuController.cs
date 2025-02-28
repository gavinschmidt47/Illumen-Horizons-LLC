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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Set Panels
        mainMenuPanel.SetActive(true);
        optionsPanel.SetActive(false);
        aboutPanel.SetActive(false);
        backButton.gameObject.SetActive(false);

        //Set Player Stats
        gameInfo.infStam = false;
        gameInfo.invincible = false;
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
}
