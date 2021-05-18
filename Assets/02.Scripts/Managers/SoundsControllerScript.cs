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

    [Header("Audio Clips Sounds")]
    public AudioClip soundUI1;
    public AudioClip selectItem;
    public AudioClip dash;
    public AudioClip chest;
    public AudioClip sword;
    public AudioClip arrow;


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

    public void PlaySoundUI1()
    {
        soundsAS.PlayOneShot(soundUI1);
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
}
