using System.Collections.Generic;
using UnityEngine;

public class PropertiesMenu : MonoBehaviour
{
    public GameObject targetRN;

    UISelection uiSelection;
    GameObject currentPart;
    [SerializeField] Transform materialHolder, tabHolder;
    [SerializeField] GameObject materialButton, tabButton;
    List<GameObject> activeMaterialButtons, activeTabButtons = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        uiSelection = GetComponent<UISelection>();
        Initialize(targetRN);
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
        int backupCount = activeMaterialButtons.Count;
        for (int i = 0; i < backupCount; i++)
        {
            print(activeMaterialButtons.Count);
            Destroy(activeMaterialButtons[0]);
            activeMaterialButtons.RemoveAt(0);
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
        currentPart.GetComponent<Renderer>().material = newMaterial;
    }
}
