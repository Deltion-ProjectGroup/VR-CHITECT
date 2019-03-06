using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIButton : Interactable
{
    // Start is called before the first frame update
    public override void Interact()
    {
        GetComponent<Button>().OnPointerClick(new PointerEventData(EventSystem.current));
    }
}
