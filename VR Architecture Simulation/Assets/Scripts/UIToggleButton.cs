using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIToggleButton : UIButtonBase
{
    public override void Interact()
    {
        GetComponent<Toggle>().isOn = !GetComponent<Toggle>().isOn;
    }
}
