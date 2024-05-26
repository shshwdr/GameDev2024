using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    Transform target;
    private float energy;
    private bool isFighting = false;
    private bool isLeaving = false;

    private float attackTimer = 0;

    public DialogueBubble dialogueBubble;

    public ProgressBar progressBar;
    private CustomerInfo info;
    public CustomerInfo Info => info;
    CustomerRequirementInfo requirement;

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
        Destroy(dish.gameObject);
        CustomerLeaveAndFight();
    }

    public void CustomerLeaveAndFight()
    {
        isFighting = true;
        dialogueBubble.showDialogue("For the Food Stand!", 4);
        energy = 15f;
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
                    Vector3.MoveTowards(transform.position, target.position, info.moveSpeed * Time.deltaTime);
                if (Vector3.Distance(transform.position, target.position) < 0.1f)
                {
                    Destroy(gameObject);
                }
            }
        }
        else if (isFighting)
        {
            progressBar.SetProgress(energy, 15);
            energy -= Time.deltaTime;

            if (energy <= 0)
            {
                isLeaving = true;

                dialogueBubble.showDialogue("I'm hungry..", 4);
                target = EnemyManager.Instance.enemySpawnTransforms.RandomItem();
            }

            if (target != null)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, 1f * Time.deltaTime);
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
        enemy.TakeDamage((info.attack));
        attackTimer = info.attackInterval;
        //target = null;
    }
}