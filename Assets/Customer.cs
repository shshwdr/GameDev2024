using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EatDish(Dish dish)
    {
        Destroy(dish.gameObject);
        CustomerLeaveAndFight();
    }

    public void CustomerLeaveAndFight()
    {
        Destroy((gameObject));
    }
}
