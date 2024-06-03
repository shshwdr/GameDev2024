using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public enum TutorialStage{start,dialogue,cook,serve,fight}
public class TutorialManager : Singleton<TutorialManager>
{
    public bool isIntutorial;
    public TutorialStage stage;
    private Customer customer;
    public Cook cook;
    private int dialogueIndex = -1;
    public Button button;

    private void Start()
    {
        button.onClick.AddListener(()=>
        {
            HideLastDialogue();
            if (stage == TutorialStage.dialogue)
            {
                var dialogueInfo = CSVLoader.Instance.TutorialDialogueInfos[dialogueIndex];
                // if (dialogueInfo.animWhenDialogue == "order")
                // {
                //     customer.PlayOrder();
                // }
                if (dialogueInfo.actionAfterDialogue == "finishDialogue")
                {
                    isIntutorial = false;
                    button.gameObject.SetActive(false);
                    RoundManager.Instance.StartRound();
                    return;
                }

                else if (dialogueInfo.actionAfterDialogue!=null && dialogueInfo.actionAfterDialogue.Length>0)
                {
                    stage = Enum.Parse<TutorialStage>(dialogueInfo.actionAfterDialogue);
                    button.gameObject.SetActive(false);
                    if (dialogueInfo.actionAfterDialogue == "fight")
                    {
                        EnemyManager.Instance.SpawnEnemy(CSVLoader.Instance.EnemyInfoDict.Values.ToList()[0],2);
                    }
                    return;
                }
                
                    
                    dialogueIndex++;
                ShowNextDialogue();
            }
        });
    }

    public void Init()
    {
        cook = GameObject.FindObjectOfType<Cook>();
        if (GameManager.Instance.skipTutorial)
        {
            button.gameObject.SetActive(false);
            return;
        }
        MusicManager.Instance.StartTutorial();
        isIntutorial = true;
        
        var enemy = EnemyManager.Instance.SpawnEnemy(CSVLoader.Instance.EnemyInfoDict.Values.ToList()[0],0);
        enemy.moveSpeed *= 4;
        stage = TutorialStage.start;

        StartCoroutine(createCustomer());
    }

    IEnumerator createCustomer()
    {
        yield return  new WaitForSeconds(4f);
        customer = CustomerManager.Instance.SpawnCustomer(CSVLoader. Instance.CustomerInfoDict.Values.ToList()[0]);
    }

    public void ShowDialogues()
    {
        dialogueIndex++;
        stage = TutorialStage.dialogue;
        ShowNextDialogue();
        
        button.gameObject.SetActive(true);
    }

    public void ShowNextDialogue()
    {
        var dialogueInfo = CSVLoader.Instance.TutorialDialogueInfos[dialogueIndex];
        if (dialogueInfo.speaker == "cook")
        {
            cook.ShowDialogue(dialogueInfo.dialogue);
        }
        else if (dialogueInfo.speaker == "customer")
        {
            customer.ShowDialogue(dialogueInfo.dialogue);
        }

    }

    public void HideLastDialogue()
    {
        if (dialogueIndex < 0)
        {
            return;
        }
        var dialogueInfo = CSVLoader.Instance.TutorialDialogueInfos[dialogueIndex];
        if (dialogueInfo.speaker == "cook")
        {
            cook.HideDialogue();
        }
        else if (dialogueInfo.speaker == "customer")
        {
            customer.HideDialogue();
        }
    }
}
