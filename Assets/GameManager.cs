using System.Collections;
using System.Collections.Generic;
using Sinbad;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public bool isGameOver = false;
    public Transform renderTrans;
    public bool skipTutorial = true;
    // Start is called before the first frame update
    void Start()
    {
        CSVLoader.Instance.Init();
        IngredientManager.Instance.Init();
        KichenToolManager.Instance.Init();
        EnemyManager.Instance.Init();
        CustomerManager.Instance.Init();
        RoundManager.Instance.Init();
        RecipeManager.Instance.Init();
        TutorialManager.Instance.Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public bool isInBattleView(Vector3 position)
    {
        return position.x > -3.18f&& position.x<0 && position.y < 1.74 && position.y > -1.74;
    }

    public Vector3 randomInBattleView()
    {
        return new Vector3(Random.Range(-3.18f, 0f), Random.Range(-1.74f, 1.74f), 0);
    }
}
