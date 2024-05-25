using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientManager : Singleton<IngredientManager>
{
    public Transform ingredientParent;
    List<Transform> ingredientTransforms = new List<Transform>();
    public void Init()
    {
        foreach (Transform trans in ingredientParent)
        {
            ingredientTransforms.Add(trans);
            
        }
        
        
        int i = 0;
        foreach (var info in CSVLoader.Instance.IngredientInfoDict.Values)
        {
            var ingredient = Instantiate(Resources.Load<GameObject>("Ingredient/ingredient"), ingredientTransforms[i]);
            ingredient.GetComponent<Ingredient>().Init(info);
            i++;
        }
    }
}
