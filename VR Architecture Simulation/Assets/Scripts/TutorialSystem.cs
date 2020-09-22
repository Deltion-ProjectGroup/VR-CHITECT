using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSystem : MonoBehaviour
{
    public CustomEvent[] tutorialParts;
    public int currentPart;
    // Start is called before the first frame update
    void Start()
    {
        StartTutorial();
    }

    public void StartTutorial()
    {
        tutorialParts[currentPart].OnCompleteEvent += NextTutorialPart;
        tutorialParts[currentPart].Activate();
    }
    public void NextTutorialPart()
    {
        tutorialParts[currentPart].OnCompleteEvent -= NextTutorialPart;
        if(currentPart + 1 < tutorialParts.Length)
        {
            currentPart++;
            tutorialParts[currentPart].OnCompleteEvent += NextTutorialPart;
            tutorialParts[currentPart].Activate();
        }
    }
}
