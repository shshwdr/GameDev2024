using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyManager : Singleton<EnemyManager>
{
    public Transform enemySpawnParent;
    [HideInInspector] public List<Transform> enemySpawnTransforms = new List<Transform>();

    public Transform enemyTrans;

     float spawnTime = -1;
     float spawnTimer = 0;

    public void Init()
    {
        foreach (Transform trans in enemySpawnParent)
        {
            enemySpawnTransforms.Add(trans);
        }
        
    }

    public Enemy SpawnEnemy(EnemyInfo enemyInfo)
    {
        var enemy = Instantiate(Resources.Load<GameObject>("Enemy/" + enemyInfo.name),
            enemySpawnTransforms.RandomItem().position, quaternion.identity, enemyTrans);
        enemy.GetComponent<Enemy>().Init(enemyInfo);
        enemies.Add(enemy.GetComponent<Enemy>());
        return enemy.GetComponent<Enemy>();
    }

    public void SpawnRandomEnemy()
    {
        var enemyInfos = CSVLoader.Instance.EnemyInfoDict.Values.ToList();
        var enemyInfo = enemyInfos.RandomItem();
        SpawnEnemy((enemyInfo));
        spawnCount++;
        var info = RoundManager.Instance.info;
        spawnTime = Random.Range(info.minInterval, info.maxInterval);
    }

    private int spawnCount = 0;

    private void Update()
    {
        if (TutorialManager.Instance.isIntutorial)
        {
            return;
        }
        
        if (RoundManager.Instance.isInBattle && !CustomerManager.Instance.FinishedSpawn)
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer > spawnTime)
            {
                spawnTimer -= spawnTime;
                SpawnRandomEnemy();
            }
        }
    }

    public void StartBattle()
    {
        var info = RoundManager.Instance.info;
        spawnTime = RoundManager.Instance.info.preTime + Random.Range(info.minInterval, info.maxInterval);
    }

    public List<Enemy> enemies = new List<Enemy>();
    public void removeEnemy(Enemy enemy)
    {
        enemies.Remove((enemy));
        if (enemies.Count == 0 && TutorialManager.Instance.isIntutorial && TutorialManager.Instance.stage == TutorialStage.fight)
        {
            
            if (TutorialManager.Instance.isIntutorial)
            {
                TutorialManager.Instance.ShowDialogues();
            }
        } 
        if (enemies.Count == 0 && CustomerManager.Instance.FinishedSpawn)
        {
            RoundManager.Instance.FinishBattle();
        }
        // if (enemies.Count == 0 && spawnCount == RoundManager.Instance.info.enemyCount)
        // {
        //     RoundManager.Instance.FinishBattle();
        // }
    }

    public void clear()
    {
        spawnCount = 0;
        foreach (var enemy in enemySpawnTransforms)
        {
            if (enemy.childCount != 0)
            {
                
                Destroy(enemy.GetChild(0).gameObject);
            }
        }
        enemies.Clear();
        spawnTimer = 0;

    }
}