using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Settings : MonoBehaviour
{
    public TMPro.TMP_Dropdown resDropdown;
    public Toggle fullScreenToggle;
    public Slider volumeSlider;
    public Slider sensitivitySlider;
    public Slider xSensitivitySlider;
    public Slider ySensitivitySlider;
    

    public AudioMixer mainMixer;
    public PlayerController playerController;

    private Resolution[] resolutions;

    int resolutionIndex = -1;
    int isFullScreen;
    float volume;
    float sensitivity;
    float xSensitivity;
    float ySensitivity;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    /// 
    public void Awake()
    {
        PrepareResolutions();

        SetSettings();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void SetResolution (int resIndex)
    {
        if (resIndex < 0)
            return;

        Screen.SetResolution(resolutions[resIndex].width, resolutions[resIndex].height, Screen.fullScreen);
        PlayerPrefs.SetInt("Resolution", resIndex);

        resDropdown.value = resIndex;

        Debug.Log($"Resolution number {resIndex} set!");
    }

    public void SetFullScreen(bool isFScreen)
    {
        Screen.fullScreen = isFScreen;
        if (isFScreen)
        {
            PlayerPrefs.SetInt("Fullscreen", 1);
            fullScreenToggle.isOn = true;

            Debug.Log("Fullscreen mode set!");
            return;
        }

        PlayerPrefs.SetInt("Fullscreen", 0);
        fullScreenToggle.isOn = false;

        Debug.Log("Fullscreen mode set!");
    }

    public void SetVolume (float vol)
    {
        mainMixer.SetFloat("Volume", vol);
        PlayerPrefs.SetFloat("Volume", vol);
        volumeSlider.value = vol;

        Debug.Log("Volume set!");
    }

    public void SetSensitivity(float sens)
    {
        if(playerController != null)
            playerController.sensitivity = sens;
        
        PlayerPrefs.SetFloat("Sensitivity", sens);

        sensitivitySlider.value = sens;

        Debug.Log("Sensitivity set!");
    }
    public void SetXSensitivity(float sens)
    {
        if(playerController != null)
            playerController.xSensitivity = sens;

        PlayerPrefs.SetFloat("X Sensitivity", sens);

        xSensitivitySlider.value = sens;

        Debug.Log("X Sensitivity set!");
    }
    public void SetYSensitivity(float sens)
    {
        if(playerController != null)
            playerController.ySensitivity = sens;

        PlayerPrefs.SetFloat("Y Sensitivity", sens);

        ySensitivitySlider.value = sens;

        Debug.Log("Y Sensitivity set!");
    }
    public void ClearProgress ()
    {
        PlayerPrefs.DeleteKey("Points total");
        PlayerPrefs.DeleteKey("Movement price");
        PlayerPrefs.DeleteKey("Attack speed price");
        PlayerPrefs.DeleteKey("Attack area price");
        PlayerPrefs.DeleteKey("Movement");
        PlayerPrefs.DeleteKey("Attack speed");
        PlayerPrefs.DeleteKey("Attack area");
        PointsSystem.Instance?.LoadPlayerPrefs();
    }

    public void ClearSettings()
    {
        PlayerPrefs.DeleteKey("Resolution");
        PlayerPrefs.DeleteKey("Fullscreen");
        PlayerPrefs.DeleteKey("Volume");
        PlayerPrefs.DeleteKey("Sensitivity");
        PlayerPrefs.DeleteKey("X Sensitivity");
        PlayerPrefs.DeleteKey("Y Sensitivity");

        Awake();
    }

    public void SetSettings ()
    {
        if (resolutionIndex == -1)
            resolutionIndex = PlayerPrefs.GetInt("Resolution", -1);
        isFullScreen = PlayerPrefs.GetInt("Fullscreen", 1);
        volume = PlayerPrefs.GetFloat("Volume", 0f);

        sensitivity = PlayerPrefs.GetFloat("Sensitivity", 1f);
        xSensitivity = PlayerPrefs.GetFloat("X Sensitivity", 1f);
        ySensitivity = PlayerPrefs.GetFloat("Y Sensitivity", 1f);

        SetResolution(resolutionIndex);

        if (isFullScreen == 0)
            SetFullScreen(false);
        else
            SetFullScreen(true);

        SetVolume (volume);

        playerController = GameObject.Find("Player")?.GetComponent<PlayerController>();

        SetSensitivity(sensitivity);
        SetXSensitivity(xSensitivity);
        SetYSensitivity(ySensitivity);
    }

    private void PrepareResolutions() // Only in Await!
    {
        int currentResolutionIndex = 0;

        resolutions = Screen.resolutions;
        List<string> options = new List<string>();
        
        resDropdown.ClearOptions();

        foreach (var res in resolutions)
        {
            options.Add($"{res.width} x {res.height}   {res.refreshRateRatio}Гц");

            if(res.width == Screen.currentResolution.width &&
                res.height == Screen.currentResolution.height) // If same resolution
                SetResolution(currentResolutionIndex);

            currentResolutionIndex++;
        }
        
        resDropdown.AddOptions(options);

        Debug.Log("Resolutions prepared!");
    }
}
