using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientManager : Singleton<IngredientManager>
{
    public Transform ingredientParent;
    [HideInInspector]
    public List<Transform> ingredientTransforms = new List<Transform>();

    public Dictionary<string, int> ingredientCountDict = new Dictionary<string, int>();

    public bool hasIngredient()
    {
        foreach (var pair in ingredientCountDict)
        {
            if (pair.Value > 0)
            {
                return true;
            }
        }

        return false;
    }
    void updateIngredientCount()
    {
        
        foreach (var item in ingredientTransforms)
        {
            if (item.childCount>0)
            {
                var ingredient = item.GetComponentInChildren<Ingredient>();
                var ingredientCount = ingredientCountDict[ingredient.Info.id];
                ingredient.countLabel.text = ingredientCount.ToString();
            }
        }
    }
    public List<Transform> IngredientTransforms()
    {
        List<Transform> list = new List<Transform>();
        foreach (var item in ingredientTransforms)
        {
            if (item.childCount>0)
            {
                list.Add(item);
            }
        }
        return list;
    }
    public void Init()
    {
        foreach (Transform trans in ingredientParent)
        {
            ingredientTransforms.Add(trans);
            
        }
        
        
        int i = 0;
        foreach (var info in CSVLoader.Instance.IngredientInfoDict.Values)
        {
            CreateIngredient(info,ingredientTransforms[i]);
            ingredientCountDict[info.id] = info.startCount;
            i++;
        }
        updateIngredientCount();
    }

    public GameObject CreateIngredient(IngredientInfo info,Transform trans)
    {
        
        var ingredient = Instantiate(Resources.Load<GameObject>("Dish/ingredient"), trans);
        ingredient.GetComponent<Ingredient>().Init(info);
        return ingredient;
    }

    public void ConsumeIngredient(string ingredient, int amount = 1)
    {
        ingredientCountDict[ingredient] -= amount;
        updateIngredientCount();
        if (!hasIngredient())
        {
            GameoverMenu.Instance.ShowGameoverMenu();
        }
    }

    public bool CanConsumeIngredient(string ingredient, int amount = 1)
    {
        return amount <= ingredientCountDict[ingredient];
    }

    public void AddIngredient(string ingredient, int amount = 1)
    {
       
        ingredientCountDict[ingredient] += amount;
        updateIngredientCount();
    }
    
}
