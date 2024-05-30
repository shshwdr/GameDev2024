using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KichenToolManager : Singleton<KichenToolManager>
{
    public Transform kichenToolParent;
    [HideInInspector]
    public List<Transform> kichenToolTransforms = new List<Transform>();
    public List<string> ownedTools = new List<string>();
    public List<string> toBuyTools = new List<string>();

    public void AddUtil()
    {
        var tool = toBuyTools[0];
        toBuyTools.RemoveAt(0);
        
        var info = CSVLoader.Instance.KichenToolInfoDict[tool];
        createKichenTool(info,kichenToolTransforms[ownedTools.Count]);
        ownedTools.Add(tool);
    }
    public void Init()
    {
        foreach (Transform trans in kichenToolParent)
        {
            kichenToolTransforms.Add(trans);
            
        }

        foreach (var pair in CSVLoader.Instance.KichenToolInfoDict)
        {
            if (pair.Value.startCount == 1)
            {
                ownedTools.Add(pair.Key);
            }
            else
            {
                toBuyTools.Add(pair.Key);
            }
        }
        
        int i = 0;
        
        //createKichenTool(CSVLoader.Instance.KichenToolInfoDict.Values.ToList()[0],kichenToolTransforms[i]);
        //i++;
        foreach (var toolName in ownedTools)
        {
            var info = CSVLoader.Instance.KichenToolInfoDict[toolName];
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
