using UnityEngine;

public class ItemButton : MonoBehaviour
{
    public Item itemData;

    public void Select()
    {
        if (Placer.placer.canSetObject)
        {
            UIManager.uiManager.ToggleMenu(UIManager.uiManager.shop);
            Placer.placer.SetTrackingObject(Instantiate(itemData.itemObject));
        }
    }
}
