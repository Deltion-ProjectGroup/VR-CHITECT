using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Audio;

public class Options : UIMenu
{
    [SerializeField]Text gridDivText;
    [SerializeField]Text rotSnapText;
    public List<UISelection.TDGODataHolder> settingsDataHolder = new List<UISelection.TDGODataHolder>();
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Toggle handToggle;
    // Start is called before the first frame update
    void Start()
    {
        handToggle.gameObject.GetComponent<Animator>().SetBool("On", handToggle.isOn);
        //UpdateRotationSnap(Placer.placer.rotateTurnAmount);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void UpdateGridDivision(UISlider uiSlider)
    {
        Placer.placer.ChangeTileDivision(uiSlider.thisSlider.value);
    }

    public void UpdateRotationSnap(UISlider uiSlider)
    {
        Placer.placer.ChangeSnapRotation((int)uiSlider.thisSlider.value);
    }
    public void ChangePrimaryHand(Toggle toggle)
    {
        InputMan.ChangePrimaryHand(toggle.isOn);
    }
    public override IEnumerator Open()
    {
        GetComponent<UISelection>().Initialize(false);
        yield return null;
        UIManager.uiManager.canToggle = true;
    }
    public override void InstantClose()
    {
        gameObject.SetActive(false);
    }
    public void ChangeData(int dataIndex)
    {
        if(dataIndex >= 0 && dataIndex < settingsDataHolder.Count)
        {
            UISelection uiSelector = GetComponent<UISelection>();
            uiSelector.selectableOptions = settingsDataHolder[dataIndex].data;
        }
    }

    public void ChangeLinear(float newValue)
    {

    }
    public void ChangeMasterVolume(Slider bar)
    {
        audioMixer.SetFloat("MasterVolume", bar.value);
    }
    public void ChangeBackgroundVolume(Slider bar)
    {
        audioMixer.SetFloat("BackgroundVolume", bar.value);
    }
    public void ChangeSFXVolume(Slider bar)
    {
        audioMixer.SetFloat("SFXVolume", bar.value);
    }
}
