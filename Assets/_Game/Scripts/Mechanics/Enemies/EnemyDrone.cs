using System.Collections;
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
            case EnemyState.Passive:
                Passive();
                break;
            case EnemyState.Attacking:
                Attacking();
                break;
            case EnemyState.Dead:
                Dead();
                break;
            default:
                break;
        }
    }

    protected override void Passive()
    {

    }

    protected override void Attacking()
    {

    }

    protected override void Dead()
    {
        Debug.Log("Enemy destroyed");
        Destroy(gameObject);
    }
}
