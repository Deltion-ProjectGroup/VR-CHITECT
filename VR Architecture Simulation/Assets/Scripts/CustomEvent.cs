using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CustomEvent : ScriptableObject
{
    public delegate void OnComplete();
    public OnComplete OnCompleteEvent;

    public abstract void Activate();
}
