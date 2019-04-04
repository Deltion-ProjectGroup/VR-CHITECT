using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staircase : Interactable
{
    public Transform upstairs, downstairs;
    public override void Interact()
    {
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        float upstairsDistance;
        upstairsDistance = Mathf.Abs(upstairs.position.y - player.position.y);
        if(upstairsDistance < Mathf.Abs(downstairs.position.y - player.position.y))
        {
            player.position = downstairs.position;
        }
        else
        {
            player.position = upstairs.position;
        }
    }
}
