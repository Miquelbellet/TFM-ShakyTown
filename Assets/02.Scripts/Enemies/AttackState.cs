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
        GoToAlertState();
    }

    public void GoToAlertState()
    {
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

    public void OnTriggerEnter2D(Collider2D collision)
    {

    }

    public void OnTriggerStay2D(Collider2D collision)
    {

    }

    public void OnTriggerExit2D(Collider2D collision)
    {

    }

    public void OnCollisionEnter2D(Collision2D collision)
    {

    }
}