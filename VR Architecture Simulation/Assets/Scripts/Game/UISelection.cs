using System.Collections.Generic;
using UnityEngine;
using OVR;
using UnityEngine.UI;

public class UISelection : MonoBehaviour
{

    public List<TwoDemensionalGOList> selectableOptions = new List<TwoDemensionalGOList>();
    sbyte currentHorIndex;
    sbyte currentVerIndex;
    public UIButtonBase currentSelected;
    [SerializeField] bool autoSelect;
    [SerializeField] OVRInput.Button selectButton;
    [SerializeField] OVRInput.Axis2D trackpadButton;
    public SelectionState selectionState;
    public AudioSource mainAudioSource;
    public AudioClip switchSound, selectSound;

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
        if (currentSelected)
        {
            currentSelected.GetComponent<UIButtonBase>().OnHoverEnd();
        }
        currentSelected = selectableOptions[currentVerIndex].xIndexes[currentHorIndex].GetComponent<UIButtonBase>();
        currentSelected.GetComponent<UIButtonBase>().OnHover();
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
            if (OVRInput.GetDown(InputMan.GetButton(selectButton)))
            {
                currentSelected.Interact();
                mainAudioSource.clip = selectSound;
                mainAudioSource.Play();
            }
        }
    }
    void SelectionNavigation()
    {
        /*PC
        Vector2 changeAmt = new Vector2();
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                changeAmt.x += 1;
            }
            else
            {
                changeAmt.x -= 1;
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                changeAmt.y -= 1;
            }
            else
            {
                changeAmt.y += 1;
            }
        }*/

        Vector2 changeAmt = OVRInput.Get(trackpadButton);
        changeAmt.x = Mathf.Round(changeAmt.x);
        changeAmt.y = Mathf.Round(changeAmt.y);


        if (changeAmt != Vector2.zero)
        {
            ChangeSelectPos(changeAmt);
        }
    }
    public void ChangeSelectPos(Vector2 changeAmount)
    {
        print(changeAmount);
        currentVerIndex -= (sbyte)changeAmount.y;
        currentSelected.GetComponent<UIButtonBase>().OnHoverEnd();
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
        if (autoSelect)
        {
            currentSelected.Interact();
            mainAudioSource.clip = selectSound;
            mainAudioSource.Play();
        }
        currentSelected.GetComponent<UIButtonBase>().OnHover();
        mainAudioSource.clip = switchSound;
        mainAudioSource.Play();
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
