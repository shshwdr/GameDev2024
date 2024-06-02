using System;
using System.Collections;
using System.Collections.Generic;
using Pool;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ShopItem : MonoBehaviour
{

    public Image icon;
    public Button purchaseButton;
    public TMP_Text costLabel;
    public TMP_Text countLabel;
    private int cost;

    private void Start()
    {
        EventPool.OptIn("updateMoney",updateItem);
    }

    public void InitRecipe()
    {
        countLabel.gameObject.SetActive(false);
        icon.sprite = Resources.Load<Sprite>("KichenToolImage/" + "Recipe");
        cost = 10;
        if (RecipeManager.Instance.hasUnlockedRecipe())
        {
            gameObject.SetActive(true);
            purchaseButton.onClick.AddListener(() =>
            {
                SFXManager.Instance.PlaySFX((SFXType.purchase));
                RoundManager.Instance.ConsumeMoney(cost);

                RecipePopup.Instance.Show((RecipeManager.Instance.GetUnlockRecipe()));
                RecipeManager.Instance.unlockRecipe();
                gameObject.SetActive(false);
                //InitRecipe();
            });
            updateItem();
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void InitUtil()
    {
        
        countLabel.gameObject.SetActive(false);

        if (KichenToolManager.Instance.toBuyTools.Count > 0)
        {
            var name = KichenToolManager.Instance.toBuyTools[0];
            var info  = CSVLoader.Instance.KichenToolInfoDict[name];
            cost = info.cost;
            icon.sprite = Resources.Load<Sprite>("KichenToolImage/" + name);
            purchaseButton.onClick.RemoveAllListeners();
            purchaseButton.onClick.AddListener(() =>
            {
                SFXManager.Instance.PlaySFX((SFXType.purchase));
                RoundManager.Instance.ConsumeMoney(cost);
                KichenToolManager.Instance.AddUtil();
                
                InitUtil();
            });
            updateItem();
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void InitIngredient(string name)
    {
        icon.sprite = Resources.Load<Sprite>("Dish/" + name);
        int count = Random.Range(2, 5+RoundManager.Instance.hpAdd());
        countLabel.text = count.ToString();
        cost = CSVLoader.Instance.IngredientInfoDict[name].cost * count;
        purchaseButton.onClick.RemoveAllListeners();
        purchaseButton.onClick.AddListener(() =>
        {
            SFXManager.Instance.PlaySFX((SFXType.purchase));
            RoundManager.Instance.ConsumeMoney(cost);
            IngredientManager.Instance.AddIngredient(name,count);
        });
        updateItem();
    }

    public void updateItem()
    {
        
        costLabel.text = cost.ToString();

        if (RoundManager.Instance.money >= cost)
        {
            purchaseButton.interactable = true;
            costLabel.color = Color.white;
            
        }
        else
        {
            purchaseButton.interactable = false;
            costLabel.color = Color.red;
        }

    }
}
