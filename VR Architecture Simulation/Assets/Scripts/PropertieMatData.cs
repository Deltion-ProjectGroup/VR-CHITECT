using UnityEngine;

public class PropertieMatData : MonoBehaviour
{
    public Material containedMaterial;
    PropertiesMenu menu;

    public void Select()
    {
        menu.ChangeMaterial(containedMaterial);
    }



    public void Initialize(Material thisMaterial, PropertiesMenu propMenu)
    {
        menu = propMenu;
        containedMaterial = thisMaterial;
    }
}
