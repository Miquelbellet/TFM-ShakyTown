using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControllerScript : MonoBehaviour
{
    [Header("Enemy type")]
    public EnemyType enemyType;
    public float damage;
    public float health;
    public bool canWalk;

    [Header("Enemy caracteristics")]
    public float walkSpeed;
    public float runSpeed;
    public float walkableArea;
    public float maxTimeWalking;
    public float playerDetectionRadius;
    
    [HideInInspector] public enum EnemyType { Bat }
    [HideInInspector] public PatrolState patrolState;
    [HideInInspector] public AlertState alertState;
    [HideInInspector] public AttackState attackState;

    [HideInInspector] public IEnemyState currentState;
    [HideInInspector] public GameObject player;
    [HideInInspector] public Vector2 initEnemyPosition;
    [HideInInspector] public Animator enemyAnimator;
    [HideInInspector] public SpriteRenderer enemySprite;

    private bool dead;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        enemyAnimator = GetComponent<Animator>();
        enemySprite = GetComponent<SpriteRenderer>();
        GetComponent<CircleCollider2D>().radius = playerDetectionRadius;
        initEnemyPosition = transform.position;
        
        patrolState = new PatrolState(this);
        alertState = new AlertState(this);
        attackState = new AttackState(this);
        currentState = patrolState;
    }

    void Update()
    {
        currentState.UpdateState();
        if (health < 0 && !dead)
        {

        }
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
        health -= damage;
        currentState.Hit();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!dead) currentState.OnTriggerEnter2D(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!dead) currentState.OnTriggerStay2D(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!dead) currentState.OnTriggerExit2D(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!dead) currentState.OnCollisionStay2D(collision);
    }
}
