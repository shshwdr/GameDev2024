using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Dish : IngredientBase
{
    public SpriteRenderer spriteRenderer;
     DishInfo info;
     public DishInfo Info => info;
     public override string Id=>info.id;
     
     public Dictionary<string,int> ingredients;

     public GameObject ui;
     public TMP_Text nameLabel;
     
    public void Init(DishInfo info,Dictionary<string,int> ingredients)
    {
         this.info = info;
         this.ingredients = ingredients;
         isInPot = true;
         spriteRenderer.sprite = Resources.Load<Sprite>("Dish/" + info.image);
         nameLabel.text = info.name;
    }

    public void OnMouseEnter()
    {
        ui.SetActive(true);
    }
    
    public void OnMouseExit()
    {
        
        ui.SetActive(false);
    }
    
}
