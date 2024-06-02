using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Customer : MonoBehaviour
{
    Transform target;
    private float duration;
    private float initialDuration;
    private bool isFighting = false;
    private bool isLeaving = false;

    private float attackTimer = 0;

    public DialogueBubble dialogueBubble;

    public ProgressBar progressBar;
    private CustomerInfo info;
    public CustomerInfo Info => info;
    CustomerRequirementInfo requirement;
    private float moveSpeed;
    private float criticalRate;
    private float attack;
    private float attackInterval;
    public bool hasOrdered = false;
    private Animator animator;
    private bool isAttacking = false;

    public GameObject money;
    public Transform buff;

    public void Init(CustomerInfo info)
    {
        this.info = info;
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = animator.GetComponent<SpriteRenderer>();
        progressBar.gameObject.SetActive(false);
        //fill requirement
        requirement = CSVLoader.Instance.CustomerRequirementInfos.RandomItem();
        HideDialogue();
        animator.SetBool("move",true);
    }

    public void HideDialogue()
    {
        
        dialogueBubble.hideDialogue();
    }

    public void ShowRequirementBubble()
    {
        if (TutorialManager.Instance.isIntutorial)
        {
            if (TutorialManager.Instance.stage == TutorialStage.start)
            {
                PlayOrder();
                hasOrdered = true;
                TutorialManager.Instance.ShowDialogues();
            }
            return;
        }
        
        if (!hasOrdered)
        {
            hasOrdered = true;
            
            animator.SetBool("move",false);
            animator.SetTrigger("order");
            dialogueBubble.showDialogue(requirement.description);
        }
        
    }

    public void PlayOrder()
    {
        
        animator.SetBool("move",false);
        animator.SetTrigger("order");
    }

    public void ShowDialogue(string dialogue)
    {
        dialogueBubble.showDialogue(dialogue);
    }
    private DishInfo dishInfo;
    private bool satisfyRequirement;
    public void FinishEating()
    {
        
        money.SetActive(false);
        if (TutorialManager.Instance.isIntutorial)
        {
            TutorialManager.Instance.ShowDialogues();
        }
        CustomerLeaveAndFight();
    }

    public bool hasServed = false;
    public void EatDish(Dish dish)
    {
        HideDialogue();
        hasServed = true;
        animator.SetBool("move",false);
        animator.SetTrigger("eat");
        

        satisfyRequirement = false;
        switch (requirement.requirementType)
        {
             case "ingredient":
                if (dish.ingredients.ContainsKey(requirement.subType))
                {
                    satisfyRequirement = true;
                }
                break;
            case "hot":
                if (requirement.subType == "TRUE" && dish.Info.isHot)
                {
                    satisfyRequirement = true;
                }else if (requirement.subType == "FALSE" && !dish.Info.isHot)
                {
                    satisfyRequirement = true;
                }
                break;
        }

        dishInfo = dish.Info;
        money.SetActive(true);
        money.GetComponentInChildren<TMP_Text>(true).text =((int)(dishInfo.cost * (satisfyRequirement ? 1.5f : 1))).ToString();
        money.transform.DOLocalMoveY(20, 1);
        if (satisfyRequirement)
        {
            
            SFXManager.Instance.PlaySFX(SFXType.customerEatHappy);
        }
        else
        {
            
            SFXManager.Instance.PlaySFX(SFXType.customerEat);
        }
        Destroy(dish.gameObject);
    }

    public void CustomerLeaveAndFight()
    {
        RoundManager.Instance.AddMoney((int)(dishInfo.cost*(satisfyRequirement?1.5f:1)));
        
        CustomerManager.Instance.removeCustomer(this);
        isFighting = true;
        initialDuration = info.duration;
        if (TutorialManager.Instance.isIntutorial)
        {
            initialDuration = 1000;
        }
        moveSpeed = info.moveSpeed;
        attack = info.attack;
        attackInterval = info.attackInterval;
        criticalRate = info.criticalRate;
        if (!TutorialManager.Instance.isIntutorial)
        {
            if (satisfyRequirement)
            {
                dialogueBubble.showDialogue("Exactly What I Want!", 4);
            }
            else
            {
                dialogueBubble.showDialogue("Hmm ok...", 4);
            }
        }

        var go =  Instantiate(Resources.Load<GameObject>("buff/" + dishInfo.buff.Keys.ToList()[0]),buff);
        go.transform.position = buff.position;
        
        if (dishInfo.buff.ContainsKey("Duration"))
        {
            initialDuration+=dishInfo.buff["Duration"] * (satisfyRequirement?1.5f:1);
        }
        
        if (dishInfo.buff.ContainsKey("MoveSpeed"))
        {
            moveSpeed += moveSpeed * dishInfo.buff["MoveSpeed"]* (satisfyRequirement?1.5f:1) /100f ;
        }
        if (dishInfo.buff.ContainsKey("CritRate"))
        {
            criticalRate += dishInfo.buff["CritRate"]* (satisfyRequirement?1.5f:1) ;
        }
        if (dishInfo.buff.ContainsKey("AttackPower"))
        {
            attack+=dishInfo.buff["AttackPower"] * (satisfyRequirement?1.5f:1);
        }

        duration = initialDuration;
        
        CustomerManager.Instance.serve();
        
        progressBar.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (GameManager.Instance.isGameOver)
        {
            return;
        }
        if (attackTimer > 0)
        {
            animator.SetBool("move",false);
            attackTimer -= Time.deltaTime;
            return;
        }

        if (isLeaving)
        {
            progressBar.gameObject.SetActive(false);
            if (target != null)
            {
                
                animator.SetBool("move",true);
                transform.position =
                    Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
                
                Vector2 movementDirection = target.position - transform.position;
                updateDirection(movementDirection);
                if (Vector3.Distance(transform.position, target.position) < 0.1f)
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                
                target = EnemyManager.Instance.enemySpawnTransforms.RandomItem();
            }
        }
        else if (isFighting)
        {
            progressBar.SetProgress(duration, initialDuration);
            duration -= Time.deltaTime;

            if (duration <= 0)
            {
                isLeaving = true;

                dialogueBubble.showDialogue("I'm hungry..", 4);
                target = EnemyManager.Instance.enemySpawnTransforms.RandomItem();
                return;
            }

            if (target != null &&target.GetComponentInChildren<Enemy>()!=null && !target.GetComponentInChildren<Enemy>().isDead &&  GameManager.Instance.isInBattleView( target.position))
            {
                animator.SetBool("move",true);
                transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
                
                Vector2 movementDirection = target.position - transform.position;
                updateDirection(movementDirection);
                if (Vector3.Distance(transform.position, target.position) < 0.1f)
                {
                    Attack(target);
                }
            }
            else
            {
                Transform closestedTrans = null;
                float closestDistance = float.MaxValue;
                for (int i = 0; i < EnemyManager.Instance.enemyTrans.childCount; i++)
                {
                    var enemyTrans = EnemyManager.Instance.enemyTrans.GetChild(i);

                    if (!GameManager.Instance.isInBattleView(enemyTrans.position))
                    {
                        continue;
                    }
                    var distance = Vector3.Distance(enemyTrans.position, transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestedTrans = enemyTrans;
                    }
                }

                target = closestedTrans;
            }
        }
    }
    
    

    void Attack(Transform trans)
    {

        var isCritical = Random.Range(0, 100) < criticalRate;

        if (isCritical)
        {
            animator.SetTrigger("kick");
        }
        else
        {
            animator.SetTrigger("attack");
        }
        
        //target = null;
        StartCoroutine((TrueAttack(trans, isCritical)));
        
        attackTimer = attackInterval;
    }

    private float animTime = 0.2f;
    IEnumerator TrueAttack(Transform trans,bool isCritical)
    {
        
        if (isCritical)
        {
            SFXManager.Instance.PlaySFX(SFXType.customerKick);
        }
        else
        {
            SFXManager.Instance.PlaySFX(SFXType.customerHit);
        }
        yield return  new WaitForSeconds(animTime);
        var enemy = trans.GetComponentInChildren<Enemy>();
        if (enemy)
        {
            enemy.TakeDamage((int)attack,transform.position,isCritical);
        }
    }
    
    
    private SpriteRenderer spriteRenderer;
    void updateDirection(Vector2 movementDirection )
    {

        if (movementDirection.x > 0)
        {
            // Moving right
            spriteRenderer.flipX = true;
        }
        else if (movementDirection.x < 0)
        {
            // Moving left
            spriteRenderer.flipX = false;
        }
        
    }
}