using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoundState { battle, result,shop,None}

public class Recipe
{
    public string dishName;
    public string kichenUtil;
    public Dictionary<string, int> ingredients;
}
public class RoundManager : Singleton<RoundManager>
{
    RoundState state = RoundState.battle;
    private int roundCount = 0;
    public EnemyRoundInfo info;
    public int money = 0;
    public int moneyEarnInRound = 0;
    public int chasedEnemyInRound = 0;
    public List<Recipe> recipesInRound = new List<Recipe>();
    public ResultMenu resultMenu;
    public bool isInBattle => state == RoundState.battle;
    public void Init()
    {
        state = RoundState.None;
        StartRound();
    }

    void StartRound()
    {
        info = CSVLoader.Instance.EnemyRoundInfos[roundCount];
        state = RoundState.battle;
        EnemyManager.Instance.StartBattle();
        moneyEarnInRound = 0;
        chasedEnemyInRound = 0;
        recipesInRound.Clear();
        MusicManager.Instance.StartBattle();
    }

    public void GetMoney(int amount)
    {
        money += amount;
        moneyEarnInRound += amount;
    }

    public void CookMeal(Dish dish)
    {
        var recipe = new Recipe(){dishName = dish.Info.id,kichenUtil = dish.Info.kichenUtil,ingredients = dish.ingredients};
    recipesInRound.Add((recipe));
    }

    public void ChasedEnemy()
    {
        chasedEnemyInRound++;
    }
    public void FinishBattle()
    {
        CustomerManager.Instance.clear();
        EnemyManager.Instance.clear();
        state = RoundState.result;
        ShowResult();
        MusicManager.Instance.StartShop();
    }

    void ShowResult()
    {
        resultMenu.ShowResult();
    }

    public void OpenShop()
    {
        StartRound();
    }
}
