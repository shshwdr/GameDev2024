using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeMenu : MonoBehaviour
{
    public Transform parent;
    public GameObject menu;

    public void Hide()
    {
        menu.SetActive(false);
    }
    public void Show()
    {
        menu.SetActive(true);
        int i = 0;
        var recipeRows = parent.GetComponentsInChildren<RecipeRow>(true);
        for (int j = 0; j < RecipeManager.Instance.recipes.Count; j++)
        {
            if (RecipeManager.Instance.isUnlocked[j])
            {
                recipeRows[i].gameObject.SetActive(true);
                recipeRows[i].Init(RecipeManager.Instance.recipes[j]);
                i++;
            }
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
