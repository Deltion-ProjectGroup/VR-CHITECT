using UnityEngine;
using OVR;

public static class InputMan
{
    public static bool rightHanded = true;

    
    public static void ChangePrimaryHand(bool _rightHanded)
    {
        rightHanded = _rightHanded;
        OVRInput.Axis1D aw;
    }

    public static OVRInput.Button GetButton(OVRInput.Button ogInput)
    {
        OVRInput.Button newInput = ogInput;

        if (!rightHanded)
        {
            switch (ogInput)
            {
                case OVRInput.Button.One:
                    newInput = OVRInput.Button.Three;
                    break;
                case OVRInput.Button.Two:
                    newInput = OVRInput.Button.Four;
                    break;

                case OVRInput.Button.Three:
                    newInput = OVRInput.Button.One;
                    break;
                case OVRInput.Button.Four:
                    newInput = OVRInput.Button.Two;
                    break;

                case OVRInput.Button.PrimaryHandTrigger:
                    newInput = OVRInput.Button.SecondaryHandTrigger;
                    break;
                case OVRInput.Button.PrimaryIndexTrigger:
                    newInput = OVRInput.Button.SecondaryIndexTrigger;
                    break;
                case OVRInput.Button.PrimaryThumbstick:
                    newInput = OVRInput.Button.SecondaryThumbstick;
                    break;

                case OVRInput.Button.SecondaryHandTrigger:
                    newInput = OVRInput.Button.PrimaryHandTrigger;
                    break;
                case OVRInput.Button.SecondaryIndexTrigger:
                    newInput = OVRInput.Button.PrimaryIndexTrigger;
                    break;
                case OVRInput.Button.SecondaryThumbstick:
                    newInput = OVRInput.Button.PrimaryThumbstick;
                    break;
            }
        }

        return newInput;
    }

    public static OVRInput.Axis2D GetAxis2D(OVRInput.Axis2D ogInput)
    {
        OVRInput.Axis2D newInput = ogInput;

        if (!rightHanded)
        {
            switch (ogInput)
            {
                case OVRInput.Axis2D.PrimaryThumbstick:
                    newInput = OVRInput.Axis2D.SecondaryThumbstick;
                    break;
                case OVRInput.Axis2D.SecondaryThumbstick:
                    newInput = OVRInput.Axis2D.PrimaryThumbstick;
                    break;

                case OVRInput.Axis2D.PrimaryTouchpad:
                    newInput = OVRInput.Axis2D.SecondaryTouchpad;
                    break;
                case OVRInput.Axis2D.SecondaryTouchpad:
                    newInput = OVRInput.Axis2D.PrimaryTouchpad;
                    break;
            }
        }

        return newInput;
    }

    public static OVRInput.Axis1D GetAxis1D(OVRInput.Axis1D ogInput)
    {
        OVRInput.Axis1D newInput = ogInput;

        if (!rightHanded)
        {
            switch (ogInput)
            {
                case OVRInput.Axis1D.PrimaryHandTrigger:
                    newInput = OVRInput.Axis1D.SecondaryHandTrigger;
                    break;
                case OVRInput.Axis1D.SecondaryHandTrigger:
                    newInput = OVRInput.Axis1D.PrimaryHandTrigger;
                    break;

                case OVRInput.Axis1D.PrimaryIndexTrigger:
                    newInput = OVRInput.Axis1D.SecondaryIndexTrigger;
                    break;
                case OVRInput.Axis1D.SecondaryIndexTrigger:
                    newInput = OVRInput.Axis1D.PrimaryIndexTrigger;
                    break;
            }
        }

        return newInput;
    }
}
