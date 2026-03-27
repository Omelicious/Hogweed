using System.Collections;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public static MainMenu Instance;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        Instance = this;
    }

    [SerializeField] private GameObject titleText;
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject settingsMenu;


    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (Input.GetButtonDown("Escape"))
            ToggleSettingsMenu();
    }

    public void PlayGame ()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Application closed");
    }

    public void ToggleSettingsMenu()
    {
        titleText.SetActive(!menu.activeSelf);
        menu.SetActive(!menu.activeSelf);
        settingsMenu.SetActive(!menu.activeSelf);
    }
}
