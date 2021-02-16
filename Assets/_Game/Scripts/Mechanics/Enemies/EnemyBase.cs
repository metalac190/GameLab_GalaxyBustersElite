using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    public enum EnemyState { Arrival, Passive, Detected, Attacking, Dead, Flee }

    [Header("Enemy State")]
    public EnemyState currentState;

    [Header("Enemy Movement")]
    public EnemyMovement.EnemyMovementPatterns currentMovementPattern;
    public EnemyMovement.EnemyZAxisMovement currentZAxisMovement;
    public EnemyMovement.EnemySpawnMovement currentSpawnMovement;

    [Header("Basic Enemy Variables")]
    public int enemyHealth = 0;
    public int enemyDamage = 0;
    public float enemySpeed = 0;
    public float attackRate = 0;
    public float playerDetectionRadius = 0;

    void Start()
    {
        currentState = EnemyState.Arrival;
    }

    public virtual void Update()
    {
        switch (currentState)
        {
            case EnemyState.Arrival:
                break;
            case EnemyState.Passive:
                break;
            case EnemyState.Detected:
                break;
            case EnemyState.Attacking:
                break;
            case EnemyState.Dead:
                break;
            case EnemyState.Flee:
                break;
            default:
                break;
        }
    }

    public virtual void Passive()
    {

    }

    public virtual void Detected()
    {

    }

    public virtual void Attacking()
    {

    }

    public virtual void Dead()
    {

    }

    public virtual void Flee()
    {

    }
}
