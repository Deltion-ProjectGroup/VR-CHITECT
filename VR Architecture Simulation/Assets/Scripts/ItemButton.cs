using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemButton : MonoBehaviour
{
    public Item itemData;


    public void InitializeButton()
    {

    }
    public void Select()
    {
        if (Placer.placer.canSetObject)
        {
            Placer.placer.SetTrackingObject(Instantiate(itemData.itemObject));
            UIManager.uiManager.ToggleMenu(UIManager.uiManager.shop);
        }
    }
}
