using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientManager : Singleton<IngredientManager>
{
    public Transform ingredientParent;
    [HideInInspector]
    public List<Transform> ingredientTransforms = new List<Transform>();

    public Dictionary<string, int> ingredientCount = new Dictionary<string, int>();

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
            ingredientCount[info.id] = info.startCount;
            i++;
        }
    }

    public GameObject CreateIngredient(IngredientInfo info,Transform trans)
    {
        
        var ingredient = Instantiate(Resources.Load<GameObject>("Ingredient/ingredient"), trans);
        ingredient.GetComponent<Ingredient>().Init(info);
        return ingredient;
    }

    public void ConsumeIngredient(string ingredient, int amount = 1)
    {
        ingredientCount[ingredient] -= amount;
    }
    
}
