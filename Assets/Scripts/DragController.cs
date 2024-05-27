using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class DragController : MonoBehaviour
{
    public Draggable draggingIngredient;
    public Transform draggingTrans;

    public LayerMask test;

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            if (draggingIngredient == null)
            {
                // Get the mouse position in world coordinates

                // Cast a ray from the mouse position
                //only raycast certain layer
                var layerMask = LayerMask.NameToLayer("Draggable");
                RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, test);
                //RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
                if (hit)
                {
                    var ingredient = hit.transform.GetComponent<Ingredient>();
                    if (ingredient != null)
                    {
                        if (ingredient.isInPot)
                        {
                            
                            draggingIngredient = ingredient;
                            ingredient.transform.parent = null;
                            //var kichenTool = ingredient.GetComponentInParent<KichenTool>();
                            //kichenTool.RemoveIngredient(draggingIngredient);
                        }
                        else
                        {
                            if (IngredientManager.Instance.CanConsumeIngredient(ingredient.Info.id))
                            {
                                IngredientManager.Instance.ConsumeIngredient(ingredient.Info.id);
                                var draggingingredient = Instantiate(Resources.Load<GameObject>("Ingredient/ingredient"),
                                    ingredient.transform.position, quaternion.identity, draggingTrans);
                                draggingingredient.GetComponent<Ingredient>()
                                    .Init(ingredient.GetComponent<Ingredient>().Info);

                                draggingIngredient = draggingingredient.GetComponent<Ingredient>();
                                draggingingredient.GetComponent<Ingredient>().putIntoPot();
                            }
                        }
                    }
                    else
                    {
                        var dish = hit.transform.GetComponent<Dish>();
                        if (dish)
                        {
                            draggingIngredient = dish;
                            dish.transform.parent = null;
                            //var kichenTool = ingredient.GetComponentInParent<KichenTool>();
                            //kichenTool.RemoveIngredient(draggingIngredient);
                        }
                    }
                }
            }
        }

        if (Input.GetMouseButton(0))
        {
            if (draggingIngredient != null)
            {
                draggingIngredient.transform.position = mousePosition;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (draggingIngredient != null)
            {
                RaycastHit2D[] hits = Physics2D.RaycastAll(mousePosition, Vector2.zero);
                if (hits.Length > 0)
                {
                    bool used = false;
                    foreach (var hit in hits)
                    {
                        var kichenTool = hit.transform.GetComponent<KichenTool>();
                        if (kichenTool != null && kichenTool.CanAddIngredient(draggingIngredient as IngredientBase))
                        {
                            kichenTool.AddIngredient(draggingIngredient as IngredientBase);
                            draggingIngredient = null;
                            used = true;
                            return;
                        }
                    }

                    if (!used)
                    {
                        foreach (var hit in hits)
                        {
                            var customer = hit.transform.GetComponent<Customer>();
                            if (customer && draggingIngredient is Dish dish && dish.Info.isFinalDish)
                            {
                                customer.EatDish(dish);
                                draggingIngredient = null;
                                return;
                            }
                        }
                    }
                }

                if (draggingIngredient is Ingredient ingredient)
                {
                    IngredientManager.Instance.AddIngredient(ingredient.Info.id);
                }
                Destroy(draggingIngredient.gameObject);
                draggingIngredient = null;
            }
        }
    }
}