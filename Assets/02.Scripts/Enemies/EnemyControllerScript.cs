using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EnemyControllerScript : MonoBehaviour
{
    public EnemyType enemyType;
    public EnemySettings batTutorialSettings;
    public EnemySettings batSettings;
    public GameObject droppedItemPrefab;

    [HideInInspector] public enum EnemyType { BatTutorial, Bat }
    [HideInInspector] public EnemySettings enemySettings;

    [HideInInspector] public PatrolState patrolState;
    [HideInInspector] public AlertState alertState;
    [HideInInspector] public AttackState attackState;
    [HideInInspector] public IEnemyState currentState;

    [HideInInspector] public GameObject player;
    [HideInInspector] public Vector2 initEnemyPosition = Vector2.zero;
    [HideInInspector] public Animator enemyAnimator;
    [HideInInspector] public SpriteRenderer enemySprite;
    [HideInInspector] public bool dead;

    ResourcesManagmentScript resourcesManagmentScript;

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
        if (enemyType == EnemyType.BatTutorial) enemySettings = batTutorialSettings;
        else if (enemyType == EnemyType.Bat) enemySettings = batSettings;

        player = GameObject.FindGameObjectWithTag("Player");
        enemyAnimator = GetComponent<Animator>();
        enemySprite = GetComponent<SpriteRenderer>();
        resourcesManagmentScript = new ResourcesManagmentScript();
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
            dead = true;
            gameObject.SetActive(false);
            DropItem();
        }
    }

    void DropItem()
    {
        int randomResource;
        int randomNumber;
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
        else
        {
            randomResource = 0;
            randomNumber = 1;
        }

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
