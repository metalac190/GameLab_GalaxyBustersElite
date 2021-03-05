using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMinion : EnemyBase
{
    private GameObject playerReference = null;

    private void Start()
    {
        playerReference = GameObject.FindGameObjectWithTag("Player"); // TEMP
    }

    private void Update() // TEMP
    {
        UpdateState();
    }

    protected override void UpdateState()
    {
        switch (currentState)
        {
            case EnemyState.Arrival:
                Arrival();
                break;
            case EnemyState.Passive:
                Passive();
                break;
            case EnemyState.Attacking:
                Attacking();
                break;
            case EnemyState.Dead:
                Dead();
                break;
            case EnemyState.Flee:
                Flee();
                break;
            default:
                break;
        }
    }

    protected override void Arrival()
    {
        currentState = EnemyState.Passive;
    }

    /// Detection not needed for drones, only done for temp testing purposes
    protected override void Passive()
    {
        if (Vector3.Distance(transform.position, playerReference.transform.position) < EnemyDetectionRadius)
        {
            Debug.Log("Detect range reached");
            currentState = EnemyState.Attacking;
        }
    }

    protected override void Attacking()
    {
        FireProjectile();
    }

    protected override void Dead()
    {
        Debug.Log("Enemy destroyed");
        Destroy(transform.parent.gameObject);
    }

    protected override void Flee()
    {

    }
}
