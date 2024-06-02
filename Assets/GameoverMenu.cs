using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameoverMenu : Singleton<GameoverMenu>
{
    public  GameObject gameoverMenu;
    public Button button;
    public void ShowGameoverMenu()
    {
        SFXManager.Instance.PlaySFX(SFXType.gameover);
        GameManager.Instance.isGameOver = true;
        gameoverMenu.SetActive(true);
        button.onClick.AddListener(() => {  GameManager.Instance.Restart();});
    }
    public void HideGameoverMenu()
    {
        gameoverMenu.SetActive(false);
    }

}
