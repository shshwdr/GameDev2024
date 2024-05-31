using System.Collections;
using System.Collections.Generic;
using Pool;
using UnityEngine;

public enum RoundState { battle, result,shop,None}

public class Recipe
{
    public string dishName;
    public string kichenUtil;
    public Dictionary<string, int> ingredients;

    public string ToString()
    {
        var str = "";
        List<string> ing = new List<string>();
        foreach (var pair in ingredients)
        {
            for (int i = 0; i < pair.Value; i++)
            {
                ing.Add((pair.Key));
            }
        }
        ing.Sort();
        for (int i = 0; i < ing.Count; i++)
        {
            str += ing[i];
            if (i != ing.Count - 1)
            {
                str += ",";
            }
        }

        str += kichenUtil;
        return str;
    }
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
    public ShopMenu shopMenu;
    public bool isInBattle => state == RoundState.battle;
    public void Init()
    {
        state = RoundState.None;
        StartRound();
    }

    public void ConsumeMoney(int amount)
    {
        money -= amount;
        
        EventPool.Trigger("updateMoney");
    }
    void StartRound()
    {
        info = CSVLoader.Instance.EnemyRoundInfos[roundCount];
        state = RoundState.battle;
        EnemyManager.Instance.StartBattle();
        CustomerManager.Instance.StartBattle();
        moneyEarnInRound = 0;
        chasedEnemyInRound = 0;
        recipesInRound.Clear();
        MusicManager.Instance.StartBattle();
    }

    public void AddMoney(int amount)
    {
        
        money += amount;
        moneyEarnInRound += amount;
        EventPool.Trigger("updateMoney");
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
        shopMenu.ShowShop();
    }

    public void FinishShop()
    {
        
        StartRound();
    }
}
