using UnityEngine;
using UnityEngine.UI;

public class PropertieTabData : MonoBehaviour
{
    public GameObject holdingPart;
    PropertiesMenu menu;
    // Start is called before the first frame update


    public void Select()
    {
        menu.UpdateProperties(holdingPart);
    }


    
    public void Initialize(GameObject thisPart, PropertiesMenu propMenu)
    {
        menu = propMenu;
        holdingPart = thisPart;
        //GetComponent<Image>().sprite = holdingPart.GetComponent<PartData>().partIcon;
    }
}
