using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
