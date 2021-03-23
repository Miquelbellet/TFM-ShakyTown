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
        if (!myEnemy.enemyAnimator.GetCurrentAnimatorStateInfo(0).IsTag("attack"))
        {
            myEnemy.transform.position = Vector2.MoveTowards(myEnemy.transform.position, myEnemy.player.transform.position, myEnemy.runSpeed * Time.deltaTime);
            myEnemy.CheckPlayerDirection(myEnemy.player.transform.position);
        }
    }

    public void GoToAlertState()
    {

    }

    public void GoToAttackState()
    {
        myEnemy.enemyAnimator.speed = 1f;
        myEnemy.enemyAnimator.SetBool("walk", false);
        myEnemy.enemyAnimator.SetBool("attack", true);
        myEnemy.currentState = myEnemy.attackState;
    }

    public void GoToPatrolState()
    {
        myEnemy.enemyAnimator.speed = 0.8f;
        myEnemy.patrolState.SetNewPosition();
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

    public void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if (myEnemy.player.GetComponent<PlayerHealthScript>().canRecieveDamage)
            {
                myEnemy.player.GetComponent<PlayerHealthScript>().PlayerHitted(myEnemy.damage);
                GoToAttackState();
            }
        }
    }
}