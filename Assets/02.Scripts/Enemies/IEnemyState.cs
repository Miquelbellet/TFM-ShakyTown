using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyState
{
    void UpdateState();
    void GoToAttackState();
    void GoToAlertState();
    void GoToPatrolState();
    void OnTriggerEnter2D(Collider2D collision);
    void OnTriggerStay2D(Collider2D collision);
    void OnTriggerExit2D(Collider2D collision);
    void OnCollisionStay2D(Collision2D collision);
    void Hit();
}
