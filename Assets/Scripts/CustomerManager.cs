using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class CustomerManager : Singleton<CustomerManager>
{
    public Transform customerSpawnParent;
    [HideInInspector]
    public List<Transform> customerSpawnTransforms = new List<Transform>();
    
    public Transform customerTrans;
    public Transform customerStartTrans;

    public float spawnTime = 5;
    public float spawnTimer = 0;
    
    public void Init()
    {
        foreach (Transform trans in customerSpawnParent)
        {
            customerSpawnTransforms.Add(trans);
            
        }
        
        
    }
    
    public void SpawnCustomer(CustomerInfo customerInfo)
    {
        var trans = getFirstTransform();
        if (!trans)
        {
            return;
        }
        var customer = Instantiate(Resources.Load<GameObject>("Customer/" + customerInfo.name), getFirstTransform().position,quaternion.identity,customerTrans);
        customer.transform.position = trans.position;
        customerCount++;
        customer.GetComponent<Customer>().Init(customerInfo);
    }

    private int customerCount = 0;
    Transform getFirstTransform()
    {
        if (customerCount >= customerSpawnTransforms.Count)
        {
            return null;
        }
        return customerSpawnTransforms[customerCount];
    }

    public void SpawnRandomCustomer()
    {
        var customerInfos = CSVLoader.Instance.CustomerInfoDict.Values.ToList();
        var customerInfo = customerInfos.RandomItem();
        SpawnCustomer((customerInfo));
    }

    private void Update()
    {
        spawnTimer+= Time.deltaTime;
        if (spawnTimer > spawnTime)
        {
            spawnTimer -= spawnTime;
            SpawnRandomCustomer();
        }
    }
}
