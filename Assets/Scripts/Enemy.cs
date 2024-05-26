using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyInfo info;
    public EnemyInfo Info => info;
     Transform target;
     public Transform HoldingItemTransform;
     public bool isBack = false;
    public void Init(EnemyInfo info)
    {
         this.info = info;
         target = IngredientManager.Instance.IngredientTransforms().RandomItem();
         
    }

    private void Update()
    {
        if (target)
        {
            if (Vector3.Distance(transform.position, target.position) < 0.1f)
            {
                if (isBack)
                {
                    Destroy(gameObject);
                    return;
                }
                
                var ingredient = target.GetComponentInChildren<Ingredient>();
                if (ingredient)
                {
                   var ingredientInfo = ingredient.Info;
                   var ingredientOb = IngredientManager.Instance.CreateIngredient(ingredientInfo, HoldingItemTransform);

                   IngredientManager.Instance.ConsumeIngredient(ingredientInfo.id);
                }
                target = EnemyManager.Instance.enemySpawnTransforms.RandomItem();
                isBack = true;
            }
            transform.position = Vector3.MoveTowards(transform.position, target.position, info.moveSpeed * Time.deltaTime);
        }
    }
}
