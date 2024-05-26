using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        
        createKichenTool(CSVLoader.Instance.KichenToolInfoDict.Values.ToList()[0],kichenToolTransforms[i]);
        i++;
        foreach (var info in CSVLoader.Instance.KichenToolInfoDict.Values)
        {
            createKichenTool(info,kichenToolTransforms[i]);
            i++;
        }
    }

    void createKichenTool(KichenToolInfo info,Transform trans)
    {
        
        var kichenTool = Instantiate(Resources.Load<GameObject>("KichenTool/"+info.name), trans);
        kichenTool.GetComponent<KichenTool>().Init(info);
    }
}
