using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KichenTool : MonoBehaviour
{ 
    KichenToolInfo info;
    public KichenToolInfo Info => info;
    private Dictionary<string,int> currentIngredientBases = new Dictionary<string,int>();
    
    public Transform kichenToolParent;
    List<Transform> kichenToolTransforms = new List<Transform>();

    private DishInfo currentDishInfo;
    private bool isCooking = false;
    
    public AudioSource audioSource;

    public ProgressBar progressBar;
    
    public void Init(KichenToolInfo info )
    {
         this.info = info;
         progressBar.gameObject.SetActive(false);
         
         foreach (Transform trans in kichenToolParent)
         {
             if (trans.gameObject.activeSelf)
             {
                 
                 kichenToolTransforms.Add(trans);
             }
            
         }
    }

    public void AddIngredient(IngredientBase ingredient)
    {
        foreach (var trans in kichenToolTransforms)
        {
            if (trans.childCount == 0)
            {
                SFXManager.Instance.PlaySFX(SFXType.placeItem);
                ingredient.transform.SetParent((trans));
                ingredient.transform.position =  trans.position;
                return;
            }
        }
    }

    public bool CanAddIngredient(IngredientBase ingredient)
    {
        if (isCooking)
        {
            PopupManager.Instance.Show("It's cooking!");
            return false;
        }
        bool hasSlot = false;
        foreach (var trans in kichenToolTransforms)
        {
            if (trans.childCount == 0)
            {
                hasSlot = true;
            }
        }

        if (!hasSlot)
        {
            PopupManager.Instance.Show("Max 3 items!");
            return false;
        }

        if (info.name == "Knife")
        {
            if (ingredient is Ingredient ing && ing.Info.needCut)
            {
                return true;
            }
            else
            {
                PopupManager.Instance.Show("Can't cut this!");
                return false;
            }
        }
        else
        {
            if (ingredient is Ingredient ing && ing.Info.needCut)
            {
                
                PopupManager.Instance.Show("Cut it first!");
                return false;
            }
            else
            {
                return true;
            }
        }


        return true;
    }

    public void RemoveIngredient(IngredientBase ingredient)
    {
        foreach (var trans in kichenToolTransforms)
        {
            if (trans.childCount != 0)
            {
                if (trans.GetChild(0) == ingredient)
                {
                    trans.parent = null;
                    
                    return;
                }
            }
        }

        if (true)
        {
            Debug.LogError(" remove ingredient failed");
        }
    }

    
    public void RemoveAllIngredient()
    {
        foreach (var trans in kichenToolTransforms)
        {
            if (trans.childCount != 0)
            {
                Destroy(trans.GetChild(0).gameObject);
            }
        }
    }

    public void Use()
    {
        
        bool hasSlot = false;
        foreach (var trans in kichenToolTransforms)
        {
            if (trans.childCount != 0)
            {
                hasSlot = true;
                TryUse();
            }
        }

    } 
    void TryUse()
    {
        currentIngredientBases = new Dictionary<string, int>();
                
        foreach (var trans in kichenToolTransforms)
        {
            if (trans.childCount > 0)
            {
                var id = trans.GetChild(0).GetComponent<IngredientBase>().Id;
                if (currentIngredientBases.ContainsKey(id))
                {
                    currentIngredientBases[id]++;
                }
                else
                {
                    currentIngredientBases.Add(id, 1);
                }
            }
        }

        if (info.id == "Knife")
        {
            var ingredientInfo = CSVLoader.Instance.IngredientInfoDict[ currentIngredientBases.Keys.ToList()[0]];
            if (ingredientInfo.isMeat)
            {
                audioSource.clip = cookSFX.RandomItem();
                //SFXManager.Instance.PlaySFX(SFXType.cutMeat);
            }
            else
            {
                audioSource.clip = cookSFX2.RandomItem();
                //SFXManager.Instance.PlaySFX(SFXType.cutVeg);
            }
            
            
        }
        else
        {
            audioSource.clip = cookSFX.RandomItem();
            //SFXManager.Instance.PlaySFX(SFXType.cook);
        }
        

        RemoveAllIngredient();

        foreach (var dishInfo in CSVLoader.Instance.DishInfoDict.Values)
        {
            if (dishInfo.kichenUtil == info.id)
            {
                
                var  dishIngredientBases = dishInfo.ingredients;

                if (info.id == "Knife")
                {
                    if (dishIngredientBases.Count == 1 &&
                        dishIngredientBases.Keys.ToList()[0] == currentIngredientBases.Keys.ToList()[0])
                    {
                        currentDishInfo = dishInfo;
                        break;
                    }
                }
                else
                {
                    if (dishIngredientBases.Count == 1 &&
                        dishIngredientBases.Keys.ToList()[0] == "Anything")
                    {
                        RecipeManager.Instance.AddRecipe(dishInfo);
                        currentDishInfo = dishInfo;
                        break;
                    }

                    var checkIngredients = new Dictionary<string, int>(dishIngredientBases);
                    
                    bool isMatch = true;
                    bool fullMatch = currentIngredientBases.Count == dishIngredientBases.Count;
                    foreach (var ingredient in dishIngredientBases)
                    {
                        if (currentIngredientBases.ContainsKey(ingredient.Key) &&
                            currentIngredientBases[ingredient.Key] >= ingredient.Value)
                        {
                            if (currentIngredientBases[ingredient.Key] != ingredient.Value)
                            {
                                fullMatch = false;
                            }
                        }
                        else
                        {
                            isMatch = false;
                            break;
                        }
                    }

                    if (isMatch)
                    {
                        if (fullMatch || dishInfo.ingredients.Keys.Contains("Anything"))
                        {
                            RecipeManager.Instance.AddRecipe(dishInfo);
                        }
                        currentDishInfo = dishInfo;
                        break;
                    }
                }
            }
        }

        audioSource.Play();
        isCooking = true;
        cookTime = currentDishInfo.time;
        progressBar.gameObject.SetActive(true);
        
    }

    void FinishCook()
    {
        
        CreateDish(currentDishInfo);
        
        audioSource.Stop();

        if (currentDishInfo.isFinalDish)
        {
            if (TutorialManager.Instance.isIntutorial)
            {
                TutorialManager.Instance.ShowDialogues();
            }
        }
    }

    public List<AudioClip> cookSFX;
    public List<AudioClip> cookSFX2;
    public void CreateDish(DishInfo info)
    {
        var dish = Instantiate(Resources.Load<GameObject>("Dish/Dish"), kichenToolTransforms[0]);
        dish.GetComponent<Dish>().Init(info,currentIngredientBases);

        if (info.isFinalDish)
        {
            RoundManager.Instance.CookMeal(dish.GetComponent<Dish>());
        }
    }

    private float cookTime = 0;
    private float cookTimer = 0;
    private void Update()
    {
        if (isCooking)
        {
            cookTimer += Time.deltaTime;
            if (cookTimer >= cookTime)
            {
                FinishCook();
                isCooking = false;
                cookTimer = 0;
                progressBar.gameObject.SetActive(false);
            }
            else
            {
                progressBar.SetProgress(cookTimer , cookTime);
            }
        }
    }
}
