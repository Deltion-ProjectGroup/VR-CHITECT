using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SetTrackingObjectEvent", menuName = "New Event/Action/SetTrackingObject")]
public class SetTrackingObjectEvent : CountEvent
{
    public override void Activate()
    {
        Placer.OnSetObject += AddCount;
    }
    public override void RemoveEvent()
    {
        Placer.OnSetObject -= AddCount;
    }
}
