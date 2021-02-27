using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public abstract class EnemyBase : MonoBehaviour
{
    public enum EnemyState { Arrival, Passive, Attacking, Dead, Flee }

    [Header("Enemy State")]
    public EnemyState currentState;

    [Header("Basic Enemy Variables")]
    public int enemyHealth = 1;
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

#if UNITY_EDITOR
    /// Visual radius of enemy detection radius if enemy selected in editor
    public virtual void OnDrawGizmosSelected()
    {
        Handles.DrawWireDisc(transform.position, Vector3.up, playerDetectionRadius);
    }
#endif

    public virtual void Arrival()
    {

    }

    public virtual void Passive()
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
