using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

    public void Init(CustomerInfo info)
    {
        this.info = info;
        animator = GetComponentInChildren<Animator>();
        progressBar.gameObject.SetActive(false);
        //fill requirement
        requirement = CSVLoader.Instance.CustomerRequirementInfos.RandomItem();
        dialogueBubble.hideDialogue();
        animator.SetBool("move",true);
    }

    public void ShowRequirementBubble()
    {
        if (!hasOrdered)
        {
            hasOrdered = true;
        }
        animator.SetTrigger("order");
        dialogueBubble.showDialogue(requirement.description);
    }

    private DishInfo dishInfo;
    private bool satisfyRequirement;
    public void FinishEating()
    {
        
        CustomerLeaveAndFight();
    }
    public void EatDish(Dish dish)
    {
        
        animator.SetBool("move",false);
        animator.SetTrigger("eat");
        progressBar.gameObject.SetActive(true);

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
        Destroy(dish.gameObject);
    }

    public void CustomerLeaveAndFight()
    {
        CustomerManager.Instance.removeCustomer(this);
        isFighting = true;
        initialDuration = info.duration;
        moveSpeed = info.moveSpeed;
        if (satisfyRequirement)
        {
            dialogueBubble.showDialogue("Exactly What I Want!", 4);
        }
        else
        {
            dialogueBubble.showDialogue("Hmm ok...", 4);
        }

        if (dishInfo.buff.ContainsKey("Duration"))
        {
            initialDuration+=dishInfo.buff["Duration"] * (satisfyRequirement?1.5f:1);
        }
        
        if (dishInfo.buff.ContainsKey("MoveSpeed"))
        {
            moveSpeed += moveSpeed * dishInfo.buff["MoveSpeed"]* (satisfyRequirement?1.5f:1) /100f ;
        }

        duration = initialDuration;
    }

    private void Update()
    {
        
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
                if (Vector3.Distance(transform.position, target.position) < 0.1f)
                {
                    Destroy(gameObject);
                }
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
            }

            if (target != null && GameManager.Instance.isInBattleView( target.position))
            {
                animator.SetBool("move",true);
                transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
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
        var enemy = trans.GetComponentInChildren<Enemy>();
        enemy.TakeDamage(info.attack,transform.position);
        attackTimer = info.attackInterval;
        //target = null;
        
        
        SFXManager.Instance.PlaySFX(SFXType.customerHit);
    }
}