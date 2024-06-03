using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class RecipeMenu : MonoBehaviour
{
    public Transform parent;
    public GameObject menu;
    public RecipeRow recipe;
    public TMP_Text desc;
    public void showRecipe(string id)
    {
        recipe.show(id);
        var info = CSVLoader.Instance.DishInfoDict[id];
        desc.text = info.description;
    }

    public void hideRecipe()
    {
        recipe. hide();
        desc.text = "";
    }
    public void Hide()
    {
        Time.timeScale = 1;
        hideRecipe();
        menu.SetActive(false);
    }
    public void Show()
    {
        Time.timeScale = 0;
        hideRecipe();
        menu.SetActive(true);
        int i = 0;
        var recipeRows =GetComponentsInChildren<RecipeDish>(true);
        for (int j = 0; j < RecipeManager.Instance.recipes.Count; j++)
        {
            recipeRows[i].gameObject.SetActive(true);
            var info = CSVLoader.Instance.DishInfoDict[RecipeManager.Instance.recipes[j].dishName];

            if (!info.isFinalDish)
            {
                continue;
            }
            if (RecipeManager.Instance.isUnlocked[j])
            {
                recipeRows[i].Init(info,true);
            }
            else
            {
                
                recipeRows[i].Init(info,false);
            }
            i++;
        }

        for (; i < recipeRows.Length; i++)
        {
            recipeRows[i].gameObject.SetActive(false);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
