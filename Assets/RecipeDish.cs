using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeDish : MonoBehaviour
{
    public Image cover;
    public Button button;
    private string id;
    public Image icon;
    public RecipeMenu menu;
    private void Start()
    {
        
       
    }

    public void OnHoverover()
    {
        if (RecipeManager.Instance.hasUnlockedRecipe(id))
        {
            
            menu.showRecipe(id);
        }
        
    }

    public void OnHoverout()
    {
        menu.hideRecipe();
    }

    // Start is called before the first frame update
    public void Init(DishInfo info,bool unlocked)
    {
        id = info.id;
        cover.sprite = Resources.Load<Sprite>("Dish/" + info.id);;
        icon.sprite = Resources.Load<Sprite>("Dish/" + info.id);;
        cover.gameObject.SetActive(!unlocked);
    }
}
