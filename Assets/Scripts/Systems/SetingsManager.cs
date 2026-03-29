using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour
{
    public AudioMixer mainMixer;

    public Resolution[] resolutions;

    // Fields
    public int resolutionIndex = -1;
    public int isFullScreen;
    public float volume;
    public float sensitivity;
    public float xSensitivity;
    public float ySensitivity;

    public static SettingsManager Instance;

    public void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy (gameObject);

        DontDestroyOnLoad(gameObject);

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
    }

    public void SetFullScreen(bool isFScreen)
    {
        Screen.fullScreen = isFScreen;
        if (isFScreen)
        {
            PlayerPrefs.SetInt("Fullscreen", 1);

            Debug.Log("Fullscreen mode set!");
            return;
        }

        PlayerPrefs.SetInt("Fullscreen", 0);
    }

    public void SetVolume (float vol)
    {
        mainMixer.SetFloat("Volume", vol);
        PlayerPrefs.SetFloat("Volume", vol);
    }

    public void SetSensitivity(float sens)
    {
        sensitivity = sens;
        PlayerPrefs.SetFloat("Sensitivity", sens);
    }
    public void SetXSensitivity(float sens)
    {
        xSensitivity = sens;
        PlayerPrefs.SetFloat("X Sensitivity", sens);
    }
    public void SetYSensitivity(float sens)
    {
        ySensitivity = sens;
        PlayerPrefs.SetFloat("Y Sensitivity", sens);
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
        PointsSystem.Instance.LoadPlayerPrefs();
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

        SetSensitivity(sensitivity);
        SetXSensitivity(xSensitivity);
        SetYSensitivity(ySensitivity);
    }

    private void PrepareResolutions() // Only in Await!
    {
        int currentResolutionIndex = 0;

        resolutions = Screen.resolutions;
        List<string> options = new List<string>();

        foreach (var res in resolutions)
        {
            options.Add($"{res.width} x {res.height}   {res.refreshRateRatio}Гц");

            if(res.width == Screen.currentResolution.width &&
                res.height == Screen.currentResolution.height) // If same resolution
                SetResolution(currentResolutionIndex);

            currentResolutionIndex++;
        }
    }
}