using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public void Init(IngredientInfo info)
    {
        spriteRenderer.sprite = Resources.Load<Sprite>("Ingredient/" + info.name);
    }
}
