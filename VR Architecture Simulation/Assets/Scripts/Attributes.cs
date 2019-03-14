using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Attributes
{
    public static List<GameObject> allChildren = new List<GameObject>();
    public static bool ToggleBool(this bool boolToToggle)
    {
        return !boolToToggle;
    }

    public static GameObject GetAbsoluteParent(this GameObject start)
    {
        if(start.transform.parent != null)
        {
            return GetAbsoluteParent(start.transform.parent.gameObject);
        }
        return start;
    }
    public static GameObject[] GetAllChildren(this GameObject start, bool first = true)
    {
        if (first)
        {
            allChildren = null;
            foreach (Transform child in start.transform)
            {
                allChildren.Add(child.gameObject);
                GetAllChildren(child.gameObject, false);
            }
            return allChildren.ToArray();
        }
        foreach(Transform child in start.transform)
        {
            allChildren.Add(child.gameObject);
            GetAllChildren(child.gameObject, false);
        }
        return null;
    }

}
