using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KichenTool : MonoBehaviour
{ 
    KichenToolInfo info;
    public KichenToolInfo Info => info;
    public void Init(KichenToolInfo info )
    {
         this.info = info;
    }
}
