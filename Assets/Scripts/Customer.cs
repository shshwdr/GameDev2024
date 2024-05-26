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
    private float moveSpeed = 0.25f;

    public DialogueBubble dialogueBubble;

    public ProgressBar progressBar;

    private void Start()
    {
        
        progressBar.gameObject.SetActive(false);
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
        dialogueBubble.showDialogue("For the Food Stand!",4);
        energy = 15f;

    }

    private void Update()
    {
        if (isLeaving)
        {
            progressBar.gameObject.SetActive(false);
            if (target != null)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
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

                dialogueBubble.showDialogue("I'm hungry..",4);
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
                for (int i = 0;  i < EnemyManager.Instance.enemyTrans.childCount;i++)
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
        Destroy(enemy.gameObject);
        target = null;
    }

}
