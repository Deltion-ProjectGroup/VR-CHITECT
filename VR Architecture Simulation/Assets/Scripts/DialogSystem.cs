using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class DialogSystem : MonoBehaviour
{
    public DialogEvent dialog;
    string[] dialogTexts;
    bool stayWhenDone;
    int currentDialog;
    public Text dialogTextHolder;
    public SteamVR_Input_Sources interactSource;
    public SteamVR_Action_Boolean interactButton;
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        if (interactButton.GetStateDown(InputMan.GetHand(interactSource)))
        {
            currentDialog++;
            if(currentDialog >= dialogTexts.Length)
            {
                if (!stayWhenDone)
                {
                    gameObject.SetActive(false);
                }
                if(dialog.OnCompleteEvent != null)
                {
                    dialog.OnCompleteEvent();
                }
            }
            else
            {
                dialogTextHolder.text = dialogTexts[currentDialog];
            }
        }
    }


    public void StartDialog(DialogEvent newDialog)
    {
        currentDialog = 0;
        dialog = newDialog;
        dialogTexts = dialog.dialogTexts;
        stayWhenDone = dialog.stayAfterFinish;
        dialogTextHolder.text = dialogTexts[currentDialog];
    }
}
