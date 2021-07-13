using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PatrolState : IEnemyState
{
    EnemyControllerScript myEnemy;

    private Vector2 newPos;
    private float time;

    public PatrolState(EnemyControllerScript enemy)
    {
        myEnemy = enemy;
    }

    public void UpdateState()
    {
        if(myEnemy.enemyAnimator.GetCurrentAnimatorStateInfo(0).IsTag("hit")) myEnemy.enemyAnimator.SetBool("hit", false);
        if (myEnemy.enemySettings.canWalk)
        {
            GoToRandomPoint();
            myEnemy.enemyAnimator.SetBool("walk", true);
        }
        else
        {
            myEnemy.enemyAnimator.SetBool("walk", false);
        }
        CheckForPlayerDistance();
    }

    public void SetNewPosition()
    {
        time = 0;
        float posX = myEnemy.initEnemyPosition.x + Random.Range(myEnemy.enemySettings.walkableArea, -myEnemy.enemySettings.walkableArea);
        float posY = myEnemy.initEnemyPosition.y + Random.Range(myEnemy.enemySettings.walkableArea, -myEnemy.enemySettings.walkableArea);
        newPos = new Vector2(posX, posY);
        myEnemy.CheckPlayerDirection(newPos);
    }

    void GoToRandomPoint()
    {
        time += Time.deltaTime;
        myEnemy.transform.position = Vector2.MoveTowards(myEnemy.transform.position, newPos, myEnemy.enemySettings.walkSpeed * myEnemy.multiplicatorVelocity * Time.deltaTime);
        if (Vector2.Distance(myEnemy.transform.position, newPos) < 0.2 || time > myEnemy.enemySettings.maxTimeWalking)
        {
            SetNewPosition();
        }
    }

    void CheckForPlayerDistance()
    {
        float playerDistance = Vector2.Distance(myEnemy.transform.position, myEnemy.player.transform.position);
        if (playerDistance < myEnemy.enemySettings.playerDetectionRadius)
        {
            GoToAlertState();
        }
    }

    public void GoToPatrolState()
    {

    }

    public void GoToAlertState()
    {
        myEnemy.enemyAnimator.speed = 1.3f;
        myEnemy.currentState = myEnemy.alertState;
    }

    public void GoToAttackState()
    {

    }

    public void Hit()
    {
        myEnemy.enemyAnimator.SetBool("walk", false);
        myEnemy.enemyAnimator.SetBool("hit", true);
        if(SceneManager.GetActiveScene().name != "InitTutorial") GoToAlertState();
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            SetNewPosition();
        }
    }
}
