using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBubble : MonoBehaviour
{
    public GameObject bubbleOb;
    public TMP_Text label;

    public void showDialogue(string text,float lastTime = -1)
    {
        text = Regex.Replace(text, @"\\n", System.Environment.NewLine);

        label.text = text;
        bubbleOb.SetActive(true);
        GetComponent<CanvasGroup>().alpha = 0;
        bubbleOb.GetComponentInChildren<ContentSizeFitter>().enabled = false;
        Invoke("ActualShow", 0.1f);
        if (lastTime > 0)
        {
            Invoke("hideDialogue", lastTime);
        }
    }

    public void ActualShow()
    {
        GetComponent<CanvasGroup>().alpha = 1;
        bubbleOb.SetActive(true);
        bubbleOb.SetActive(false);
        bubbleOb.SetActive(true);
        bubbleOb.GetComponentInChildren<ContentSizeFitter>().enabled = true;
    }

    public void hideDialogue()
    {
        if (this && bubbleOb)
        {
            
            bubbleOb.SetActive(false);
        }
    }
}
