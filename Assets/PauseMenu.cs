using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject menu;

    public void Show()
    {
        Time.timeScale = 0;
        menu.SetActive(true);
    }
    public void Hide()
    {
        Time.timeScale = 1;
        menu.SetActive(false);
    }
    public void Restart()
    {
        GameManager.Instance.Restart();
    }

    public void Resume()
    {
        Hide();
        
    }

    public void OpenRecipes()
    {
        Hide();
        GameObject.FindObjectOfType<RecipeMenu>(true).Show();
    }
}
