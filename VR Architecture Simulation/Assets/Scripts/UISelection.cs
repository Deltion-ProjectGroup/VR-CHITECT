using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using UnityEngine.UI;

public class UISelection : MonoBehaviour
{

    public List<TwoDemensionalGOList> selectableOptions = new List<TwoDemensionalGOList>();
    int currentHorIndex;
    int currentVerIndex;
    UIButton currentSelected;
    [SerializeField]Vector2 outlineScale;
    [SerializeField]SteamVR_Action_Boolean acceptButton, selectButton;
    [SerializeField]SteamVR_Action_Vector2 trackpadPos;

    // Start is called before the first frame update
    void Start()
    {
        currentSelected = selectableOptions[currentVerIndex].xIndexes[currentHorIndex].GetComponent<UIButton>();
        Outline outline = currentSelected.gameObject.AddComponent<Outline>();
        outline.effectDistance = outlineScale;
    }

    // Update is called once per frame
    void Update()
    {
        SelectionNavigation();
        if (Input.GetKeyDown(KeyCode.KeypadEnter) || acceptButton.GetStateDown(InputMan.rightHand))
        {
            currentSelected.Interact();
        }
    }
    void SelectionNavigation()
    {
        //VR
        Vector2 changeAmount = new Vector2();
        if (selectButton.GetLastStateDown(InputMan.rightHand))
        {
            int rawAxisX = Mathf.RoundToInt(trackpadPos.axis.x);
            int rawAxisY = Mathf.RoundToInt(trackpadPos.axis.y);
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
                changeAmtPC.y += 1;
            }
            else
            {
                changeAmtPC.y -= 1;
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
        currentVerIndex -= (int)changeAmount.y;
        Destroy(currentSelected.gameObject.GetComponent<Outline>());
        if(currentVerIndex < 0)
        {
            currentVerIndex = selectableOptions.Count - 1;
        }
        else
        {
            if(currentVerIndex >= selectableOptions.Count)
            {
                currentVerIndex = 0;
            }
        }

        currentHorIndex += (int)changeAmount.x;
        if (currentHorIndex < 0)
        {
            currentHorIndex = selectableOptions[currentVerIndex].xIndexes.Count - 1;
        }
        else
        {
            if (currentHorIndex >= selectableOptions[currentVerIndex].xIndexes.Count)
            {
                currentHorIndex = 0;
            }
        }

        currentSelected = selectableOptions[currentVerIndex].xIndexes[currentHorIndex].GetComponent<UIButton>();
        Outline outline = currentSelected.gameObject.AddComponent<Outline>();
        outline.effectDistance = outlineScale;
        print(currentSelected.gameObject.name);
    }
    [System.Serializable]
    public struct TwoDemensionalGOList
    {
        public List<GameObject> xIndexes;
    }
}
