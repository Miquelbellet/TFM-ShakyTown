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

    [HideInInspector] public GameObject player;
    [HideInInspector] public IEnemyState currentState;
    [HideInInspector] public Vector2 initEnemyPosition;

    private bool dead;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        patrolState = new PatrolState(this);
        alertState = new AlertState(this);
        attackState = new AttackState(this);
        currentState = patrolState;
        initEnemyPosition = transform.position;
        GetComponent<CircleCollider2D>().radius = playerDetectionRadius;
    }

    void Update()
    {
        currentState.UpdateState();
        if (health < 0 && !dead)
        {

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!dead) currentState.OnCollisionEnter2D(collision);
    }
}
