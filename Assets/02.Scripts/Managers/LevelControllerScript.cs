using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelControllerScript : MonoBehaviour
{
    [Header("Levels settings")]
    public float timeChangingLevel;
    public GameObject backgroundImage;
    public GameObject[] levelsObjects;

    [Header("Player Level Positions")]
    public Vector2 playerPosHouse;
    public Vector2 playerPosLevel1;
    public Vector2 playerPosLevel2_1;
    public Vector2 playerPosLevel2_3;
    public Vector2 playerPosLevel2_4;
    public Vector2 playerPosLevel3;
    public Vector2 playerPosLevel4_3;
    public Vector2 playerPosLevel4_5;
    public Vector2 playerPosLevel5;

    enum Levels { Level_1, Level_2, Level_3, Level_4, Level_5 };
    Levels currentLevel;

    private GameObject player;
    private bool changingLevel;
    private int colliderNumber;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        currentLevel = Levels.Level_1;
    }

    public void PlayerChangedLevel(int numCollider)
    {
        if (!changingLevel)
        {
            changingLevel = true;
            Time.timeScale = 0;
            colliderNumber = numCollider;
            player.GetComponent<PlayerMovementScript>().stopRollMove = true;
            backgroundImage.SetActive(true);
            backgroundImage.GetComponent<Animator>().SetTrigger("fade_in");
        }
    }

    public void FadeInFinished()
    {
        RespawnAllEnemies();
        SetNewLevel();
        backgroundImage.GetComponent<Animator>().SetTrigger("fade_out");
    }

    public void FadeOutFinished()
    {
        changingLevel = false;
        backgroundImage.SetActive(false);
        Time.timeScale = 1;
    }

    void SetNewLevel()
    {
        switch (currentLevel)
        {
            case Levels.Level_1:
                if (colliderNumber == 1)
                {
                    SetActiveLevel(2);
                    player.transform.position = playerPosLevel2_1;
                    currentLevel = Levels.Level_2;
                }
                break;
            case Levels.Level_2:
                if (colliderNumber == 1)
                {
                    SetActiveLevel(1);
                    player.transform.position = playerPosLevel1;
                    currentLevel = Levels.Level_1;
                }
                else if (colliderNumber == 2)
                {
                    SetActiveLevel(3);
                    player.transform.position = playerPosLevel3;
                    currentLevel = Levels.Level_3;
                }
                else if (colliderNumber == 3)
                {
                    SetActiveLevel(4);
                    player.transform.position = playerPosLevel4_3;
                    currentLevel = Levels.Level_4;
                }
                break;
            case Levels.Level_3:
                if (colliderNumber == 2)
                {
                    SetActiveLevel(2);
                    player.transform.position = playerPosLevel2_3;
                    currentLevel = Levels.Level_2;
                }
                break;
            case Levels.Level_4:
                if (colliderNumber == 3)
                {
                    SetActiveLevel(2);
                    player.transform.position = playerPosLevel2_4;
                    currentLevel = Levels.Level_2;
                }
                else if (colliderNumber == 4)
                {
                    SetActiveLevel(5);
                    player.transform.position = playerPosLevel5;
                    currentLevel = Levels.Level_5;
                }
                break;
            case Levels.Level_5:
                if (colliderNumber == 4)
                {
                    SetActiveLevel(4);
                    player.transform.position = playerPosLevel4_5;
                    currentLevel = Levels.Level_4;
                }
                break;
        }
    }

    void SetActiveLevel(int levelNum)
    {
        for (int i = 0; i < levelsObjects.Length; i++)
        {
            if (levelNum == i+1) levelsObjects[i].SetActive(true);
            else levelsObjects[i].SetActive(false);
        }
    }

    void RespawnAllEnemies()
    {
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in allEnemies)
        {
            enemy.GetComponent<EnemyControllerScript>().RespawnEnemy();
        }
    }
}
