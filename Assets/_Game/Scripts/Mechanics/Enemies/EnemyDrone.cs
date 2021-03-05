﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDrone : EnemyBase
{
    private GameObject playerReference = null;

    private void Start()
    {
        playerReference = GameManager.player.obj;
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
        }
    }

    protected override void Attacking()
    {

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
