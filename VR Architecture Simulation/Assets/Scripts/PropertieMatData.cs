using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropertieMatData : MonoBehaviour
{
    public Material containedMaterial;
    public PropertiesMenu menu;

    public void Select()
    {
        menu.ChangeMaterial(containedMaterial);
    }



    public void Initialize(Material thisMaterial)
    {
        containedMaterial = thisMaterial;
    }
}
