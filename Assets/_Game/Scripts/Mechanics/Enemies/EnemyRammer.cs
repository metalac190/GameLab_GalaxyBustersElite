using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyRammer : EnemyBase
{
    private GameObject playerReference = null;

    [Header("Enemy Rammer Charge Speed")]
    [SerializeField] private float enemyChargeSpeed = 0;

    [Header("Enemy Rammer Movement Fix - Don't Touch")]
    [SerializeField] UnityEvent OnRamAttackEnter;
    [SerializeField] UnityEvent OnRamAttackExit;

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
            transform.LookAt(playerReference.transform.position);

            currentState = EnemyState.Attacking;

            OnRamAttackEnter.Invoke();
        }
    }

    protected override void Attacking()
    {
        if (Vector3.Distance(transform.position, playerReference.transform.position) < EnemyDetectionRadius)
        {
            transform.LookAt(playerReference.transform.position);
            transform.position = Vector3.MoveTowards(transform.position, playerReference.transform.position, enemyChargeSpeed * Time.deltaTime);
        }
        else if (Vector3.Distance(transform.position, playerReference.transform.position) > EnemyDetectionRadius)
        {
            OnRamAttackExit.Invoke();
        }
    }

    public override void Dead()
    {
        Debug.Log("Enemy destroyed");
        Destroy(transform.parent.gameObject);
    }
}
