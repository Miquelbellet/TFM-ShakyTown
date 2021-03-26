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
        if (!myEnemy.enemyAnimator.GetCurrentAnimatorStateInfo(0).IsTag("attack") && !myEnemy.enemyAnimator.GetCurrentAnimatorStateInfo(0).IsTag("hit") && !myEnemy.dead)
        {
            myEnemy.transform.position = Vector2.MoveTowards(myEnemy.transform.position, myEnemy.player.transform.position, myEnemy.enemySettings.runSpeed * Time.deltaTime);
            myEnemy.CheckPlayerDirection(myEnemy.player.transform.position);
        }
        else if (myEnemy.enemyAnimator.GetCurrentAnimatorStateInfo(0).IsTag("hit") && !myEnemy.dead)
        {
            myEnemy.enemyAnimator.SetBool("hit", false);
            myEnemy.enemyAnimator.SetBool("walk", true);
        }
        if (myEnemy.player.GetComponent<PlayerHealthScript>().playerDead)
        {
            GoToPatrolState();
        }
        CheckForPlayerDistance();
    }

    void CheckForPlayerDistance()
    {
        float playerDistance = Vector2.Distance(myEnemy.transform.position, myEnemy.player.transform.position);
        if (playerDistance > myEnemy.enemySettings.playerDetectionRadius)
        {
            GoToPatrolState();
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
        myEnemy.enemyAnimator.SetBool("walk", false);
        myEnemy.enemyAnimator.SetBool("hit", true);
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if (myEnemy.player.GetComponent<PlayerHealthScript>().canRecieveDamage && !myEnemy.player.GetComponent<PlayerHealthScript>().playerDead)
            {
                myEnemy.player.GetComponent<PlayerHealthScript>().PlayerHitted(myEnemy.enemySettings.damage);
                GoToAttackState();
            }
        }
    }
}