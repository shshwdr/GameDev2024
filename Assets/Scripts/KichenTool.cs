using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KichenTool : MonoBehaviour
{ 
    KichenToolInfo info;
    public KichenToolInfo Info => info;
    
    
    public Transform kichenToolParent;
    List<Transform> kichenToolTransforms = new List<Transform>();
    
    
    
    public void Init(KichenToolInfo info )
    {
         this.info = info;
         
         
         foreach (Transform trans in kichenToolParent)
         {
             kichenToolTransforms.Add(trans);
            
         }
    }

    public void AddIngredient(Ingredient ingredient)
    {
        foreach (var trans in kichenToolTransforms)
        {
            if (trans.childCount == 0)
            {
                ingredient.transform.SetParent((trans));
                ingredient.transform.position =  trans.position;
                return;
            }
        }
    }

    public bool CanAddIngredient(Ingredient ingredient)
    {
        bool hasSlot = false;
        foreach (var trans in kichenToolTransforms)
        {
            if (trans.childCount == 0)
            {
                hasSlot = true;
                return true;
            }
        }

        return false;
    }
}
