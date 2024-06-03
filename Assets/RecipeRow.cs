using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeRow : MonoBehaviour
{
    public Image bkImage;
    public GameObject recipeItem;
    public GameObject buff;

        public TMP_Text cost;
    public TMP_Text buffValue;

    public void show(string id)
    {
        bkImage.sprite = Resources.Load<Sprite>("Recipe/" + id);
        gameObject.SetActive(true);
        
        var info = CSVLoader.Instance.DishInfoDict[id];
        cost.text = info.cost.ToString();
        buffValue.text = info.buff.Values.ToList()[0].ToString();
    }

    public void hide()
    {
        gameObject.SetActive(false);
    }
    // public void Init(Recipe recipe)
    // {
    //     // for (int i = 0;i < transform.childCount; i++)
    //     // {
    //     //     Destroy(transform.GetChild(i).gameObject);
    //     // }
    //     // GameObject item;
    //     // for (int i = 0; i < recipe.ingredients.Count; i++)
    //     // {
    //     //      item = Instantiate(recipeItem, transform);
    //     //     item.GetComponent<RecipeItem>().image.sprite = Resources.Load<Sprite>("Dish/" + recipe.ingredients.ElementAt(i).Key);
    //     // }
    //     //
    //     // item = Instantiate(recipeItem, transform);
    //     // item.GetComponent<RecipeItem>().image.sprite = Resources.Load<Sprite>("KichenToolImage/" + recipe.kichenUtil);
    //     //
    //     // item = Instantiate(recipeItem, transform);
    //     // item.GetComponent<RecipeItem>().image.sprite = Resources.Load<Sprite>("Dish/" + CSVLoader.Instance.DishInfoDict[recipe.dishName].image);
    //     //
    //     // item = Instantiate(buff, transform);
    //     // item.GetComponent<TMP_Text>().text = CSVLoader.Instance.DishInfoDict[recipe.dishName].buffString();
    //
    // }
}
