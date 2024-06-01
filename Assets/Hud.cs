using System.Collections;
using System.Collections.Generic;
using Pool;
using TMPro;
using UnityEngine;

public class Hud : MonoBehaviour
{
    public TMP_Text moneyLabel;
    // Start is called before the first frame update
    void Start()
    {
        EventPool.OptIn("updateMoney", UpdateMoney);
    }

    void UpdateMoney()
    {
        moneyLabel.text = RoundManager.Instance.money.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
