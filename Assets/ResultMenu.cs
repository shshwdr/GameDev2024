using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultMenu : MonoBehaviour
{
    public GameObject _gameObject;
    public TMP_Text chsedLabel;
    public TMP_Text earnLabel;
    public Transform recipeLabel;
    public GameObject recipeCell;

    public Button nextButton;

    private void Start()
    {
        nextButton.onClick.AddListener(() =>
        {
            Hide();
            RoundManager.Instance.OpenShop();
        });
        Hide();
    }

    public void Hide()
    {
        _gameObject.SetActive(false);
    }
    public void ShowResult()
    {
        _gameObject.SetActive(true);
        chsedLabel.text = $"You chased {RoundManager.Instance.chasedEnemyInRound} enemies";
        earnLabel.text = $"You earned {RoundManager.Instance.moneyEarnInRound} money";
        foreach (var recipe in RoundManager.Instance.recipesInRound)
        {
            var recipeGO = Instantiate(recipeCell, recipeLabel);
            recipeGO.GetComponent<RecipeRow>().Init((recipe));
        }
        
    }
}
