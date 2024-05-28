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

     private bool isHitting = false;
     private bool isEating = false;
     private bool isBeforeEating = false;

     private Animator animator;
     private SpriteRenderer spriteRenderer;

     public float hitMoveSpeed = 0.7f;
     public float eatMoveSpeed = 0.3f;
     private Vector3 lastAttacker;
     private bool hasEntered = false;
    public void Init(EnemyInfo info)
    {
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = animator.GetComponent<SpriteRenderer>();
         this.info = info;
         target = IngredientManager.Instance.IngredientTransforms().RandomItem().GetComponentInChildren<Ingredient>().eatTransform;
         currentHP = info.hp;
         progressBar.SetProgress(currentHP, info.hp);
         progressBar.gameObject.SetActive(false);
    }

    public void TakeDamage(int damage, Vector3 attackerPosition)
    {
        
        SFXManager.Instance.PlaySFX(SFXType.seagullHit);
        lastAttacker = attackerPosition;
        progressBar.gameObject.SetActive(true);
        currentHP -= damage;
        currentHP = math.max(currentHP, 0);
        progressBar.SetProgress(currentHP, info.hp);
        
        //hit back
        
        animator.SetTrigger("hit");
        

        if (currentHP <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void startHitting()
    {
        
        //被攻击了就不宜动了
        isHitting = true;
        isEating = false;
        isBeforeEating = false;
    }
    public void finishHitting()
    {
        isHitting = false;
    }

    void move()
    {
        
    }
    private void Update()
    {
        if (isHitting)
        {
            var oppositeLastTarget = (transform.position - lastAttacker).normalized + transform.position;
            transform.position = Vector3.MoveTowards(transform.position, oppositeLastTarget, hitMoveSpeed * Time.deltaTime);
            
            Vector2 movementDirection = oppositeLastTarget - transform.position;
            updateDirection(movementDirection);
            
        }

        if (isBeforeEating)
        {
            
            var oppositeLastTarget = Vector3.right + transform.position;
            transform.position = Vector3.MoveTowards(transform.position, oppositeLastTarget, eatMoveSpeed * Time.deltaTime);
            Vector2 movementDirection = oppositeLastTarget - transform.position;
            updateDirection(movementDirection);
        }
        
        if (isHitting || isEating){
            
            return;
        }


        if (!hasEntered && isInBattleView())
        {
            SFXManager.Instance.PlaySFX(SFXType.seagullEnter);
            hasEntered = true;
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
                animator.SetTrigger("eat");
                
                SFXManager.Instance.PlaySFX(SFXType.seagullEat);
                isBeforeEating = true;
                isEating = true;
            }
            transform.position = Vector3.MoveTowards(transform.position, target.position, info.moveSpeed * Time.deltaTime);
            Vector2 movementDirection = target.position - transform.position;
            updateDirection(movementDirection);
        }
    }

    void updateDirection(Vector2 movementDirection )
    {

        if (movementDirection.x > 0)
        {
            // Moving right
            spriteRenderer.flipX = false;
        }
        else if (movementDirection.x < 0)
        {
            // Moving left
            spriteRenderer.flipX = true;
        }
        
    }

    public bool isInBattleView()
    {
        return GameManager.Instance.isInBattleView((transform.position));
    }
    public void Eat()
    {
        isBeforeEating = false;
        var ingredient = target.parent.GetComponentInChildren<Ingredient>();
        if (ingredient)
        {
            if (IngredientManager.Instance.CanConsumeIngredient(ingredient.Info.id))
            {
                
                IngredientManager.Instance.ConsumeIngredient(ingredient.Info.id);
            }
            // var ingredientInfo = ingredient.Info;
            // var ingredientOb = IngredientManager.Instance.CreateIngredient(ingredientInfo, HoldingItemTransform);
            //
            // IngredientManager.Instance.ConsumeIngredient(ingredientInfo.id);
        }
        
        target = EnemyManager.Instance.enemySpawnTransforms.RandomItem();
        isBack = true;
    }

    public void FinishEating()
    {
        isEating = false;
    }
}
