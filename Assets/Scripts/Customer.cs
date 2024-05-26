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
    private float moveSpeed = 0.6f;

   // public ProgressBar progressBar;

    public void EatDish(Dish dish)
    {
        Destroy(dish.gameObject);
        CustomerLeaveAndFight();
    }

    public void CustomerLeaveAndFight()
    {
        isFighting = true;
        energy = 15f;

    }

    private void Update()
    {
        if (isLeaving)
        {
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
            energy -= Time.deltaTime;
            
            if (energy <= 0)
            {
                isLeaving = true;

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
