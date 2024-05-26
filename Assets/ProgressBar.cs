using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Image fill;

    public TMP_Text label;
    
    public  void SetProgress(float progress, float max)
    {
        fill.fillAmount = (float)progress / max;
        if (label)
        {
            
            label.text = $"{progress} / {max}";
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}