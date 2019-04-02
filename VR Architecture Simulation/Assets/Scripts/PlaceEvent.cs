using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New PlaceEvent", menuName = "New Event/Action/PlaceObject")]
public class PlaceEvent : CountEvent
{
    public override void Activate()
    {
        Placer.OnPlaceObject += AddCount;
    }
    public override void RemoveEvent()
    {
        Placer.OnPlaceObject -= AddCount;
    }
}
