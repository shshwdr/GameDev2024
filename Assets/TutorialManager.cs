using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : Singleton<TutorialManager>
{
    public bool isIntutorial;
    public void Init()
    {
        isIntutorial = true;
    }
}
