using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyInfo info;
    public EnemyInfo Info => info;
     Transform target;
     public Transform HoldingItemTransform;
     public bool isBack = false;
     private int currentHP;
     public ProgressBar progressBar;

     private float dizzyTime = 0.5f;
     private float dizzyTimer = 0f;
    public void Init(EnemyInfo info)
    {
         this.info = info;
         target = IngredientManager.Instance.IngredientTransforms().RandomItem();
         currentHP = info.hp;
         progressBar.SetProgress(currentHP, info.hp);
    }

    public void TakeDamage(int damage)
    {
        dizzyTimer = dizzyTime;
        currentHP -= damage;
        currentHP = math.max(currentHP, 0);
        progressBar.SetProgress(currentHP, info.hp);
        
        //hit back
        

        if (currentHP <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (dizzyTimer > 0)
        {
            dizzyTimer -= Time.deltaTime;
            return;
        }
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
