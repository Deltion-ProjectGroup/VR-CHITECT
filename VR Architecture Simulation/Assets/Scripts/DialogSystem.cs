using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OVR;

public class DialogSystem : MonoBehaviour
{
    public DialogEvent dialog;
    string[] dialogTexts;
    bool stayWhenDone;
    int currentDialog;
    public Text dialogTextHolder;
    public OVRInput.Button interactSource;
    public bool isEnabled;
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        if (isEnabled)
        {
            if (OVRInput.GetDown(InputMan.GetButton(interactSource)))
            {
                currentDialog++;
                if (currentDialog >= dialogTexts.Length)
                {
                    Player.isEnabled = true;
                    Placer.isEnabled = true;
                    isEnabled = false;
                    print("ENABLED PLACER");
                    if (!stayWhenDone)
                    {
                        gameObject.SetActive(false);
                    }
                    if (dialog.OnCompleteEvent != null)
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
    }


    public void StartDialog(DialogEvent newDialog)
    {
        isEnabled = true;
        Player.isEnabled = false;
        Placer.isEnabled = false;
        print("DISABLED PLACER");
        currentDialog = 0;
        dialog = newDialog;
        dialogTexts = dialog.dialogTexts;
        stayWhenDone = dialog.stayAfterFinish;
        dialogTextHolder.text = dialogTexts[currentDialog];
    }
}
