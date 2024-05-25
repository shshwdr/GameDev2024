using System.Collections;
using System.Collections.Generic;
using Sinbad;
using UnityEngine;

public class IngredientInfo
{
    public string id;
    public string name;
    public int startCount;
    public int cost;
    public bool isMeat;
}
public class DishInfo
{
    public string id;
    public string name;
    public int cost;
    public bool isFinalDish;
    public List<string> ingredients;
    public string kichenUtil;
    public string description;
    public List<string> sourceIngredients;
    public bool isHot;
}
public class CSVLoader : Singleton<CSVLoader>
{
    
    
    
    public Dictionary<string, IngredientInfo> IngredientInfoDict = new Dictionary<string, IngredientInfo>();
    public Dictionary<string, DishInfo> DishInfoDict = new Dictionary<string, DishInfo>();
    public void Init()
    {
        var ingredientInfos = CsvUtil.LoadObjects<IngredientInfo>("ingredient");
        foreach (var info in ingredientInfos)
        {
            IngredientInfoDict[info.id] = info;
        }
        
         var dishInfos = CsvUtil.LoadObjects<DishInfo>("dish");
         foreach (var info in dishInfos)
         {
             DishInfoDict[info.id] = info;
         }
    }
}
