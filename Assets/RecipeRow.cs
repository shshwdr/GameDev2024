using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RecipeRow : MonoBehaviour
{
    public GameObject recipeItem;

    public void Init(Recipe recipe)
    {
        GameObject item;
        for (int i = 0; i < recipe.ingredients.Count; i++)
        {
             item = Instantiate(recipeItem, transform);
            item.GetComponent<RecipeItem>().image.sprite = Resources.Load<Sprite>("Dish/" + recipe.ingredients.ElementAt(i).Key);
        }
        
        item = Instantiate(recipeItem, transform);
        item.GetComponent<RecipeItem>().image.sprite = Resources.Load<Sprite>("KichenToolImage/" + recipe.kichenUtil);
        
        item = Instantiate(recipeItem, transform);
        item.GetComponent<RecipeItem>().image.sprite = Resources.Load<Sprite>("Dish/" + CSVLoader.Instance.DishInfoDict[recipe.dishName].image);
    }
}
