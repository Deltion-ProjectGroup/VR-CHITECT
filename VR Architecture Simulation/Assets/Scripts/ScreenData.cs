using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenData : MonoBehaviour
{
    public Dropdown resolutionDropdown;
    public Dropdown qualityDropdown;
    public Toggle fullscreenToggle;
    // Start is called before the first frame update
    void Awake()
    {
        int initializeOption = 0;
        resolutionDropdown.ClearOptions();
        List<Dropdown.OptionData> optionData = new List<Dropdown.OptionData>();
        foreach(Resolution thisResolution in Screen.resolutions)
        {
            optionData.Add(new Dropdown.OptionData(thisResolution.width.ToString() + "x" + thisResolution.height.ToString()));
            if(Screen.currentResolution.width == thisResolution.width && Screen.currentResolution.height == thisResolution.height)
            {
                initializeOption = optionData.Count - 1;
            }
        }
        resolutionDropdown.AddOptions(optionData);
        resolutionDropdown.value = initializeOption;
        resolutionDropdown.RefreshShownValue();
        fullscreenToggle.isOn = Screen.fullScreen;
        optionData = new List<Dropdown.OptionData>();
        qualityDropdown.ClearOptions();
        foreach(string qualityName in QualitySettings.names)
        {
            print(qualityName);
            optionData.Add(new Dropdown.OptionData(qualityName));
            if(qualityName == QualitySettings.names[QualitySettings.GetQualityLevel()])
            {
                initializeOption = optionData.Count - 1;
            }
        }
        qualityDropdown.AddOptions(optionData);
        qualityDropdown.value = initializeOption;
        qualityDropdown.RefreshShownValue();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeResolution()
    {
        Screen.SetResolution(Screen.resolutions[resolutionDropdown.value].width, Screen.resolutions[resolutionDropdown.value].height, Screen.fullScreen);
    }
    public void ToggleFullscreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }
    public void ChangeQualityLevel()
    {
        QualitySettings.SetQualityLevel(qualityDropdown.value);
    }
}
