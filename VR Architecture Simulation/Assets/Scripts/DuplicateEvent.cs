using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New DuplicateEvent", menuName = "New Event/Action/Duplicate Object")]
public class DuplicateEvent : CountEvent
{
    public override void Activate()
    {
        Player.OnDuplicate += AddCount;
    }
    public override void RemoveEvent()
    {
        Player.OnDuplicate -= AddCount;
    }
}
