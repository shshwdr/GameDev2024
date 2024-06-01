using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cook : MonoBehaviour
{
    public DialogueBubble dialogueBubble;

    private void Start()
    {
        
        HideDialogue();
    }

    public void HideDialogue()
    {
        
        dialogueBubble.hideDialogue();
    }
    public void ShowDialogue(string dialogue)
    {
        dialogueBubble.showDialogue(dialogue);
    }
}
