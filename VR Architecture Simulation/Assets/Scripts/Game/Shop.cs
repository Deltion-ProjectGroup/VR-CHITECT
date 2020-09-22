using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OVR;
using UnityEngine.UI;

public class Shop : UIMenu
{
    public Placer placingSystem;
    [SerializeField] OVRInput.Axis2D changeTabTrackpad;
    [SerializeField] OVRInput.Button changeTabButton, selectButton;
    sbyte selectedHorIndex;
    sbyte selectedVerIndex;
    float verTileDistance;
    [SerializeField] GameObject[] selectionTabs;
    [SerializeField] Animation[] indicatorHolders;
    List<GameObject> shopButtons = new List<GameObject>();
    [SerializeField] Transform sectionHolder;
    [SerializeField] Transform itemHolder;
    [SerializeField] float tickDelay;
    [SerializeField] sbyte ticks;
    [SerializeField] GameObject shopItem;
    public bool canMove = true;
    bool doneRemoving;
    Vector3 requiredHorPos;
    Vector3 requiredVerPos;
    [Tooltip("Put in here the amount of items that fit")]
    public int possibleVisibleIcons;
    public Text tabTypeText;

    [SerializeField] AudioSource mainAudioSource;
    [SerializeField] AudioClip switchSound, arriveSwitchSound, selectSound;
    // Start is called before the first frame update
    void Awake()
    {
        List<GameObject> listedSelecTabs = new List<GameObject>();
        foreach(Transform child in sectionHolder)
        {
            listedSelecTabs.Add(child.gameObject);
        }
        selectionTabs = listedSelecTabs.ToArray();
        requiredHorPos = sectionHolder.localPosition;
        requiredVerPos = itemHolder.localPosition;
        tabTypeText.text = selectionTabs[selectedHorIndex].GetComponent<ShopTabData>().tabType;
    }

    // Update is called once per frame
    void Update()
    {
        //VR
        if (canMove)
        {
            ShopNavigation();
            if (OVRInput.GetDown(InputMan.GetButton(selectButton)))
            {
                shopButtons[selectedVerIndex].GetComponent<ItemButton>().Select();
                mainAudioSource.clip = selectSound;
                mainAudioSource.Play();
            }
        }


    }
    void ShopNavigation()
    {
        Vector2 moveAmount = OVRInput.Get(changeTabTrackpad);
        moveAmount.x = Mathf.Round(moveAmount.x);
        moveAmount.y = Mathf.Round(moveAmount.y);

        if(moveAmount.x != 0)
        {
            if(moveAmount.x > 0)
            {
                StartCoroutine(ChangeHorIndex(1));
            }
            else
            {
                StartCoroutine(ChangeHorIndex(-1));
            }
        }

        if (moveAmount.y != 0)
        {
            if (moveAmount.y > 0)
            {
                StartCoroutine(ChangeVerIndex(1));
            }
            else
            {
                StartCoroutine(ChangeVerIndex(-1));
            }
        }

        /*PC
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                StartCoroutine(ChangeHorIndex(1));
            }
            else
            {
                StartCoroutine(ChangeHorIndex(-1));
            }
        }
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))
        {
            if (Input.GetKey(KeyCode.DownArrow))
            {
                StartCoroutine(ChangeVerIndex(1));
            }
            else
            {
                StartCoroutine(ChangeVerIndex(-1));
            }
        }*/
    }
    public void SpawnObject(GameObject objectToPlace)
    {
        placingSystem.SetTrackingObject(Instantiate(objectToPlace, Vector3.zero, Quaternion.identity));
    }
    IEnumerator ChangeHorIndex(sbyte changeAmount)
    {
        canMove = false;
        int previousHorIndex = selectedHorIndex;
        selectedHorIndex += changeAmount;
        if(selectedHorIndex < 0)
        {
            selectedHorIndex = (sbyte)(selectionTabs.Length - 1);
        }
        else
        {
            if(selectedHorIndex >= selectionTabs.Length)
            {
                selectedHorIndex = default;
            }
        }
        float moveAmount = selectionTabs[previousHorIndex].transform.localPosition.x - selectionTabs[selectedHorIndex].transform.localPosition.x;
        requiredHorPos = sectionHolder.transform.localPosition;
        requiredHorPos.x += moveAmount;
        moveAmount /= ticks;
        StartCoroutine(ClearShopItems(false));
        mainAudioSource.clip = switchSound;
        mainAudioSource.Play();
        tabTypeText.text = selectionTabs[selectedHorIndex].GetComponent<ShopTabData>().tabType;
        for (sbyte i = 0; i < ticks; i++)
        {
            sectionHolder.localPosition += (new Vector3(moveAmount, 0));
            yield return new WaitForSeconds(tickDelay);
        }
        mainAudioSource.clip = arriveSwitchSound;
        mainAudioSource.Play();
        while (!doneRemoving)
        {
            yield return null;
        }
        StartCoroutine(UpdateShopItems());
        FixVerIndex();
        doneRemoving = false;
    }

    IEnumerator ChangeVerIndex(sbyte changeAmount)
    {
        canMove = false;
        int previousVerIndex = selectedVerIndex;
        selectedVerIndex += changeAmount;
        if (selectedVerIndex < 0)
        {
            selectedVerIndex = (sbyte)(shopButtons.Count - 1);
        }
        else
        {
            if (selectedVerIndex >= shopButtons.Count)
            {
                selectedVerIndex = 0;
            }
        }
        float moveAmount = shopButtons[previousVerIndex].transform.localPosition.y - shopButtons[selectedVerIndex].transform.localPosition.y;
        requiredVerPos = itemHolder.localPosition;
        requiredVerPos.y += moveAmount;
        moveAmount /= ticks;

        for (int i = 0; i < ticks; i++)
        {
            itemHolder.localPosition += (new Vector3(0, moveAmount));
            yield return new WaitForSeconds(tickDelay);
        }
        canMove = true;
    }
    void FixVerIndex()
    {
        if(selectedVerIndex >= shopButtons.Count)
        {
            float newVal = CalcVerDistance();
            newVal *= selectedVerIndex - (shopButtons.Count - 1);
            itemHolder.localPosition += new Vector3(0, newVal);
            selectedVerIndex = (sbyte)(shopButtons.Count - 1);
            print(newVal);
        }
    }
    float CalcVerDistance()
    {
        verTileDistance = -(shopItem.GetComponent<RectTransform>().rect.height + itemHolder.GetComponent<VerticalLayoutGroup>().spacing);
        return (verTileDistance);
    }
    IEnumerator UpdateShopItems()
    {
        GameObject newItem = null;
        for(int i = 0; i < selectionTabs[selectedHorIndex].GetComponent<ShopTabData>().tabItems.Length; i++)
        {
            Item item = selectionTabs[selectedHorIndex].GetComponent<ShopTabData>().tabItems[i];
            newItem = Instantiate(shopItem, itemHolder);
            shopButtons.Add(newItem);
            newItem.GetComponent<ItemButton>().itemData = item;
            newItem.GetComponent<ItemButton>().Initialize();
            if (shopButtons.Count <= possibleVisibleIcons)
            {
                newItem.GetComponent<Animation>().Play("ShopItemAppear");
                yield return new WaitForSeconds(newItem.GetComponent<Animation>().clip.length / 4);
            }
        }
        if(newItem != null)
        {
            yield return new WaitForSeconds(newItem.GetComponent<Animation>().clip.length / 4 * 3);
        }
        canMove = true;
        UIManager.uiManager.canToggle = true;
    }
    public override IEnumerator Open()
    {
        canMove = false;
        for(byte i = 0; i < indicatorHolders.Length; i++)
        {
            indicatorHolders[i].Play();
        }
        for(byte i = 0; i < selectionTabs.Length; i++)
        {
            if(i >= selectedHorIndex - 1)
            {
                selectionTabs[i].GetComponent<Animation>().Play();
                yield return new WaitForSeconds(selectionTabs[i].GetComponent<Animation>().clip.length / 4);
            }
            else
            {
                selectionTabs[i].transform.localScale = Vector3.one;
            }
        }
        StartCoroutine(UpdateShopItems());
    }
    IEnumerator ClearShopItems(bool open)
    {
        for(int i = shopButtons.Count - 1; i >= 0 ; i--)
        {
            GameObject button = shopButtons[i];
            if(i >= selectedVerIndex && i < selectedVerIndex + possibleVisibleIcons)
            {
                button.GetComponent<Animation>().Play("ShopItemDisappear");
                yield return new WaitForSeconds(button.GetComponent<Animation>().GetClip("ShopItemDisappear").length);
            }
            Destroy(button);
            shopButtons.RemoveAt(i);
        }
        shopButtons = new List<GameObject>();
        doneRemoving = true;
    }
    public override void InstantClose()
    {
        StopAllCoroutines();
        canMove = true;
        doneRemoving = false;
        sectionHolder.transform.localPosition = requiredHorPos;
        itemHolder.transform.localPosition = requiredVerPos;
        FixVerIndex();
        for (int i = shopButtons.Count - 1; i >= 0; i--)
        {
            GameObject button = shopButtons[i];
            Destroy(button);
            shopButtons.RemoveAt(i);
        }
        for(sbyte i = 0; i < selectionTabs.Length; i++)
        {
            selectionTabs[i].transform.localScale = Vector3.zero;
        }
        gameObject.SetActive(false);
    }
}
