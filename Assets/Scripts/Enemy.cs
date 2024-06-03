using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
     private int maxHP;
    public void Init(EnemyInfo info)
    {
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = animator.GetComponent<SpriteRenderer>();
         this.info = info;
         target = IngredientManager.Instance.IngredientTransforms().RandomItem().GetComponentInChildren<Ingredient>().eatTransform;
         currentHP = info.hp+RoundManager.Instance.hpAdd();
         maxHP = currentHP;
         progressBar.SetProgress(currentHP, maxHP);
         progressBar.gameObject.SetActive(false);
         moveSpeed = info.moveSpeed;
    }

    public void TakeDamage(int damage, Vector3 attackerPosition,bool isCritical)
    {
        if (isDead)
        {
            return;
        }
        SFXManager.Instance.PlaySFX(SFXType.seagullHit);
        this.isCritical = isCritical;
        if (!isCritical)
        {
            lastAttacker = attackerPosition;
            progressBar.gameObject.SetActive(true);
            currentHP -= damage;
            currentHP = math.max(currentHP, 0);
        
            //hit back
        
            animator.SetTrigger("hit");
        }
        else
        {
            progressBar.gameObject.SetActive(false);
            currentHP = 0;
            animator.SetTrigger("bigHit");
            spriteRenderer.sortingLayerName = "UI";
            spriteRenderer.sortingOrder = 1000;
        }

        progressBar.SetProgress(currentHP, maxHP);
        if (currentHP <= 0)
        {
            RoundManager.Instance.ChasedEnemy();

            if (eatenInfo != null)
            {
                var draggingingredient = Instantiate(Resources.Load<GameObject>("Dish/ingredient"),
                    transform.position, quaternion.identity,GameManager.Instance.renderTrans);
                draggingingredient.GetComponent<Ingredient>()
                    .Init(eatenInfo);
                draggingingredient.GetComponent<Ingredient>().putIntoPot();
            }
            
            EnemyDestroy();
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
        if (GameManager.Instance.isGameOver)
        {
            return;
        }
        if (isDead)
        {
            return;
        }
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
                    EnemyDestroy();
                    return;
                }
                animator.SetTrigger("eat");
                
                isBeforeEating = true;
                isEating = true;
            }
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
            Vector2 movementDirection = target.position - transform.position;
            updateDirection(movementDirection);
        }
    }

    public float moveSpeed;

    private bool isCritical = false;
    public void EnemyDestroy()
    {
        EnemyManager.Instance.removeEnemy(this);
        isDead = true;
        StartCoroutine(DestoryInternal());
        Invoke("DestoryInternal", 1);
    }

    public bool isDead = false;
    IEnumerator DestoryInternal()
    {
        yield return  new WaitForSeconds(0.3f);
        if (isCritical)
        {
            SFXManager.Instance.PlaySFX(SFXType.splatBig);
        }

        yield return  new WaitForSeconds(0.7f);
        if (isCritical)
        {
            
            transform.DOMoveY(transform.position.y - 5, 0.7f);
            yield return  new WaitForSeconds(0.7f);
        }
        Destroy(gameObject);
    }

    void updateDirection(Vector2 movementDirection )
    {
        if (isDead)
        {
            return;
        }
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

    private IngredientInfo eatenInfo;
    public void Eat()
    {
        if (isDead)
        {
            return;
        }
        SFXManager.Instance.PlaySFX(SFXType.seagullEat);
        isBeforeEating = false;
        var ingredient = target.parent.GetComponentInChildren<Ingredient>();
        if (ingredient)
        {
            if (IngredientManager.Instance.CanConsumeIngredient(ingredient.Info.id))
            {
                
                IngredientManager.Instance.ConsumeIngredient(ingredient.Info.id);

                eatenInfo = ingredient.Info;
            }
            // var ingredientInfo = ingredient.Info;
            // var ingredientOb = IngredientManager.Instance.CreateIngredient(ingredientInfo, HoldingItemTransform);
            //
            // IngredientManager.Instance.ConsumeIngredient(ingredientInfo.id);
        }

        if (!IngredientManager.Instance.hasIngredient())
        {
            GameoverMenu.Instance.ShowGameoverMenu();
        }
        target = EnemyManager.Instance.enemySpawnTransforms.RandomItem();
        isBack = true;
    }

    public void FinishEating()
    {
        isEating = false;
    }
}
