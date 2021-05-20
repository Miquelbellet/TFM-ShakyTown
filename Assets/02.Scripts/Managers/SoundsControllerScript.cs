using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsControllerScript : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource musicAS;
    public AudioSource soundsAS;

    [Header("Audio Clips Musics")]
    public AudioClip musicLevel1;
    public AudioClip musicLevel2;
    public AudioClip musicLevel3;
    public AudioClip musicLevel4;
    public AudioClip musicLevel5;

    [Header("Audio Clips Monsters")]
    public AudioClip monsterDie1;
    public AudioClip monsterDie2;
    public AudioClip monsterDie3;
    public AudioClip monsterDie4;
    public AudioClip monsterDie5;
    public AudioClip monsterDie6;

    [Header("Audio Clips Sounds")]
    public AudioClip soundUI;
    public AudioClip selectItem;
    public AudioClip dash;
    public AudioClip chest;
    public AudioClip sword;
    public AudioClip arrow;
    public AudioClip dropItem;
    public AudioClip potion;
    public AudioClip buyItem;
    public AudioClip sellItem;
    public AudioClip note;
    public AudioClip playerHit;
    public AudioClip playerDead;


    public void PlayMusicForLevel(int levelNum)
    {
        musicAS.Stop();
        switch (levelNum)
        {
            case 1:
                musicAS.clip = musicLevel1;
                break;
            case 2:
                musicAS.clip = musicLevel2;
                break;
            case 3:
                musicAS.clip = musicLevel3;
                break;
            case 4:
                musicAS.clip = musicLevel4;
                break;
            case 5:
                musicAS.clip = musicLevel5;
                break;
        }
        musicAS.Play();
    }

    public void PlayMonsterDie(int levelMonster)
    {
        switch (levelMonster)
        {
            case 1:
                soundsAS.PlayOneShot(monsterDie1);
                break;
            case 2:
                soundsAS.PlayOneShot(monsterDie2);
                break;
            case 3:
                soundsAS.PlayOneShot(monsterDie3);
                break;
            case 4:
                soundsAS.PlayOneShot(monsterDie4);
                break;
            case 5:
                soundsAS.PlayOneShot(monsterDie5);
                break;
            case 6:
                soundsAS.PlayOneShot(monsterDie6);
                break;
        }
    }

    public void PlaySoundUI1()
    {
        soundsAS.PlayOneShot(soundUI);
    }

    public void PlaySelectItem()
    {
        soundsAS.PlayOneShot(selectItem);
    }

    public void PlayDash()
    {
        soundsAS.PlayOneShot(dash);
    }

    public void PlayChest()
    {
        soundsAS.PlayOneShot(chest);
    }

    public void PlaySword()
    {
        soundsAS.PlayOneShot(sword);
    }

    public void PlayArrow()
    {
        soundsAS.PlayOneShot(arrow);
    }

    public void PlayDropItem()
    {
        soundsAS.PlayOneShot(dropItem);
    }

    public void PlayPotion()
    {
        soundsAS.PlayOneShot(potion);
    }

    public void PlayBuyItem()
    {
        soundsAS.PlayOneShot(buyItem);
    }

    public void PlaySellItem()
    {
        soundsAS.PlayOneShot(sellItem);
    }

    public void PlayNote()
    {
        soundsAS.PlayOneShot(note);
    }

    public void PlayPlayerHit()
    {
        soundsAS.PlayOneShot(playerHit);
    }

    public void PlayPlayerDead()
    {
        soundsAS.PlayOneShot(playerDead);
    }
}
