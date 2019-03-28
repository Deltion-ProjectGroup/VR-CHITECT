using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public abstract class UIButtonBase : Interactable
{
    public Animator buttonAnimator;
    // Start is called before the first frame update
    public override void Interact()
    {

    }
    public abstract void OnHover();
    public abstract void OnHoverEnd();
}