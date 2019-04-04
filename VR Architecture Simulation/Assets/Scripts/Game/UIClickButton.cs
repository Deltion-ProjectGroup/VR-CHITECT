using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIClickButton : UIButtonBase
{
    [SerializeField] Image buttonImage;
    [SerializeField] Color hoverColor, toggledColor;

    public override void Interact()
    {
        GetComponent<Animation>().Play();
        GetComponent<Button>().OnPointerClick(new PointerEventData(EventSystem.current));
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
