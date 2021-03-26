using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyState
{
    void UpdateState();
    void GoToAttackState();
    void GoToAlertState();
    void GoToPatrolState();
    void Hit();
    void OnCollisionStay2D(Collision2D collision);
}
