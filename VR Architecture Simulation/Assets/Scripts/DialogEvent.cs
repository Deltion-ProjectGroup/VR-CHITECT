using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Dialog", menuName = "New Event/Create New Dialog")]
public class DialogEvent : Event
{
    [TextArea]
    public string[] dialogTexts;
    public bool stayAfterFinish;
    public UIManager.DialogSource source;
    public override void Activate()
    {
        GameObject.FindGameObjectWithTag("Manager").GetComponent<UIManager>().EnableDialogUI(this, source);
    }

}
