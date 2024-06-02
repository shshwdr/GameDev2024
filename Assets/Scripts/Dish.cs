using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Dish : IngredientBase
{
    public SpriteRenderer spriteRenderer;
     DishInfo info;
     public DishInfo Info => info;
     public override string Id=>info.id;
     
     public Dictionary<string,int> ingredients;

     public GameObject ui;
     public TMP_Text nameLabel;

     public Transform buffTrans;
     public TMP_Text buffValue;
    public void Init(DishInfo info,Dictionary<string,int> ingredients)
    {
         this.info = info;
         this.ingredients = ingredients;
         isInPot = true;
         spriteRenderer.sprite = Resources.Load<Sprite>("Dish/" + info.image);
         //stringify a dictionary
         nameLabel.text = info.name;
         if (info.buff.Count > 0)
         {
             nameLabel.text = $"{info.name}\n{info.description}\n";
             var go =  Instantiate(Resources.Load<GameObject>("buff/" + info.buff.Keys.ToList()[0]),buffTrans);
             go.transform.position = buffTrans.position;
             buffValue.text = info.buff.Values.ToList()[0].ToString();
         }
         else
         {
             buffValue.text = "";
         }

         ui.SetActive(false);
    }
    
    static string SerializeDictionary(Dictionary<string, int> dictionary)
    {
        StringBuilder sb = new StringBuilder();

        bool first = true;

        foreach (var kvp in dictionary)
        {
            if (!first)
            {
                sb.Append(",");
            }


            sb.Append(kvp.Key);
            sb.Append(" add ");
            sb.Append(kvp.Value);

            first = false;
        }

        return sb.ToString();
    }

    public void OnMouseEnter()
    {
        ui.SetActive(true);
    }
    
    public void OnMouseExit()
    {
        
        ui.SetActive(false);
    }
    
}
