using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IEnemyState
{
    EnemyControllerScript myEnemy;

    public AttackState(EnemyControllerScript enemy)
    {
        myEnemy = enemy;
    }

    public void UpdateState()
    {
        if (myEnemy.enemyAnimator.GetCurrentAnimatorStateInfo(0).IsTag("attack"))
        {
            GoToAlertState();
        }
    }

    public void GoToAlertState()
    {
        myEnemy.enemyAnimator.speed = 1.3f;
        myEnemy.enemyAnimator.SetBool("attack", false);
        myEnemy.enemyAnimator.SetBool("walk", true);
        myEnemy.currentState = myEnemy.alertState;
    }

    public void GoToAttackState()
    {

    }

    public void GoToPatrolState()
    {

    }

    public void Hit()
    {

    }

    public void OnCollisionStay2D(Collision2D collision)
    {

    }
}