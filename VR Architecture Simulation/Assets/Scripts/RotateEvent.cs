using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New RotateEvent", menuName = "New Event/Action/Rotate")]
public class RotateEvent : CountEvent
{
    public override void Activate()
    {
        Placer.OnRotate += AddCount;
    }
    public override void RemoveEvent()
    {
        Placer.OnRotate -= AddCount;
    }
}
