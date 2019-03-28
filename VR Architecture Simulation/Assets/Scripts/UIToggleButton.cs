using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIToggleButton : UIButtonBase
{
    [SerializeField] Image buttonImage;
    [SerializeField] Color hoverColor;

    public override void Interact()
    {
        GetComponent<Toggle>().isOn = !GetComponent<Toggle>().isOn;
    }
    public override void OnHover()
    {
        buttonImage.color = hoverColor;
    }
    public override void OnHoverEnd()
    {
        buttonImage.color = Color.white;
    }
}
