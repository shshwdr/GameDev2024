using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : MonoBehaviour
{
    IngredientInfo info;
    public IngredientInfo Info => info;
    public SpriteRenderer spriteRenderer;
    public void Init(IngredientInfo info)
    {
        this.info = info;
        spriteRenderer.sprite = Resources.Load<Sprite>("Ingredient/" + info.name);
    }
}
