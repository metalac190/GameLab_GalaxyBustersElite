using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRammer : EnemyBase
{
    private GameObject playerReference = null;

    private void Start()
    {
        playerReference = GameManager.player.obj;
    }

    private void FixedUpdate()
    {
        UpdateState();
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
            default:
                break;
        }
    }

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
        transform.position = Vector3.MoveTowards(transform.position, playerReference.transform.position, EnemyMoveSpeed * Time.deltaTime);
    }

    protected override void Dead()
    {
        Debug.Log("Enemy destroyed");
        Destroy(gameObject);
    }
}
