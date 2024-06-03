using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipePopup : Singleton<RecipePopup>
{
    public RecipeRow recipeRow;
    public Button button;
public GameObject recipePopup;

private void Start()
{
    button.onClick.AddListener(() =>
    {
        Hide();
    });
}

public void Show(  Recipe recipe)
    {
        recipeRow.show(recipe.dishName);
        recipePopup.SetActive(true);
    }
    public void Hide()
    {
        recipePopup.SetActive(false);
    }
}
