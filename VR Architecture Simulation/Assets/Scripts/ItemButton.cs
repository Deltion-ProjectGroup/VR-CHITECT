using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    public Item itemData;
    public Image holderImage;
    public void Select()
    {
        if (Placer.placer.canSetObject)
        {
            UIManager.uiManager.ToggleMenu(UIManager.uiManager.shop);
            Placer.placer.SetTrackingObject(Instantiate(itemData.itemObject));
        }
    }
    public void Initialize()
    {
        holderImage.sprite = itemData.itemIcon;
    }
}
