using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KichenTool : MonoBehaviour
{ 
    KichenToolInfo info;
    public KichenToolInfo Info => info;
    
    
    public Transform kichenToolParent;
    List<Transform> kichenToolTransforms = new List<Transform>();
    
    
    
    public void Init(KichenToolInfo info )
    {
         this.info = info;
         
         
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

        TryUse();
    }
    public bool TryUse()
    {
        bool hasSlot = false;
        foreach (var trans in kichenToolTransforms)
        {
            if (trans.childCount == 0)
            {
                hasSlot = true;
                return false;
            }
        }

        
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
                    CreateDish(dishInfo);
                }
            }
            
            
        }

        return true;
    }

    public void CreateDish(DishInfo info)
    {
        var dish = Instantiate(Resources.Load<GameObject>("Dish/Dish"), kichenToolTransforms[0]);
        dish.GetComponent<Dish>().Init(info);
    }
}
