using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CountEvent : CustomEvent
{
    public int wantedCount;
    public int currentCount;
    public void AddCount()
    {
        currentCount++;
        CheckCount();
    }
    public void CheckCount()
    {
        if(currentCount >= wantedCount)
        {
            RemoveEvent();
            OnCompleteEvent();
        }
    }
    public abstract void RemoveEvent();
}
