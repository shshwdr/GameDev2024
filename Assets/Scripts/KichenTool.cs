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

    public void AddIngredient(Ingredient ingredient)
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

    public bool CanAddIngredient(Ingredient ingredient)
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

    public void RemoveIngredient(Ingredient ingredient)
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
                Destroy(trans.gameObject);
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

        RemoveAllIngredient();

        foreach (var dishInfo in CSVLoader.Instance.DishInfoDict.Values)
        {
            var currentIngredients = new List<string>();
                
            foreach (var trans in kichenToolTransforms)
            {
                currentIngredients.Add(trans.GetChild(0).GetComponent<Ingredient>().Info.id);
            }

            currentIngredients.Sort();
            var currentIngredientsStr = string.Join(",", currentIngredients);
            if (dishInfo.kichenUtil == info.id)
            {
                
                var  dishIngredients = dishInfo.ingredients;
                dishIngredients.Sort();
                var dishIngredientsStr = string.Join(",", dishIngredients);
                if (currentIngredientsStr == dishIngredientsStr)
                {
                    var dish = Instantiate(Resources.Load<GameObject>("Dish/Dish"), kichenToolParent);
                    dish.GetComponent<Dish>().Init(dishInfo);
                }
            }
            
            
        }

        return true;
    }
}
