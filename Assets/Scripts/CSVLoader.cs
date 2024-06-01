using System.Collections;
using System.Collections.Generic;
using Sinbad;
using UnityEngine;

public class IngredientInfo:BaseInfo
{
    public int startCount;
    public int cost;
    public bool isMeat;
    public bool needCut;
}

public class KichenToolInfo:BaseInfo
{
    
    public string id;
    public string name;
    public int cost;
    public int startCount;
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
    public float duration;
    public float criticalRate;
}

public class DishInfo:BaseInfo
{
    public int cost;
    public bool isFinalDish;
    public Dictionary<string,int> ingredients;
    public string kichenUtil;
    public string description;
    public bool isHot;
    public float time;
    public string image;
    public Dictionary<string, int> buff;

    public string buffString()
    {
        var str = "";
        foreach (var b in buff)
        {
            str+=b.Key+" +"+b.Value;
        }

        return str;
    }
}

public class TutorialDialogueInfo
{
    public string dialogue;
    public string speaker;
    public string actionAfterDialogue;
    public string animWhenDialogue;

}

public class EnemyRoundInfo
{
    public int preTime;
    public List<string> enemies;
    public int enemyCount;
    public float minInterval;
    public float maxInterval;
    public int customerCount;
    public float customerMinInterval;
    public float customerMaxInterval;


}
public class CSVLoader : Singleton<CSVLoader>
{
    
    
    
    public Dictionary<string, IngredientInfo> IngredientInfoDict = new Dictionary<string, IngredientInfo>();
    public Dictionary<string, DishInfo> DishInfoDict = new Dictionary<string, DishInfo>();
    public Dictionary<string, KichenToolInfo> KichenToolInfoDict = new Dictionary<string, KichenToolInfo>();
    public Dictionary<string, EnemyInfo> EnemyInfoDict = new Dictionary<string, EnemyInfo>();
    public Dictionary<string, CustomerInfo> CustomerInfoDict = new Dictionary<string, CustomerInfo>();
    public List<CustomerRequirementInfo> CustomerRequirementInfos = new List<CustomerRequirementInfo>();
    public List<EnemyRoundInfo> EnemyRoundInfos = new List<EnemyRoundInfo>();
    public List<TutorialDialogueInfo> TutorialDialogueInfos = new List<TutorialDialogueInfo>();
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
         var kichenToolInfos = CsvUtil.LoadObjects<KichenToolInfo>("KichenTool");
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
          EnemyRoundInfos = CsvUtil.LoadObjects<EnemyRoundInfo>("enemyRound");
          TutorialDialogueInfos = CsvUtil.LoadObjects<TutorialDialogueInfo>("tutorialDialogue");
    }
}
