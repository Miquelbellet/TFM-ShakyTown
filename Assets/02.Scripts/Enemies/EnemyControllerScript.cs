using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControllerScript : MonoBehaviour
{
    public EnemyType enemyType;
    public EnemySettings batSettings;
    
    [HideInInspector] public enum EnemyType { Bat }
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

    private float enemylifes;

    void Start()
    {
        if (enemyType == EnemyType.Bat) enemySettings = batSettings;

        player = GameObject.FindGameObjectWithTag("Player");
        enemyAnimator = GetComponent<Animator>();
        enemySprite = GetComponent<SpriteRenderer>();
        if(initEnemyPosition == Vector2.zero) initEnemyPosition = transform.position;
        enemylifes = enemySettings.health;

        patrolState = new PatrolState(this);
        alertState = new AlertState(this);
        attackState = new AttackState(this);
        currentState = patrolState;
    }

    void Update()
    {
        currentState.UpdateState();
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
        if (enemylifes <= 0)
        {
            EnemyDead();
        }
    }

    public void RespawnEnemy()
    {
        Start();
        dead = false;
        transform.position = initEnemyPosition;
        enemylifes = enemySettings.health;
        currentState = patrolState;
        gameObject.SetActive(true);
    }

    void EnemyDead()
    {
        dead = true;
        gameObject.SetActive(false);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!dead) currentState.OnCollisionStay2D(collision);
    }
}
