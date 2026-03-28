using UnityEngine;
using System.Collections;
using System.Runtime.Serialization;
using UnityEngine.SceneManagement;

public class InGameMenu : MonoBehaviour
{
    public static InGameMenu Instance;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        Instance = this;

        settingsMenu.SetActive(true); // Provoking Awake to apply settings
        settingsMenu.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    [SerializeField] private GameObject escapeMenu;
    [SerializeField] private GameObject buyMenu;
    [SerializeField] public GameObject settingsMenu; // public for now
    [SerializeField] private GameObject inGameUI; // only toggles when escapemenu toggles

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Update()
    {
        // menu
        if (Input.GetButtonDown("Escape"))
        {
            if (!escapeMenu.activeSelf && settingsMenu.activeSelf)
            {
                CloseSettingsMenu();
                return;
            }
            else if (!escapeMenu.activeSelf)
            {
                OpenEscapeMenu();
                return;
            }
            CloseEscapeMenu();
        }
        
        // Buy menu
        if (Input.GetButtonDown("Buy"))
        {
            if (escapeMenu.activeSelf || settingsMenu.activeSelf)
                return;
                
            if (!buyMenu.activeSelf)
            {
                OpenBuyMenu();
                return;
            }
            CloseBuyMenu();
        }
    }

    public void ToMainMenu ()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Application closed");
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        // also it's for the first push attempt
    }
    public void UnPauseGame()
    {
        Time.timeScale = 1f;
        // also it's for the first push attempt
    }

    public void OpenEscapeMenu()
    {
        escapeMenu.SetActive(true);
        buyMenu.SetActive(false);
        settingsMenu.SetActive(false);
        inGameUI.SetActive(false);

        if(!Cursor.visible) // in case of opened buymenu
            ToggleCursor();
        PauseGame();
    }
    public void CloseEscapeMenu()
    {
        escapeMenu.SetActive(false);
        buyMenu.SetActive(false);
        settingsMenu.SetActive(false);
        inGameUI.SetActive(true);

        ToggleCursor();
        UnPauseGame();
    }
    public void OpenBuyMenu()
    {
        escapeMenu.SetActive(false);
        buyMenu.SetActive(true);
        settingsMenu.SetActive(false);

        ToggleCursor();
        PauseGame();
    }
    public void CloseBuyMenu()
    {
        escapeMenu.SetActive(false);
        buyMenu.SetActive(false);
        settingsMenu.SetActive(false);

        ToggleCursor();
        UnPauseGame();
    }
    public void OpenSettingsMenu() // Should only be called from escapemenu methods
    {
        escapeMenu.SetActive(false);
        buyMenu.SetActive(false);
        settingsMenu.SetActive(true);

        PauseGame();
    }
    public void CloseSettingsMenu() // Returns to escape menu
    {
        escapeMenu.SetActive(true);
        buyMenu.SetActive(false);
        settingsMenu.SetActive(false);

        PauseGame();
    }

    private void ToggleCursor()
    {
        Cursor.visible = !Cursor.visible;
        if (Cursor.lockState == CursorLockMode.None)
        {
            Cursor.lockState = CursorLockMode.Locked;
            return;
        }
        Cursor.lockState = CursorLockMode.None;
    }
}
