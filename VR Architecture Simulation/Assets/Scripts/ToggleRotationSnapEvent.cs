using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ToggleRotSnapEvent", menuName = "New Event/Action/Toggle/RotationSnap")]
public class ToggleRotationSnapEvent : CountEvent
{
    public override void Activate()
    {
        Placer.OnToggleRotationSnap += AddCount;
    }
    public override void RemoveEvent()
    {
        Placer.OnToggleRotationSnap -= AddCount;
    }
}
