﻿using UnityEngine;
using Valve.VR;

public static class InputMan
{
    public static SteamVR_Input_Sources rightHand = SteamVR_Input_Sources.RightHand;
    public static SteamVR_Input_Sources leftHand = SteamVR_Input_Sources.LeftHand;

    
    public static void ChangePrimaryHand(bool rightHanded)
    {
        if (rightHanded)
        {
            rightHand = SteamVR_Input_Sources.RightHand;
            leftHand = SteamVR_Input_Sources.LeftHand;
        }
        else
        {
            leftHand = SteamVR_Input_Sources.RightHand;
            rightHand = SteamVR_Input_Sources.LeftHand;
        }
        Debug.Log("OG RIGHT HAND IS NOW " + rightHand.ToString());
        Debug.Log("OG LEFT HAND IS NOW " + leftHand.ToString());
    }
    public static SteamVR_Input_Sources GetHand(SteamVR_Input_Sources ogSource)
    {
        switch (ogSource)
        {
            case SteamVR_Input_Sources.RightHand:
                return rightHand;

            case SteamVR_Input_Sources.LeftHand:
                return leftHand;

        }
        return rightHand;
    }
}