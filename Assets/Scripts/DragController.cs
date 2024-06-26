using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragController : MonoBehaviour
{
    public Draggable draggingIngredient;
    public Transform draggingTrans;

    public LayerMask test;

    private Transform originParentTransform;
    private Vector3 originPosition;

    private GameObject mouseOverObj;

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        var layerMask = LayerMask.NameToLayer("Draggable");
        //RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, test);
        
        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePosition, Vector2.zero);

        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                // If true, the mouse is over a UI element, so ignore other input
                return;
            }
            if (draggingIngredient == null)
            {
                // Get the mouse position in world coordinates

                // Cast a ray from the mouse position
                //only raycast certain layer
                //var layerMask = LayerMask.NameToLayer("Draggable");
                RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, test);
                //RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
                //RaycastHit2D hit = hits.Length>0?hits[0]:default(RaycastHit2D);
                if (hit)
                {
                    var ingredient = hit.transform.GetComponent<Ingredient>();
                    if (ingredient != null)
                    {
                        if (ingredient.isInPot)
                        {
                            originParentTransform = ingredient.transform.parent;
                            originPosition = ingredient.transform.position;
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
                                var draggingingredient = Instantiate(Resources.Load<GameObject>("Dish/ingredient"),
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
                            
                            originParentTransform = dish.transform.parent;
                            originPosition = dish.transform.position;
                            draggingIngredient = dish;
                            dish.transform.parent = null;
                            //var kichenTool = ingredient.GetComponentInParent<KichenTool>();
                            //kichenTool.RemoveIngredient(draggingIngredient);
                        }
                    }
                }
            }
        }

        GameObject thisMouseOverObj = null;
        if (Input.GetMouseButton(0))
        {
            if (draggingIngredient != null)
            {
                draggingIngredient.transform.position = mousePosition;
            }
        }
        else
        {
            
            if (hits.Length > 0)
            {
                foreach (var hit in hits)
                {
                    var dish = hit.transform.GetComponent<Dish>();
                    if (dish != null)
                    {
                        dish.OnMouseEnter();
                        thisMouseOverObj = dish.gameObject;
                        break;
                    }
                }
            }

        }

        if (mouseOverObj != thisMouseOverObj)
        {
            if (mouseOverObj)
            {
                mouseOverObj.GetComponent<Dish>().OnMouseExit();
            }
            mouseOverObj = thisMouseOverObj;
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (draggingIngredient != null)
            {
                if (hits.Length > 0)
                {
                    bool used = false;
                    foreach (var hit in hits)
                    {
                        //扔在厨具上，检查是否需要切和是否是刀
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
                            //扔在顾客上，检查是否是菜以及是否是已经order了的顾客
                            var customer = hit.transform.GetComponent<Customer>();
                            if (customer && draggingIngredient is Dish dish && dish.Info.isFinalDish && customer.hasOrdered && !customer.hasServed)
                            {
                                customer.EatDish(dish);
                                used = true;
                                draggingIngredient = null;
                                return;
                            }
                            else
                            {
                                if (customer)
                                {
                                    if (draggingIngredient is Dish dish2 && dish2.Info.isFinalDish)
                                    {
                                        if (customer.hasServed)
                                        {
                                            
                                            PopupManager.Instance.Show("They have eaten!");
                                        }
                                        else
                                        {
                                            
                                            PopupManager.Instance.Show("They haven't ordered!");
                                        }
                                    }
                                    else
                                    {
                                        PopupManager.Instance.Show("It's not cooked!");
                                    }
                                }
                            }
                        }
                    }
                }

                if (draggingIngredient is Ingredient ingredient)
                {
                    IngredientManager.Instance.AddIngredient(ingredient.Info.id);
                    Destroy(draggingIngredient.gameObject);
                    draggingIngredient = null;
                }
                else
                {
                    //todo: move it back
                    draggingIngredient.transform.parent = originParentTransform;
                    draggingIngredient.transform.position = originPosition;
                    //Destroy(draggingIngredient.gameObject);
                    draggingIngredient = null;
                }
            }
        }
    }
}