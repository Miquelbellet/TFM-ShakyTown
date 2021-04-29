using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class LevelControllerScript : MonoBehaviour
{
    [Header("Levels settings")]
    public float timeChangingLevel;
    public GameObject backgroundImage;
    public GameObject level4Obstacle;
    public GameObject[] levelsObjects;
    public GameObject[] levelsEnemiesObjects;

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

    [Header("Final Level Enemy Objects")]
    public GameObject bossObject;
    public GameObject enemiesPartentObj;

    [HideInInspector] public enum Levels { Level_1, Level_2, Level_3, Level_4, Level_5 };
    [HideInInspector] public Levels currentLevel = Levels.Level_1;
    [HideInInspector] public int currentLevelNumber;
    [HideInInspector] public bool openLevel4Obstacle;

    ResourcesManagmentScript resourcesManagmentScript;

    private GameObject player;
    private bool changingLevel;
    private bool resetingPlayer;
    private int colliderNumber;
    private bool savingGame;
    private bool endingTutorial;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        resourcesManagmentScript = new ResourcesManagmentScript();
        if (PlayerPrefs.GetString("dadHelpNote", "false") == "true") openLevel4Obstacle = true;
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

    public void FadeInAnimation()
    {
        endingTutorial = true;
        backgroundImage.SetActive(true);
        backgroundImage.GetComponent<Animator>().SetTrigger("fade_in");
    }

    public void FadeInFinished()
    {
        if (savingGame) { Time.timeScale = 1; }
        else if (endingTutorial) { player.GetComponent<PlayerScript>().ChangeToMainScene(); }
        else if (!resetingPlayer)
        {
            SetNewLevel();
            backgroundImage.GetComponent<Animator>().SetTrigger("fade_out");
        }
        else
        {
            resetingPlayer = false;
            SetActiveLevel(1);
            player.transform.position = playerPosHouse;
            Camera.main.transform.position = new Vector3(playerPosHouse.x, playerPosHouse.y, Camera.main.transform.position.z);
            currentLevel = Levels.Level_1;
            currentLevelNumber = 1;
            player.GetComponent<PlayerHealthScript>().ResetPlayerLife();
            backgroundImage.GetComponent<Animator>().SetTrigger("fade_out");
        }
        
    }

    public void FadeOutFinished()
    {
        changingLevel = false;
        backgroundImage.SetActive(false);
        Time.timeScale = 1;
        if (colliderNumber == 4) StartCoroutine(ShowDialog());
    }

    IEnumerator ShowDialog()
    {
        for (int i = 15; i <= 21; i++)
        {
            if(i == 21) Invoke("finalLine", 0.2f);
            else player.GetComponent<PlayerScript>().DropNoteItem(i);
            yield return new WaitForSeconds(0.2f);
        }
    }

    void finalLine()
    {
        bossObject.SetActive(false);
        enemiesPartentObj.SetActive(true);
        player.GetComponent<PlayerScript>().DropNoteItem(21);
    }

    public void ResetPlayer()
    {
        resetingPlayer = true;
        changingLevel = true;
        Time.timeScale = 0;
        player.GetComponent<PlayerMovementScript>().stopRollMove = true;
        backgroundImage.SetActive(true);
        backgroundImage.GetComponent<Animator>().SetTrigger("fade_in");
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
                }
                break;
            case Levels.Level_2:
                if (colliderNumber == 1)
                {
                    SetActiveLevel(1);
                    player.transform.position = playerPosLevel1;
                }
                else if (colliderNumber == 2)
                {
                    SetActiveLevel(3);
                    player.transform.position = playerPosLevel3;
                }
                else if (colliderNumber == 3)
                {
                    SetActiveLevel(4);
                    player.transform.position = playerPosLevel4_3;
                }
                break;
            case Levels.Level_3:
                if (colliderNumber == 2)
                {
                    SetActiveLevel(2);
                    player.transform.position = playerPosLevel2_3;
                }
                break;
            case Levels.Level_4:
                if (colliderNumber == 3)
                {
                    SetActiveLevel(2);
                    player.transform.position = playerPosLevel2_4;
                }
                else if (colliderNumber == 4)
                {
                    SetActiveLevel(5);
                    player.transform.position = playerPosLevel5;
                }
                break;
            case Levels.Level_5:
                if(colliderNumber == 1)
                {
                    SetActiveLevel(1);
                    player.transform.position = playerPosHouse;
                }
                break;
        }
    }
    
    public void SetActiveLevel(int levelNum)
    {
        currentLevelNumber = levelNum;
        if (levelNum == 1) currentLevel = Levels.Level_1;
        else if (levelNum == 2) currentLevel = Levels.Level_2;
        else if (levelNum == 3) currentLevel = Levels.Level_3;
        else if (levelNum == 4) currentLevel = Levels.Level_4;
        else if (levelNum == 5) currentLevel = Levels.Level_5;

        for (int i = 0; i < levelsObjects.Length; i++)
        {
            if (levelNum == i+1)
            {
                for (int n = 0; n < levelsEnemiesObjects[i].transform.childCount; n++)
                {
                    levelsEnemiesObjects[i].transform.GetChild(n).gameObject.GetComponent<EnemyControllerScript>().RespawnEnemy();
                }
                levelsObjects[i].SetActive(true);

                if(levelNum == 2)
                {
                    if (openLevel4Obstacle) level4Obstacle.SetActive(false);
                    else level4Obstacle.SetActive(true);
                }
            }
            else
            {
                levelsObjects[i].SetActive(false);
            }
        }
    }

    public bool CheckEnemiesInLevel(int levelNum)
    {
        bool allEnabled = true;
        for (int i = 0; i < levelsEnemiesObjects[levelNum - 1].transform.childCount; i++)
        {
            if (levelsEnemiesObjects[levelNum - 1].transform.GetChild(i).gameObject.activeSelf) allEnabled = false;
        }
        return allEnabled;
    }

    public void SaveGame()
    {
        GetComponent<ToolBarScript>().SaveBarTools();
        player.GetComponent<PlayerScript>().SavePlayerSettings();
        SaveChestsItems();
        AssetDatabase.Refresh();

        savingGame = true;
        backgroundImage.SetActive(true);
        backgroundImage.GetComponent<Animator>().SetTrigger("fade_in");
    }

    private void SaveChestsItems()
    {
        GameObject[] allChests = GameObject.FindGameObjectsWithTag("Chest");
        foreach (GameObject chest in allChests)
        {
            string path = "Assets/Resources/chests/chest_"+ chest.GetComponent<ChestScript>().chestId + ".txt";
            StreamWriter chestObjects = resourcesManagmentScript.WriteDataToResource(path);
            Tool[] chestObjectsList = chest.GetComponent<ChestScript>().chestObjectsList;
            foreach (Tool toolObj in chestObjectsList)
            {
                string toolStr = Tool.CreateFromObject(toolObj);
                chestObjects.WriteLine(toolStr);
            }
            chestObjects.Close();
        }
    }
}
