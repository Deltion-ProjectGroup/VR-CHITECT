using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New TogglePosSnapEvent", menuName = "New Event/Action/Toggle/PositionSnap")]
public class TogglePositionSnapEvent : CountEvent
{
    public override void Activate()
    {
        Placer.OnTogglePositionSnap += AddCount;
    }
    public override void RemoveEvent()
    {
        Placer.OnTogglePositionSnap -= AddCount;
    }
}
