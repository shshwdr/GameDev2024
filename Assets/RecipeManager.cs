using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeManager : Singleton<RecipeManager>
{
    public List<Recipe> recipes = new List<Recipe>();
    public List<bool> isUnlocked = new List<bool>();
    public void Init()
    {
        foreach (var dishInfo in CSVLoader.Instance.DishInfoDict.Values)
        {
            var recipe = new Recipe(){dishName = dishInfo.id,kichenUtil = dishInfo.kichenUtil,ingredients = dishInfo.ingredients};
            recipes.Add((recipe));
            isUnlocked.Add(false);
        }    
    }

    public void AddRecipe(DishInfo info)
    {
        var newRecipe = new Recipe(){dishName = info.id,kichenUtil = info.kichenUtil,ingredients = info.ingredients};
        RecipePopup.Instance.Show(newRecipe);
        for(int i = 0;i<isUnlocked.Count;i++)
        {
            var l = isUnlocked[i];
            var recipe = recipes[i];
            if (recipe.dishName == info.id)
            {
               // PopupManager.Instance.Show("New Recipe Unlocked!");
                isUnlocked[i] = true;
                return;
            }
            if (!l)
            {
            }
        }
    }
    public void unlockRecipe()
    {
        
        for(int i = 0;i<isUnlocked.Count;i++)
        {
            var l = isUnlocked[i];
            if (!l)
            {
                isUnlocked[i] = true;
            }
        }
    }
    public bool hasUnlockedRecipe()
    {
        foreach (var l in isUnlocked)
        {
            if (!l)
            {
                return true;
            }
        }

        return false;
    }

    public Recipe GetUnlockRecipe()
    {
        for(int i = 0;i<isUnlocked.Count;i++)
        {
            var l = isUnlocked[i];
            if (!l)
            {
                return recipes[i];
            }
        }

        return recipes[0];
    }
}
