using System.Collections;
using System.Collections.Generic;
using Pool;
using Unity.Mathematics;
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

    public int hpAdd()
    {
        return info!=null?info.hpAdd:0;
    }

    public void ConsumeMoney(int amount)
    {
        money -= amount;
        
        EventPool.Trigger("updateMoney");
    }
    public void StartRound()
    {
        clear();
        roundCount = math.min(roundCount, CSVLoader.Instance.EnemyRoundInfos.Count - 1);
        info = CSVLoader.Instance.EnemyRoundInfos[ roundCount];
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

    public void CookMeal(DishInfo dish) 
    {
        var recipe = new Recipe(){dishName = dish.id};
        recipesInRound.Add((recipe));
    }

    public void ChasedEnemy()
    {
        chasedEnemyInRound++;
    }

    public void clear()
    {
        
        CustomerManager.Instance.clear();
        EnemyManager.Instance.clear();
    }
    public void FinishBattle()
    {
        clear();
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
        roundCount++;
        StartRound();
    }
}
