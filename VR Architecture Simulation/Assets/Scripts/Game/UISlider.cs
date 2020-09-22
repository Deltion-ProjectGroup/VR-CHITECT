using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OVR;
using UnityEngine.UI;

public class UISlider : UIButtonBase
{
    [HideInInspector]public Slider thisSlider;
    public float speedModifier;
    public Text amountShower;
    public bool sliding = false;
    [SerializeField] OVRInput.Axis2D moveButton;
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
            Vector2 moveAmount = OVRInput.Get(InputMan.GetAxis2D(moveButton));

            GetComponent<Slider>().value += moveAmount.x * speedModifier * Time.deltaTime;
            //GetComponent<Slider>().value += Input.GetAxis("Horizontal") * speedModifier * Time.deltaTime;
            amountShower.text = thisSlider.value.ToString("F2");
        }
    }

    public override void Interact()
    {
        sliding = !sliding;
        buttonAnimator.SetBool("On", sliding);
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
