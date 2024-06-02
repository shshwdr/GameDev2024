using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class ShopMenu : MonoBehaviour
{
    public GameObject _gameObject;

    public Transform parent;

    public Button nextButton;
    // Start is called before the first frame update
    void Start()
    {
        
        nextButton.onClick.AddListener(() =>
        {
            Hide();
            RoundManager.Instance.FinishShop();
        });
        Hide();
    }

    public void Hide()
    {
        _gameObject.SetActive(false);
    }
    
    public void ShowShop()
    {
        _gameObject.SetActive(true);

        int i = 0;
        var keys = CSVLoader.Instance.IngredientInfoDict.Keys;
        foreach (var shopItem in parent.GetComponentsInChildren<ShopItem>(true))
        {
            if (i == 0)
            {
                //recipe
                shopItem.InitRecipe();
                
            }
            else if(i == 5)
            {
                //cook util
                shopItem.InitUtil();
            }
            else
            {
                shopItem.InitIngredient(keys.ToList().RandomItem());
            }

            i++;
        }
        
    }
}
