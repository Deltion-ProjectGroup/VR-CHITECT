using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Attributes
{
    public static bool ToggleBool(this bool boolToToggle)
    {
        if (boolToToggle)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
