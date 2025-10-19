using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Linq;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Dropdown resolutionDropDown;
    public Slider musicSlider;
    public Slider soundSlider;
    public Toggle toggle;
    public GameObject settingsWindow;


    private Resolution[] resolutions;

    void Start()
    {
        
        float musicValue = PlayerPrefs.GetFloat("musicValue", 0f);
        SetMusicVolume(musicValue);
        musicSlider.value = musicValue;

        float soundValue = PlayerPrefs.GetFloat("soundEffect", 0f);
        SetSoundVolume(soundValue);
        soundSlider.value = soundValue;

        // Set fullscreen mode based on saved settings
        if(PlayerPrefs.GetInt("fullScreen", 1) == 1)
        {
            Screen.fullScreen = true;
            toggle.isOn = true;
        }
        else if (PlayerPrefs.GetInt("fullScreen", 1) == 0)
        {
            Screen.fullScreen = false;
            toggle.isOn = false;

        }

       resolutions = Screen.resolutions.Distinct().ToArray();
        resolutionDropDown.ClearOptions();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropDown.AddOptions(options);
        resolutionDropDown.value = currentResolutionIndex;
        resolutionDropDown.RefreshShownValue();

        // Set the screen resolution based on saved settings
        int savedWidth = PlayerPrefs.GetInt("resolutionW", Screen.width);
        int savedHeight = PlayerPrefs.GetInt("resolutionH", Screen.height);
        Screen.SetResolution(savedWidth, savedHeight, Screen.fullScreen);

        // Update the dropdown to reflect the saved resolution
        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].width == savedWidth && resolutions[i].height == savedHeight)
            {
                currentResolutionIndex = i;
                break;
            }
        }
        resolutionDropDown.value = currentResolutionIndex;
        resolutionDropDown.RefreshShownValue();
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("music", volume);
        PlayerPrefs.SetFloat("musicValue", volume);
    }

    public void SetSoundVolume(float volume)
    {
        audioMixer.SetFloat("soundEffect", volume);
        PlayerPrefs.SetFloat("soundEffect", volume);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
        PlayerPrefs.SetInt("fullScreen", isFullScreen ? 1 : 0);
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt("resolutionW", resolution.width);
        PlayerPrefs.SetInt("resolutionH", resolution.height);
    }

    public void OpenSettingsWindow()
    {
        settingsWindow.SetActive(true);
    }

    public void CloseSettingsWindow()
    {
        settingsWindow.SetActive(false);
    }
}
