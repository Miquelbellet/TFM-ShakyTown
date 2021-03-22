using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IEnemyState
{
    EnemyControllerScript myEnemy;

    private Vector2 newPos;
    private float time;

    public PatrolState(EnemyControllerScript enemy)
    {
        myEnemy = enemy;
        SetNewPosition();
    }

    public void UpdateState()
    {
        if (myEnemy.canWalk)
        {
            GoToRandomPoint();
        }
    }

    void SetNewPosition()
    {
        time = 0;
        float posX = myEnemy.initEnemyPosition.x + Random.Range(myEnemy.walkableArea, -myEnemy.walkableArea);
        float posY = myEnemy.initEnemyPosition.y + Random.Range(myEnemy.walkableArea, -myEnemy.walkableArea);
        newPos = new Vector2(posX, posY);
    }

    void GoToRandomPoint()
    {
        time += Time.deltaTime;
        myEnemy.transform.position = Vector2.MoveTowards(myEnemy.transform.position, newPos, myEnemy.walkSpeed * Time.deltaTime);
        if (Vector2.Distance(myEnemy.transform.position, newPos) < 0.2 || time > myEnemy.maxTimeWalking)
        {
            SetNewPosition();
        }
    }

    public void GoToPatrolState()
    {

    }

    public void GoToAlertState()
    {
        myEnemy.currentState = myEnemy.alertState;
    }

    public void GoToAttackState()
    {

    }

    public void Hit()
    {
        GoToAlertState();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GoToAlertState();
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {

    }

    public void OnTriggerExit2D(Collider2D collision)
    {

    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            SetNewPosition();
        }
    }
}