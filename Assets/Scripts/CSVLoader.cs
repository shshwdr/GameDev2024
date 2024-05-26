using System.Collections;
using System.Collections.Generic;
using Sinbad;
using UnityEngine;

public class IngredientInfo:BaseInfo
{
    public int startCount;
    public int cost;
    public bool isMeat;
}

public class KichenToolInfo:BaseInfo
{
    
    public string id;
    public string name;
}

public class BaseInfo
{
    public string id;
    public string name;
}

public class EnemyInfo : BaseInfo
{
    public float moveSpeed;
    public int hp;
}

public class CustomerRequirementInfo
{
    public string description;
    public string requirementType;
    public string subType;
}

public class CustomerInfo : BaseInfo
{
    public float moveSpeed;
    public int attack;
    public float attackInterval;
}

public class DishInfo:BaseInfo
{
    public int cost;
    public bool isFinalDish;
    public List<string> ingredients;
    public string kichenUtil;
    public string description;
    public List<string> sourceIngredients;
    public bool isHot;
    public float time;
}
public class CSVLoader : Singleton<CSVLoader>
{
    
    
    
    public Dictionary<string, IngredientInfo> IngredientInfoDict = new Dictionary<string, IngredientInfo>();
    public Dictionary<string, DishInfo> DishInfoDict = new Dictionary<string, DishInfo>();
    public Dictionary<string, KichenToolInfo> KichenToolInfoDict = new Dictionary<string, KichenToolInfo>();
    public Dictionary<string, EnemyInfo> EnemyInfoDict = new Dictionary<string, EnemyInfo>();
    public Dictionary<string, CustomerInfo> CustomerInfoDict = new Dictionary<string, CustomerInfo>();
    public List<CustomerRequirementInfo> CustomerRequirementInfos = new List<CustomerRequirementInfo>();
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
         var kichenToolInfos = CsvUtil.LoadObjects<KichenToolInfo>("kichenTool");
         foreach (var info in kichenToolInfos)
         {
             KichenToolInfoDict[info.id] = info;
         }
         var enemyInfos = CsvUtil.LoadObjects<EnemyInfo>("enemy");
         foreach (var info in enemyInfos)
         {
             EnemyInfoDict[info.id] = info;
         }
         var customerInfos = CsvUtil.LoadObjects<CustomerInfo>("customer");
         foreach (var info in customerInfos)
         {
             CustomerInfoDict[info.id] = info;
         }
         CustomerRequirementInfos = CsvUtil.LoadObjects<CustomerRequirementInfo>("customerRequirement");
    }
}
