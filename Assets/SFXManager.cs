using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SFXType{
    placeItem,
    seagullEnter,
    seagullEat,
    seagullHit,
    customerEnter,
    customerEat,
    customerEatHappy,
    customerHit,
    customerKick,
    purchase,
    splatBig,
    gameover
}
public class SFXManager : Singleton<SFXManager>
{
    public AudioSource audioSource;
    public List<AudioClip> placeItemSFX;
     public  List<AudioClip> seagullEnterSFX;
     public  List<AudioClip> seagullEatSFX;
     public  List<AudioClip> seagullHitSFX;
     public  List<AudioClip> customerEnterSFX;
     public  List<AudioClip> customerEatSFX;
     public  List<AudioClip> customerEatHappySFX;
     public  List<AudioClip> customerHitSFX;
     public  List<AudioClip> customerKickSFX;
     public List<AudioClip> purchaseSFX;
     public List<AudioClip> splatBig;
     public AudioClip gameoverSFX;

     public void PlaySFX(SFXType name)
     {
         switch (name)
         {
             case SFXType.placeItem:
                 audioSource.PlayOneShot(placeItemSFX.RandomItem());
                 break;
             case SFXType.purchase:
                 audioSource.PlayOneShot(purchaseSFX.RandomItem());
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
             case SFXType.customerEatHappy:
                 audioSource.PlayOneShot(customerEatHappySFX.RandomItem());
                 break;
             case SFXType.customerHit:
                 audioSource.PlayOneShot(customerHitSFX.RandomItem());
                 break;
             case SFXType.customerKick:
                 audioSource.PlayOneShot(customerKickSFX.RandomItem());
                 break;
             case SFXType.splatBig:
                 audioSource.PlayOneShot(splatBig.RandomItem());
                 break;
             case SFXType.gameover:
                 audioSource.PlayOneShot(gameoverSFX);
                 break;
             }
     }
}
