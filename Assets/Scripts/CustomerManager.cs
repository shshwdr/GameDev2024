using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class CustomerManager : Singleton<CustomerManager>
{
    public Transform customerSpawnParent;
    [HideInInspector] public List<Transform> customerSpawnTransforms = new List<Transform>();

    public Transform customerTrans;
    public Transform customerStartTrans;

    public float spawnTime = 5;
    public float spawnTimer = 0;

    public void serve()
    {
        serveCount++;
        if (serveCount == RoundManager.Instance.info.customerCount)
        {
            FinishedSpawn = true;
            if (EnemyManager.Instance.enemies.Count == 0)
            {
                RoundManager.Instance.FinishBattle();
            }
        }
    }
    public void Init()
    {
        foreach (Transform trans in customerSpawnParent)
        {
            customerSpawnTransforms.Add(trans);
        }
    }

    private List<Customer> customers = new List<Customer>();

    public void SpawnCustomer(CustomerInfo customerInfo)
    {
        var customer = Instantiate(Resources.Load<GameObject>("Customer/" + customerInfo.name),
            customerStartTrans.position, quaternion.identity, customerTrans);
        customer.transform.position = customerStartTrans.position;
        customer.GetComponent<Customer>().Init(customerInfo);
        customers.Add(customer.GetComponent<Customer>());
        
        
        spawnCount++;

    }


    private int customerCount = 0;

    public void SpawnRandomCustomer()
    {
        var customerInfos = CSVLoader.Instance.CustomerInfoDict.Values.ToList();
        var customerInfo = customerInfos.RandomItem();
        SpawnCustomer((customerInfo));
    }

    public void StartBattle()
    {
        
        var info = RoundManager.Instance.info;
        spawnTime = Random.Range(info.customerMinInterval, info.customerMaxInterval);
    }

    private int spawnCount = 0;
    private int serveCount = 0;
    public bool FinishedSpawn = false;
    private void Update()
    {
        if (RoundManager.Instance.isInBattle && spawnCount < RoundManager.Instance.info.customerCount)
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer > spawnTime && customers.Count < customerSpawnTransforms.Count)
            {
                spawnTimer -= spawnTime;
                SpawnRandomCustomer();
            }
        }

        //move all customer forward
            for (int i = 0; i < customers.Count; i++)
            {
                if ((customers[i].transform.position - customerSpawnTransforms[i].position).magnitude > 0.1f)
                {
                    customers[i].transform.position = Vector3.MoveTowards(customers[i].transform.position,
                        customerSpawnTransforms[i].position, 1f * Time.deltaTime);
                }
                else
                {
                    if (i == 0)
                    {
                        customers[i].ShowRequirementBubble();
                    }
                }
            }
    }

    public void clear()
    {

        foreach (Transform trans in customerTrans)
        {
            Destroy(trans.gameObject);
        }
        customers.Clear();
        spawnTimer = 0;
        
        spawnCount = 0;
        serveCount = 0;
        FinishedSpawn = false;
    }

    public void removeCustomer(Customer cus)
    {
        customers.Remove(cus);
    }
}