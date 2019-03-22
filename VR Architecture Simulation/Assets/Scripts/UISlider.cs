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



    public void Awake()
    {
        thisSlider = GetComponent<Slider>();
    }
    public void Update()
    {
        if (sliding)
        {
            print("Moving");
            if (confirmMove.GetState(InputMan.rightHand))
            {
                GetComponent<Slider>().value += moveAmount.axis.x * speedModifier * Time.deltaTime;
            }
            GetComponent<Slider>().value += Input.GetAxis("Horizontal") * speedModifier * Time.deltaTime;
            amountShower.text = thisSlider.value.ToString("2F");
        }
    }

    public override void Interact()
    {
        sliding = !sliding;
        if (sliding)
        {
            UIManager.uiManager.settings.GetComponent<UISelection>().selectionState = UISelection.SelectionState.Frozen;
        }
        else
        {
            UIManager.uiManager.settings.GetComponent<UISelection>().selectionState = UISelection.SelectionState.Selecting;
        }
    }
}
