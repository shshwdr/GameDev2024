using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Ingredient : IngredientBase
{
    IngredientInfo info;
    public Transform eatTransform;
    public TMP_Text countLabel;
    public override string Id=>info.id;
    public IngredientInfo Info => info;
    public SpriteRenderer spriteRenderer;
    public void Init(IngredientInfo info)
    {
        this.info = info;
        spriteRenderer.sprite = Resources.Load<Sprite>("Ingredient/" + info.name);
    }

    public void putIntoPot()
    {
        isInPot = true;
        countLabel.transform.parent.gameObject.SetActive(false);
    }
}
