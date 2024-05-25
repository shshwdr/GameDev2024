using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dish : IngredientBase
{
    public SpriteRenderer spriteRenderer;
     DishInfo info;
     public DishInfo Info => info;
     public override string Id=>info.id;
     
    public void Init(DishInfo info)
    {
         this.info = info;
         isInPot = true;
         spriteRenderer.sprite = Resources.Load<Sprite>("Dish/" + info.name);
    }
}
