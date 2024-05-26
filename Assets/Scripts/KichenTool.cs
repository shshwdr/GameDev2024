using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KichenTool : MonoBehaviour
{ 
    KichenToolInfo info;
    public KichenToolInfo Info => info;
    
    
    public Transform kichenToolParent;
    List<Transform> kichenToolTransforms = new List<Transform>();

    private DishInfo currentDishInfo;
    private bool isCooking = false;

    public ProgressBar progressBar;
    
    public void Init(KichenToolInfo info )
    {
         this.info = info;
         progressBar.gameObject.SetActive(false);
         
         foreach (Transform trans in kichenToolParent)
         {
             if (trans.gameObject.activeSelf)
             {
                 
                 kichenToolTransforms.Add(trans);
             }
            
         }
    }

    public void AddIngredient(IngredientBase ingredient)
    {
        ingredient.isInPot = true;
        foreach (var trans in kichenToolTransforms)
        {
            if (trans.childCount == 0)
            {
                ingredient.transform.SetParent((trans));
                ingredient.transform.position =  trans.position;
                return;
            }
        }
    }

    public bool CanAddIngredient(IngredientBase ingredient)
    {
        if (isCooking)
        {
            return false;
        }
        bool hasSlot = false;
        foreach (var trans in kichenToolTransforms)
        {
            if (trans.childCount == 0)
            {
                hasSlot = true;
                return true;
            }
        }

        return false;
    }

    public void RemoveIngredient(IngredientBase ingredient)
    {
        foreach (var trans in kichenToolTransforms)
        {
            if (trans.childCount != 0)
            {
                if (trans.GetChild(0) == ingredient)
                {
                    trans.parent = null;
                    
                    return;
                }
            }
        }

        if (true)
        {
            Debug.LogError(" remove ingredient failed");
        }
    }

    
    public void RemoveAllIngredient()
    {
        foreach (var trans in kichenToolTransforms)
        {
            if (trans.childCount != 0)
            {
                Destroy(trans.GetChild(0).gameObject);
            }
        }
    }

    public void Use()
    {
        
        bool hasSlot = false;
        foreach (var trans in kichenToolTransforms)
        {
            if (trans.childCount == 0)
            {
                hasSlot = true;
                return ;
            }
        }

        TryUse();
    } 
    void TryUse()
    {
        
        var currentIngredientBases = new List<string>();
                
        foreach (var trans in kichenToolTransforms)
        {
            currentIngredientBases.Add(trans.GetChild(0).GetComponent<IngredientBase>().Id);
        }

        currentIngredientBases.Sort();
        var currentIngredientBasesStr = string.Join(",", currentIngredientBases);

        RemoveAllIngredient();

        foreach (var dishInfo in CSVLoader.Instance.DishInfoDict.Values)
        {
            if (dishInfo.kichenUtil == info.id)
            {
                
                var  dishIngredientBases = dishInfo.ingredients;
                dishIngredientBases.Sort();
                var dishIngredientBasesStr = string.Join(",", dishIngredientBases);
                if (currentIngredientBasesStr == dishIngredientBasesStr)
                {
                    currentDishInfo = dishInfo;
                    break;
                }
            }
        }

        isCooking = true;
        cookTime = currentDishInfo.time;
        progressBar.gameObject.SetActive(true);
    }

    void FinishCook()
    {
        
        CreateDish(currentDishInfo);
    }
    
    public void CreateDish(DishInfo info)
    {
        var dish = Instantiate(Resources.Load<GameObject>("Dish/Dish"), kichenToolTransforms[0]);
        dish.GetComponent<Dish>().Init(info);
    }

    private float cookTime = 0;
    private float cookTimer = 0;
    private void Update()
    {
        if (isCooking)
        {
            cookTimer += Time.deltaTime;
            if (cookTimer >= cookTime)
            {
                FinishCook();
                isCooking = false;
                cookTimer = 0;
                progressBar.gameObject.SetActive(false);
            }
            else
            {
                progressBar.SetProgress(cookTimer , cookTime);
            }
        }
    }
}
