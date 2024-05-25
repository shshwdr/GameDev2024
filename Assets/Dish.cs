using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dish : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
     DishInfo info;
     public DishInfo Info => info;
    public void Init(DishInfo info)
    {
         this.info = info;
         spriteRenderer.sprite = Resources.Load<Sprite>("Dish/" + info.name);
    }
}
