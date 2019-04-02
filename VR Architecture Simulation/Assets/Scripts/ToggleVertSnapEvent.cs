using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VertSnapEvent", menuName = "New Event/Action/VertSnap")]
public class ToggleVertSnapEvent : CountEvent
{
    public override void Activate()
    {
        Player.OnVertSnap += AddCount;
    }
    public override void RemoveEvent()
    {
        Player.OnVertSnap -= AddCount;
    }
}
