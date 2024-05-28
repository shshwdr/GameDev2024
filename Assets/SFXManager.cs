using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SFXType{
    placeItem,
    cutVeg,
    cutMeat,
    cook,
    seagullEnter,
    seagullEat,
    seagullHit,
    customerEnter,
    customerEat,
    customerHit,
    gameover
}
public class SFXManager : Singleton<SFXManager>
{
    public AudioSource audioSource;
    public List<AudioClip> placeItemSFX;
    public  List<AudioClip> cutVegSFX;
    public  List<AudioClip> cutMeatSFX;
     public  List<AudioClip> cookSFX;
     public  List<AudioClip> seagullEnterSFX;
     public  List<AudioClip> seagullEatSFX;
     public  List<AudioClip> seagullHitSFX;
     public  List<AudioClip> customerEnterSFX;
     public  List<AudioClip> customerEatSFX;
     public  List<AudioClip> customerHitSFX;
     
     public AudioClip gameoverSFX;

     public void PlaySFX(SFXType name)
     {
         switch (name)
         {
             case SFXType.placeItem:
                 audioSource.PlayOneShot(placeItemSFX.RandomItem());
                 break;
             case SFXType.cutVeg:
                 audioSource.PlayOneShot(cutVegSFX.RandomItem());
                 break;
             case SFXType.cutMeat:
                 audioSource.PlayOneShot(cutMeatSFX.RandomItem());
                 break;
             case SFXType.cook:
                 audioSource.PlayOneShot(cookSFX.RandomItem());
                 break;
             case SFXType.seagullEnter:
                 audioSource.PlayOneShot(seagullEnterSFX.RandomItem());
                 break;
             case SFXType.seagullEat:
                 audioSource.PlayOneShot(seagullEatSFX.RandomItem());
                 break;
             case SFXType.seagullHit:
                 audioSource.PlayOneShot(seagullHitSFX.RandomItem());
                 break;
             case SFXType.customerEnter:
                 audioSource.PlayOneShot(customerEnterSFX.RandomItem());
                 break;
             case SFXType.customerEat:
                 audioSource.PlayOneShot(customerEatSFX.RandomItem());
                 break;
             case SFXType.customerHit:
                 audioSource.PlayOneShot(customerHitSFX.RandomItem());
                 break;
             case SFXType.gameover:
                 audioSource.PlayOneShot(gameoverSFX);
                 break;
             }
     }
}
