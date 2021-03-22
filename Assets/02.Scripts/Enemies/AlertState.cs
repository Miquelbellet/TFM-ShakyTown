using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertState : IEnemyState
{
    EnemyControllerScript myEnemy;

    public AlertState(EnemyControllerScript enemy)
    {
        myEnemy = enemy;
    }

    public void UpdateState()
    {
        myEnemy.transform.position = Vector2.MoveTowards(myEnemy.transform.position, myEnemy.player.transform.position, myEnemy.runSpeed * Time.deltaTime);
    }

    public void GoToAlertState()
    {

    }

    public void GoToAttackState()
    {
        myEnemy.currentState = myEnemy.attackState;
    }

    public void GoToPatrolState()
    {
        myEnemy.currentState = myEnemy.patrolState;
    }

    public void Hit()
    {

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {

    }

    public void OnTriggerStay2D(Collider2D collision)
    {

    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            GoToPatrolState();
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            GoToAttackState();
        }
    }
}