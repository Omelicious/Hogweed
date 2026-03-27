using UnityEngine;
using UnityEngine.UI;

public class MenuSystem : MonoBehaviour
{        
    public static MenuSystem Instance;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
    }

    [SerializeField] private PlayerController playerController;
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject inGameUI;
    [SerializeField] private GameObject buyMenu;

    // Menu
    public void ToggleEscapeMenu()
    {
        if (InGameMenu.Instance.settingsMenu.activeSelf)
        {
            InGameMenu.Instance.CloseSettingsMenu();
            return;
        }

        if (!buyMenu.activeSelf) // if no buyMenu
        {
            ShowCursor();
            playerController.isActivelyPlaying = !playerController.isActivelyPlaying;
        }

        menu.SetActive(!menu.activeSelf);
        buyMenu.SetActive(false);
        inGameUI.SetActive(!inGameUI.activeSelf);
    }

    // Buy menu
    public void ToggleBuyMenu()
    {
        if (menu.activeSelf || InGameMenu.Instance.settingsMenu.activeSelf)
            return;

        ShowCursor();

        playerController.isActivelyPlaying = !playerController.isActivelyPlaying;
        buyMenu.SetActive(!buyMenu.activeSelf);
    }

    private void ShowCursor()
    {
        Cursor.visible = !Cursor.visible;
        if (Cursor.lockState == CursorLockMode.None)
        {
            Cursor.lockState = CursorLockMode.Locked;
            return;
        }
        Cursor.lockState = CursorLockMode.None;
    }
        // // Releases the cursor
        // Cursor.lockState = CursorLockMode.None;

        // // Locks the cursor
        // Cursor.lockState = CursorLockMode.Locked;

        // // Confines the cursor
        // Cursor.lockState = CursorLockMode.Confined;
}
