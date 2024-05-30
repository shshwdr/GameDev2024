using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CheatMenu : MonoBehaviour
{
    public GameObject prefab;
    public Transform parent;

    public void Start()
    {
        var go = Instantiate(prefab, parent);
        go.GetComponent<Button>().onClick.AddListener(RandomGenerateDishes);
        go.GetComponentInChildren<TMP_Text>().text = "Generate Dishes";
        
        go = Instantiate(prefab, parent);
        go.GetComponent<Button>().onClick.AddListener(GetMoney);
        go.GetComponentInChildren<TMP_Text>().text = "Get Money";
        
     
        go = Instantiate(prefab, parent);
        go.GetComponent<Button>().onClick.AddListener(FinishTurn);
        go.GetComponentInChildren<TMP_Text>().text = "FinishTurn";
    }

    public void FinishTurn()
    {
        
        RoundManager.Instance.FinishBattle();
    }

    public void GetMoney()
    {
        RoundManager.Instance.AddMoney(10000);
    }

    public void RandomGenerateDishes()
    {
        foreach (var trans in KichenToolManager.Instance.kichenToolTransforms)
        {
            var tool = trans.childCount>0?trans.GetChild(0).GetComponent<KichenTool>():null;
            if (tool)
            {
                foreach (var dishInfo in CSVLoader.Instance.DishInfoDict.Values)
                {
                    if (dishInfo.kichenUtil == tool.Info.id)
                    {
                         tool.CreateDish(dishInfo);
                    }
                }
            }
        }
    }


    public void SuperSpeedup()
    {
        Time.timeScale = 5;
    }
}