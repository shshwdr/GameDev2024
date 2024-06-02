using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : Singleton<MusicManager>
{
    public AudioSource audioSource;
    public AudioClip battleClip;
    public AudioClip shopClip;
    public AudioClip tutorialClip;
    public void StartBattle()
    {
        audioSource.clip = battleClip;
        audioSource.Play();
    }
    public void StartShop()
    {
        audioSource.clip = shopClip;
        audioSource.Play();
    }

    public void StartTutorial()
    {
        audioSource.clip = tutorialClip;
        audioSource.Play();
    }
}
