using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemySettings : ScriptableObject
{
    [Header("Enemy basic")]
    public float damage;
    public float health;
    public int dificultyLevel;
    public bool canWalk;

    [Header("Walkable settings")]
    public float walkSpeed;
    public float runSpeed;
    public float walkableArea;
    public float maxTimeWalking;
    public float playerDetectionRadius;
}
