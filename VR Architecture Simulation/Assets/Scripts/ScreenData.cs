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
        resolutionDropdown.ClearOptions();
        List<Dropdown.OptionData> optionData = new List<Dropdown.OptionData>();
        foreach(Resolution thisResolution in Screen.resolutions)
        {
            optionData.Add(new Dropdown.OptionData(thisResolution.width.ToString() + "x" + thisResolution.height.ToString()));
        }
        resolutionDropdown.AddOptions(optionData);
        fullscreenToggle.isOn = Screen.fullScreen;
        optionData = new List<Dropdown.OptionData>();
        foreach(string qualityName in QualitySettings.names)
        {
            optionData.Add(new Dropdown.OptionData(qualityName));
        }
        qualityDropdown.AddOptions(optionData);
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
