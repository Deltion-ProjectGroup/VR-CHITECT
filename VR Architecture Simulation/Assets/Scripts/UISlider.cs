using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using UnityEngine.UI;

public class UISlider : UIButtonBase
{
    [HideInInspector]public Slider thisSlider;
    public float speedModifier;
    public Text amountShower;
    public bool sliding = false;
    public SteamVR_Action_Vector2 moveAmount;
    public SteamVR_Action_Boolean confirmMove;
    [SerializeField] SteamVR_Input_Sources controlSource;
    [SerializeField] Image sliderKnob;
    [SerializeField] Color hoverColor, selectedColor;


    public void Awake()
    {
        thisSlider = GetComponent<Slider>();
    }
    public void Update()
    {
        if (sliding)
        {
            print("Moving");
            if (confirmMove.GetState(InputMan.GetHand(controlSource)))
            {
                GetComponent<Slider>().value += moveAmount.axis.x * speedModifier * Time.deltaTime;
            }
            GetComponent<Slider>().value += Input.GetAxis("Horizontal") * speedModifier * Time.deltaTime;
            amountShower.text = thisSlider.value.ToString("F2");
        }
    }

    public override void Interact()
    {
        sliding = !sliding;
        if (sliding)
        {
            UIManager.uiManager.settings.GetComponent<UISelection>().selectionState = UISelection.SelectionState.Frozen;
            sliderKnob.color = selectedColor;
        }
        else
        {
            UIManager.uiManager.settings.GetComponent<UISelection>().selectionState = UISelection.SelectionState.Selecting;
            sliderKnob.color = hoverColor;
        }
    }
    public override void OnHover()
    {
        sliderKnob.color = hoverColor;
    }
    public override void OnHoverEnd()
    {
        sliderKnob.color = Color.white;
    }
}
