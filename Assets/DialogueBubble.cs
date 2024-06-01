using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueBubble : MonoBehaviour
{
    public GameObject bubbleOb;
    public TMP_Text label;

    public void showDialogue(string text,float lastTime = -1)
    {
        bubbleOb.SetActive(true);
        label.text = text;

        if (lastTime > 0)
        {
            Invoke("hideDialogue", lastTime);
        }
    }

    public void hideDialogue()
    {
        if (this && bubbleOb)
        {
            
            bubbleOb.SetActive(false);
        }
    }
}
