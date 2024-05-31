using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopupManager : Singleton<PopupManager>
{
    public TMP_Text label;
    public GameObject popup;
    public float showTime = 3f;
    public void Show(string text)
    {
        label.text = text;
        popup.SetActive(true);
        StartCoroutine(Hide());
    }
    IEnumerator Hide()
    {
        yield return new WaitForSeconds(showTime);
        popup.SetActive(false);
    }
}
