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

    public void Init(CustomerInfo info)
    {
        this.info = info;

        progressBar.gameObject.SetActive(false);
        //fill requirement
        requirement = CSVLoader.Instance.CustomerRequirementInfos.RandomItem();
        dialogueBubble.showDialogue(requirement.description);
    }

    public void EatDish(Dish dish)
    {
        progressBar.gameObject.SetActive(true);

        bool satisfyRequirement = false;
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
        
        CustomerLeaveAndFight(dish.Info,satisfyRequirement);
        Destroy(dish.gameObject);
    }

    public void CustomerLeaveAndFight(DishInfo dishInfo, bool satisfy)
    {
        isFighting = true;
        initialDuration = info.duration;
        moveSpeed = info.moveSpeed;
        if (satisfy)
        {
            dialogueBubble.showDialogue("Exactly What I Want!", 4);
        }
        else
        {
            dialogueBubble.showDialogue("Hmm ok...", 4);
        }

        if (dishInfo.buff.ContainsKey("Duration"))
        {
            initialDuration+=dishInfo.buff["Duration"];
        }
        
        if (dishInfo.buff.ContainsKey("MoveSpeed"))
        {
            moveSpeed += moveSpeed * dishInfo.buff["MoveSpeed"]/100f;
        }

        duration = initialDuration;
    }

    private void Update()
    {
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
            return;
        }

        if (isLeaving)
        {
            progressBar.gameObject.SetActive(false);
            if (target != null)
            {
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