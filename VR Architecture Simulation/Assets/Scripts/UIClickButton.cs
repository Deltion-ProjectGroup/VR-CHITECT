using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIClickButton : UIButtonBase
{
    public override void Interact()
    {
        GetComponent<Button>().OnPointerClick(new PointerEventData(EventSystem.current));
    }
    public void OnClick()
    {

    }
}
