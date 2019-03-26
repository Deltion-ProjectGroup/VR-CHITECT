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
    [SerializeField] SteamVR_Input_Sources controlSource;

    public void Awake()
    {
        thisDropdown = GetComponent<Dropdown>();
    }
    public void Update()
    {
        if (selecting)
        {
            int moveAmt = 0;
            if (acceptButton.GetStateDown(InputMan.GetHand(controlSource)))
            {
                moveAmt = Mathf.RoundToInt(movement.axis.y);
            }
            else
            {
                if(Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.UpArrow))
                {
                    if (Input.GetKeyDown(KeyCode.DownArrow))
                    {
                        moveAmt = 1;
                    }
                    else
                    {
                        moveAmt = -1;
                    }
                }
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
            print(thisDropdown.value);
            thisDropdown.Select();
            thisDropdown.Hide();
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
        thisDropdown.value = newVal;
    }
}
