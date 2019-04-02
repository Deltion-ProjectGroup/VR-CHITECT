using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New DestroyEvent", menuName = "New Event/Action/DestroyObject")]
public class DestroyEvent : CountEvent
{
    public override void Activate()
    {
        Placer.OnDestroyObject += AddCount;
    }
    public override void RemoveEvent()
    {
        Placer.OnDestroyObject -= AddCount;
    }
}
