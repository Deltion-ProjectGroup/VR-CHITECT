using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PropertiesMenu : MonoBehaviour
{
    public GameObject targetRN;

    public UISelection uiSelection;
    public GameObject currentPart;
    public Transform materialHolder, tabHolder;
    public GameObject materialButton, tabButton;
    public List<GameObject> activeMaterialButtons, activeTabButtons = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        uiSelection = GetComponent<UISelection>();
        Initialize(targetRN);
    }

    // Update is called once per frame
    void Update()
    {
        
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
            newButton.GetComponent<PropertieTabData>().menu = this;
            newButton.GetComponent<PropertieTabData>().Initialize(target.transform.GetChild(i).gameObject);
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
            newMaterialButton.GetComponent<PropertieMatData>().menu = this;
            newMaterialButton.GetComponent<PropertieMatData>().Initialize(target.GetComponent<PartData>().availableMaterials[i]);
            activeMaterialButtons.Add(newMaterialButton);
            uiSelection.selectableOptions[uiSelection.selectableOptions.Count - 1].xIndexes.Add(newMaterialButton);
        }
    }
    public void ChangeMaterial(Material newMaterial)
    {
        currentPart.GetComponent<Renderer>().material = newMaterial;
    }
}
