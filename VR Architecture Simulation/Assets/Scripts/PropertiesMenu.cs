using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropertiesMenu : UIMenu
{
    public GameObject targetRN;

    UISelection uiSelection;
    GameObject currentPart;
    [SerializeField] Transform materialHolder, tabHolder;
    [SerializeField] GameObject materialButton, tabButton;
    List<GameObject> activeMaterialButtons = new List<GameObject>(), activeTabButtons = new List<GameObject>();
    // Start is called before the first frame update
    void Awake()
    {
        uiSelection = GetComponent<UISelection>();
    }

    public void Initialize(GameObject target)
    {
        if(uiSelection.selectableOptions.Count > 0)
        {
            uiSelection.selectableOptions[0].xIndexes.Clear();
        }
        int backupCount = activeTabButtons.Count;
        for (int i = 0; i < backupCount; i++)
        {
            Destroy(activeTabButtons[0]);
            activeTabButtons.RemoveAt(0);
        }
        for(int i = 0; i < target.transform.childCount; i++)
        {
            GameObject newButton = Instantiate(tabButton, tabHolder);
            newButton.GetComponent<PropertieTabData>().Initialize(target.transform.GetChild(i).gameObject, this);
            activeTabButtons.Add(newButton);
            uiSelection.selectableOptions[0].xIndexes.Add(newButton);
        }
        UpdateProperties(activeTabButtons[0].GetComponent<PropertieTabData>().holdingPart);
    }
    public void UpdateProperties(GameObject target)
    {
        currentPart = target;
        if (uiSelection.selectableOptions.Count > 0)
        {
            uiSelection.selectableOptions[uiSelection.selectableOptions.Count - 1].xIndexes.Clear();
        }
        if(activeMaterialButtons.Count > 0)
        {
            int backupCount = activeMaterialButtons.Count;
            for (int i = 0; i < backupCount; i++)
            {
                print(activeMaterialButtons.Count);
                Destroy(activeMaterialButtons[0]);
                activeMaterialButtons.RemoveAt(0);
            }
        }
        for(int i = 0; i < target.GetComponent<PartData>().availableMaterials.Length; i++)
        {
            print(target.GetComponent<PartData>().availableMaterials.Length);
            GameObject newMaterialButton = Instantiate(materialButton, materialHolder);
            newMaterialButton.GetComponent<PropertieMatData>().Initialize(target.GetComponent<PartData>().availableMaterials[i], this);
            activeMaterialButtons.Add(newMaterialButton);
            uiSelection.selectableOptions[uiSelection.selectableOptions.Count - 1].xIndexes.Add(newMaterialButton);
        }
    }
    public void ChangeMaterial(Material newMaterial)
    {
        foreach(Placer.PlacementPart placementPart in Placer.placer.ogPartData)
        {
            if(placementPart.part == currentPart)
            {
                placementPart.ogMaterial = newMaterial;
            }
        }
    }
    public override IEnumerator Open()
    {
        Initialize(targetRN);
        yield return null;
        UIManager.uiManager.canToggle = true;
    }
    public override void InstantClose()
    {
        gameObject.SetActive(false);
    }
}
