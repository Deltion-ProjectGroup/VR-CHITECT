using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New TeleportEvent", menuName = "New Event/Action/Teleport")]
public class TeleportEvent : CountEvent
{
    public override void Activate()
    {
        Player.OnTeleport += AddCount;
    }
    public override void RemoveEvent()
    {
        Player.OnTeleport -= CheckCount;
    }
}
