using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public TMPro.TMP_Dropdown resDropdown;
    public Toggle fullScreenToggle;
    public Slider volumeSlider;
    public Slider sensitivitySlider;
    public Slider xSensitivitySlider;
    public Slider ySensitivitySlider;
    
    public PlayerController playerController;

    private Resolution[] resolutions;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>

    public void Awake()
    {
        PrepareResolutions();

        SetSettings();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void SetResolution(int resIndex)
    {
        resDropdown.value = resIndex;

        SettingsManager.Instance.SetResolution(resIndex);
    }

    public void SetFullScreen(bool isFScreen)
    {
        fullScreenToggle.isOn = isFScreen;

        SettingsManager.Instance.SetFullScreen(isFScreen);
    }

    public void SetVolume(float vol)
    {
        volumeSlider.value = vol;

        SettingsManager.Instance.SetVolume(vol);
    }

    public void SetSensitivity(float sens)
    {
        sensitivitySlider.value = sens;

        SettingsManager.Instance.SetSensitivity(sens);
        if(playerController != null)
            playerController.UpdateSensititvity();

    }
    public void SetXSensitivity(float sens)
    {
        xSensitivitySlider.value = sens;

        SettingsManager.Instance.SetXSensitivity(sens);
        if(playerController != null)
            playerController.UpdateSensititvity();
    }
    public void SetYSensitivity(float sens)
    {
        ySensitivitySlider.value = sens;

        SettingsManager.Instance.SetYSensitivity(sens);
        if(playerController != null)
            playerController.UpdateSensititvity();
    }
    public void ClearProgress()
    {
        SettingsManager.Instance.ClearProgress();
    }

    public void ClearSettings()
    {
        SettingsManager.Instance.ClearSettings();

        Awake();
    }

    public void SetSettings()
    {
        resDropdown.value = SettingsManager.Instance.resolutionIndex;
        if (SettingsManager.Instance.isFullScreen == 1)
            fullScreenToggle.isOn = true;
        else
            fullScreenToggle.isOn = false;
        volumeSlider.value = SettingsManager.Instance.volume;
        sensitivitySlider.value = SettingsManager.Instance.sensitivity;
        xSensitivitySlider.value = SettingsManager.Instance.xSensitivity;
        ySensitivitySlider.value = SettingsManager.Instance.ySensitivity;

        playerController = GameObject.Find("Player")?.GetComponent<PlayerController>();
    }

    private void PrepareResolutions() // Only in Awake!
    {
        resolutions = SettingsManager.Instance.resolutions;
        
        List<string> options = new List<string>();

        foreach (var res in resolutions)
            options.Add($"{res.width} x {res.height}   {res.refreshRateRatio}Гц");
        
        resDropdown.AddOptions(options);
    }
}
