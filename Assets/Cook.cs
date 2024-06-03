using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cook : MonoBehaviour
{
    public DialogueBubble dialogueBubble;
    private int cookAnimator = 0;
    private Animator animator;
    private void Start()
    {
        animator= GetComponentInChildren<Animator>();
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

    public void StartCook()
    {
        cookAnimator++;
        animator.SetInteger("cook",cookAnimator);
    }

    public void StopCook()
    {
        cookAnimator--;
        animator.SetInteger("cook",cookAnimator);
        
    }
}
