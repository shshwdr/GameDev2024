using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KichenToolManager : Singleton<KichenToolManager>
{
    public Transform kichenToolParent;
    [HideInInspector]
    public List<Transform> kichenToolTransforms = new List<Transform>();
    
    public void Init()
    {
        foreach (Transform trans in kichenToolParent)
        {
            kichenToolTransforms.Add(trans);
            
        }
        
        
        int i = 0;
        foreach (var info in CSVLoader.Instance.KichenToolInfoDict.Values)
        {
            var kichenTool = Instantiate(Resources.Load<GameObject>("KichenTool/"+info.name), kichenToolTransforms[i]);
            kichenTool.GetComponent<KichenTool>().Init(info);
            i++;
        }
    }
}
