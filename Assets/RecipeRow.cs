using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeRow : MonoBehaviour
{
    public GameObject recipeItem;
    public GameObject buff;

    public void Init(Recipe recipe)
    {
        for (int i = 0;i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
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
        
        item = Instantiate(buff, transform);
        item.GetComponent<TMP_Text>().text = CSVLoader.Instance.DishInfoDict[recipe.dishName].buffString();

    }
}
