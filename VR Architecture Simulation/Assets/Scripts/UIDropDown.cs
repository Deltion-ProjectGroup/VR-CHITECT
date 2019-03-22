using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using UnityEngine.EventSystems;

public class UIDropDown : UIButtonBase
{
    public bool selecting;
    public SteamVR_Action_Boolean acceptButton;
    public SteamVR_Action_Vector2 movement;
    public float movementModifier;
    public Dropdown thisDropdown;

    public void Awake()
    {
        thisDropdown = GetComponent<Dropdown>();
    }
    public void Update()
    {
        if (selecting)
        {
            int moveAmt;
            if (acceptButton.GetState(InputMan.rightHand))
            {
                moveAmt = Mathf.CeilToInt(movement.axis.y);
            }
            else
            {
                moveAmt = (int)Input.GetAxisRaw("Vertical");
            }
            if(moveAmt != 0)
            {
                Move(moveAmt);
            }
        }
    }
    public override void Interact()
    {
        selecting = !selecting;
        if (selecting)
        {
            thisDropdown.OnPointerClick(new PointerEventData(EventSystem.current));
            UIManager.uiManager.settings.GetComponent<UISelection>().selectionState = UISelection.SelectionState.Frozen;
        }
        else
        {
            thisDropdown.OnPointerClick(new PointerEventData(EventSystem.current));
            UIManager.uiManager.settings.GetComponent<UISelection>().selectionState = UISelection.SelectionState.Selecting;
        }
        print(UIManager.uiManager.settings.GetComponent<UISelection>().selectionState.ToString());
    }
    public void Move(int moveAmount)
    {
        int newVal = thisDropdown.value + moveAmount;
        if(newVal < 0)
        {
            newVal = thisDropdown.options.Count - 1;
        }
        else
        {
            if(newVal >= thisDropdown.options.Count)
            {
                newVal = 0;
            }
        }
    }
}
