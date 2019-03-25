using UnityEngine;
using UnityEngine.UI;

public class PropertieMatData : MonoBehaviour
{
    public Image materialColor;
    public Material containedMaterial;
    PropertiesMenu menu;

    public void Select()
    {
        print("Selected");
        menu.ChangeMaterial(containedMaterial);
    }



    public void Initialize(Material thisMaterial, PropertiesMenu propMenu, Color buttonColor)
    {
        menu = propMenu;
        containedMaterial = thisMaterial;
        materialColor.color = buttonColor;
    }
}
