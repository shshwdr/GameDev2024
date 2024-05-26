using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    public Transform enemySpawnParent;
    [HideInInspector]
    public List<Transform> enemySpawnTransforms = new List<Transform>();

    public float spawnTime = 3;
    public float spawnTimer = 0;
    public void Init()
    {
        foreach (Transform trans in enemySpawnParent)
        {
            enemySpawnTransforms.Add(trans);
            
        }
        
        
        int i = 0;
        foreach (var info in CSVLoader.Instance.KichenToolInfoDict.Values)
        {
            var enemySpawn = Instantiate(Resources.Load<GameObject>("KichenTool/"+info.name), enemySpawnTransforms[i]);
            enemySpawn.GetComponent<KichenTool>().Init(info);
            i++;
        }
    }

    public void SpawnEnemy(EnemyInfo enemyInfo)
    {
        var enemy = Instantiate(Resources.Load<GameObject>("Enemy/" + enemyInfo.name), enemySpawnTransforms.RandomItem());
         enemy.GetComponent<Enemy>().Init(enemyInfo);
    }

    public void SpawnRandomEnemy()
    {
        var enemyInfos = CSVLoader.Instance.EnemyInfoDict.Values.ToList();
        var enemyInfo = enemyInfos.RandomItem();
        SpawnEnemy((enemyInfo));
    }

    private void Update()
    {
        spawnTimer+= Time.deltaTime;
         if (spawnTimer > spawnTime)
         {
             spawnTimer -= spawnTime;
             SpawnRandomEnemy();
         }
    }
}
