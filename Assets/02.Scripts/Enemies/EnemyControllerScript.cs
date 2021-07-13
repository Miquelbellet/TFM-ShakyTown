using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EnemyControllerScript : MonoBehaviour
{
    [Header("Enemy config")]
    public EnemyType enemyType;
    public bool finalBoss;
    public GameObject droppedItemPrefab;

    [Header("Enemies Settings Level Easy")]
    public EnemySettings batTutorialSettingsEasy;
    public EnemySettings batSettingsEasy;
    public EnemySettings goblinSettingsEasy;
    public EnemySettings orcSettingsEasy;
    public EnemySettings ratSettingsEasy;
    public EnemySettings spiderSettingsEasy;
    public EnemySettings trollSettingsEasy;

    [Header("Enemies Settings Level Normal")]
    public EnemySettings batTutorialSettingsNormal;
    public EnemySettings batSettingsNormal;
    public EnemySettings goblinSettingsNormal;
    public EnemySettings orcSettingsNormal;
    public EnemySettings ratSettingsNormal;
    public EnemySettings spiderSettingsNormal;
    public EnemySettings trollSettingsNormal;

    [Header("Enemies Settings Level Hard")]
    public EnemySettings batTutorialSettingsHard;
    public EnemySettings batSettingsHard;
    public EnemySettings goblinSettingsHard;
    public EnemySettings orcSettingsHard;
    public EnemySettings ratSettingsHard;
    public EnemySettings spiderSettingsHard;
    public EnemySettings trollSettingsHard;

    [HideInInspector] public enum EnemyType { BatTutorial, Bat, Goblin, Orc, Rat, Spider, Troll }
    [HideInInspector] public EnemySettings enemySettings;

    [HideInInspector] public PatrolState patrolState;
    [HideInInspector] public AlertState alertState;
    [HideInInspector] public AttackState attackState;
    [HideInInspector] public IEnemyState currentState;

    [HideInInspector] public GameObject player;
    [HideInInspector] public Vector2 initEnemyPosition = Vector2.zero;
    [HideInInspector] public Animator enemyAnimator;
    [HideInInspector] public SpriteRenderer enemySprite;
    [HideInInspector] public float multiplicatorVelocity = 1;
    [HideInInspector] public bool dead;

    ResourcesManagmentScript resourcesManagmentScript;
    GameObject gameController;

    private float enemylifes;
    private Tool[] resourcesList;

    void Update()
    {
        if (currentState == null)
        {
            RespawnEnemy();
        }
        currentState.UpdateState();
    }

    void InitializeEnemy()
    {
        resourcesManagmentScript = new ResourcesManagmentScript();
        player = GameObject.FindGameObjectWithTag("Player");
        gameController = GameObject.FindGameObjectWithTag("GameController");
        enemyAnimator = GetComponent<Animator>();
        enemySprite = GetComponent<SpriteRenderer>();

        if (gameController.GetComponent<LevelControllerScript>().levelDifficulty == 0)
        {
            switch (enemyType)
            {
                case EnemyType.BatTutorial:
                    enemySettings = batTutorialSettingsEasy;
                    break;
                case EnemyType.Bat:
                    enemySettings = batSettingsEasy;
                    break;
                case EnemyType.Goblin:
                    enemySettings = goblinSettingsEasy;
                    break;
                case EnemyType.Orc:
                    enemySettings = orcSettingsEasy;
                    break;
                case EnemyType.Rat:
                    enemySettings = ratSettingsEasy;
                    break;
                case EnemyType.Spider:
                    enemySettings = spiderSettingsEasy;
                    break;
                case EnemyType.Troll:
                    enemySettings = trollSettingsEasy;
                    break;
            }
        }
        else if (gameController.GetComponent<LevelControllerScript>().levelDifficulty == 1)
        {
            switch (enemyType)
            {
                case EnemyType.BatTutorial:
                    enemySettings = batTutorialSettingsNormal;
                    break;
                case EnemyType.Bat:
                    enemySettings = batSettingsNormal;
                    break;
                case EnemyType.Goblin:
                    enemySettings = goblinSettingsNormal;
                    break;
                case EnemyType.Orc:
                    enemySettings = orcSettingsNormal;
                    break;
                case EnemyType.Rat:
                    enemySettings = ratSettingsNormal;
                    break;
                case EnemyType.Spider:
                    enemySettings = spiderSettingsNormal;
                    break;
                case EnemyType.Troll:
                    enemySettings = trollSettingsNormal;
                    break;
            }
        }
        else if (gameController.GetComponent<LevelControllerScript>().levelDifficulty == 2)
        {
            switch (enemyType)
            {
                case EnemyType.BatTutorial:
                    enemySettings = batTutorialSettingsHard;
                    break;
                case EnemyType.Bat:
                    enemySettings = batSettingsHard;
                    break;
                case EnemyType.Goblin:
                    enemySettings = goblinSettingsHard;
                    break;
                case EnemyType.Orc:
                    enemySettings = orcSettingsHard;
                    break;
                case EnemyType.Rat:
                    enemySettings = ratSettingsHard;
                    break;
                case EnemyType.Spider:
                    enemySettings = spiderSettingsHard;
                    break;
                case EnemyType.Troll:
                    enemySettings = trollSettingsHard;
                    break;
            }
        }

        SetResourcesList();
        if (initEnemyPosition == Vector2.zero) initEnemyPosition = transform.position;
        enemylifes = enemySettings.health;

        patrolState = new PatrolState(this);
        alertState = new AlertState(this);
        attackState = new AttackState(this);
        currentState = patrolState;
    }

    void SetResourcesList()
    {
        StreamReader resourcesDoc = resourcesManagmentScript.ReadDataFromResource("Assets/Resources/all_items.txt");
        resourcesList = new Tool[6];
        bool startList = false;
        int listCount = 0;
        for (int i = 0; i < Mathf.Infinity; i++)
        {
            string itemStr = resourcesDoc.ReadLine();
            if (startList)
            {
                if (itemStr == null) break;
                Tool item = Tool.CreateFromJSON(itemStr);
                resourcesList[listCount] = item;
                listCount++;
            }
            if (itemStr == "RESOURCES")
            {
                startList = true;
            }
        }
        resourcesDoc.Close();
    }

    public void CheckPlayerDirection(Vector2 followPos)
    {
        enemyAnimator.SetBool("side", false);
        enemyAnimator.SetBool("down", false);
        enemyAnimator.SetBool("up", false);

        Vector2 enemyPos = transform.position;
        Vector3 dir = (enemyPos - followPos).normalized;
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            if (dir.x > 0)
            {
                enemyAnimator.SetBool("side", true);
                enemySprite.flipX = true;
            }
            else
            {
                enemyAnimator.SetBool("side", true);
                enemySprite.flipX = false;
            }
        }
        else
        {
            if (dir.y > 0)
            {
                enemyAnimator.SetBool("down", true);
            }
            else
            {
                enemyAnimator.SetBool("up", true);
            }
        }
    }

    public void Hitted(float damage)
    {
        enemylifes -= damage;
        currentState.Hit();
    }

    public void RespawnEnemy()
    {
        InitializeEnemy();
        dead = false;
        transform.position = initEnemyPosition;
        enemylifes = enemySettings.health;
        currentState = patrolState;
        gameObject.SetActive(true);
    }

    public void EnemyDead()
    {
        if (enemylifes <= 0)
        {
            switch (enemyType)
            {
                case EnemyType.BatTutorial:
                    gameController.GetComponent<SoundsControllerScript>().PlayMonsterDie(1);
                    break;
                case EnemyType.Bat:
                    gameController.GetComponent<SoundsControllerScript>().PlayMonsterDie(1);
                    break;
                case EnemyType.Goblin:
                    gameController.GetComponent<SoundsControllerScript>().PlayMonsterDie(2);
                    break;
                case EnemyType.Orc:
                    gameController.GetComponent<SoundsControllerScript>().PlayMonsterDie(3);
                    break;
                case EnemyType.Rat:
                    gameController.GetComponent<SoundsControllerScript>().PlayMonsterDie(4);
                    break;
                case EnemyType.Spider:
                    gameController.GetComponent<SoundsControllerScript>().PlayMonsterDie(5);
                    break;
                case EnemyType.Troll:
                    gameController.GetComponent<SoundsControllerScript>().PlayMonsterDie(6);
                    break;
            }

            dead = true;
            gameObject.SetActive(false);
            DropItem();
            if (finalBoss)
            {
                player.GetComponent<PlayerScript>().DropNoteItem(22);
                Invoke("ReturnToHouse", 0.2f);
            }
        }
    }

    void ReturnToHouse()
    {
        player.GetComponent<PlayerScript>().ReturnToHouse();
    }

    void DropItem()
    {
        int randomResource = 0;
        int randomNumber = 1;
        if (enemySettings.dificultyLevel == 1)
        {
            randomResource = Random.Range(0, 3);
            randomNumber = Random.Range(1, 4);
        }
        else if (enemySettings.dificultyLevel == 2)
        {
            randomResource = Random.Range(0, 4);
            randomNumber = Random.Range(2, 6);
        }
        else if (enemySettings.dificultyLevel == 3)
        {
            randomResource = Random.Range(0, 6);
            randomNumber = Random.Range(3, 7);
        }
        else if (enemySettings.dificultyLevel == 4)
        {
            randomResource = Random.Range(0, 7);
            randomNumber = Random.Range(4, 8);
        }
        else return;

        GameObject dropItem = Instantiate(droppedItemPrefab, transform.position, Quaternion.Euler(0, 0, 0));
        dropItem.GetComponent<DroppedItemScript>().droppedTool = resourcesList[randomResource];
        dropItem.GetComponent<DroppedItemScript>().droppedTool.countItems = randomNumber;
        dropItem.GetComponent<DroppedItemScript>().SetItem(resourcesList[randomResource]);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!dead) currentState.OnCollisionStay2D(collision);
    }
}
