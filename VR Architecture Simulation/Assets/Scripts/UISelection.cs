using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using UnityEngine.UI;

public class UISelection : MonoBehaviour
{

    public List<TwoDemensionalGOList> selectableOptions = new List<TwoDemensionalGOList>();
    sbyte currentHorIndex;
    sbyte currentVerIndex;
    public UIButtonBase currentSelected;
    [SerializeField] bool autoSelect;
    [SerializeField]Vector2 outlineScale;
    [SerializeField]SteamVR_Action_Boolean acceptButton, selectButton;
    [SerializeField]SteamVR_Action_Vector2 trackpadPos;
    public SelectionState selectionState;


    public enum SelectionState {Selecting, Frozen }
    // Start is called before the first frame update
    void Start()
    {

    }

    public void Initialize(bool resetPos = false)
    {
        if (resetPos)
        {
            currentHorIndex = default;
            currentVerIndex = default;
        }
        currentSelected = selectableOptions[currentVerIndex].xIndexes[currentHorIndex].GetComponent<UIButtonBase>();
        Outline newOutline = currentSelected.gameObject.AddComponent<Outline>();
        newOutline.effectDistance = outlineScale;
    }

    // Update is called once per frame

    void Update()
    {
        if(selectionState == SelectionState.Selecting)
        {
            SelectionNavigation();
        }
        if (!autoSelect)
        {
            if (Input.GetKeyDown(KeyCode.Q) || acceptButton.GetStateDown(InputMan.rightHand))
            {
                currentSelected.Interact();
            }
        }
    }
    void SelectionNavigation()
    {
        //VR
        Vector2 changeAmount = new Vector2();
        if (selectButton.GetLastStateDown(InputMan.rightHand))
        {
            sbyte rawAxisX = (sbyte)Mathf.RoundToInt(trackpadPos.axis.x);
            sbyte rawAxisY = (sbyte)Mathf.RoundToInt(trackpadPos.axis.y);
            if (rawAxisX != 0)
            {
                changeAmount.x = rawAxisX;
            }
            if (rawAxisY != 0)
            {
                changeAmount.y = rawAxisY;
            }
            ChangeSelectPos(changeAmount);
        }

        //PC
        Vector2 changeAmtPC = new Vector2();
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                changeAmtPC.x += 1;
            }
            else
            {
                changeAmtPC.x -= 1;
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                changeAmtPC.y -= 1;
            }
            else
            {
                changeAmtPC.y += 1;
            }
        }
        if (changeAmtPC != Vector2.zero)
        {
            ChangeSelectPos(changeAmtPC);
        }
    }
    public void ChangeSelectPos(Vector2 changeAmount)
    {
        print(changeAmount);
        currentVerIndex -= (sbyte)changeAmount.y;
        Destroy(currentSelected.gameObject.GetComponent<Outline>());
        if(currentVerIndex < 0)
        {
            currentVerIndex = (sbyte)(selectableOptions.Count - 1);
        }
        else
        {
            if(currentVerIndex >= selectableOptions.Count)
            {
                currentVerIndex = default;
            }
        }

        currentHorIndex += (sbyte)changeAmount.x;
        if (currentHorIndex < 0)
        {
            currentHorIndex = (sbyte)(selectableOptions[currentVerIndex].xIndexes.Count - 1);
        }
        else
        {
            if (currentHorIndex >= selectableOptions[currentVerIndex].xIndexes.Count)
            {
                currentHorIndex = default;
            }
        }

        currentSelected = selectableOptions[currentVerIndex].xIndexes[currentHorIndex].GetComponent<UIButtonBase>();
        Outline outline = currentSelected.gameObject.AddComponent<Outline>();
        outline.effectDistance = outlineScale;
        if (autoSelect)
        {
            currentSelected.Interact();
        }
        print(currentSelected.gameObject.name);
    }
    [System.Serializable]
    public struct TwoDemensionalGOList
    {
        public List<GameObject> xIndexes;
    }
    [System.Serializable]
    public struct TDGODataHolder
    {
        public List<TwoDemensionalGOList> data;
    }
}
