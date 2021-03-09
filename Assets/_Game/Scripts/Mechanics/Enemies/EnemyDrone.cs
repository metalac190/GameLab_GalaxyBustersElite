using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyDrone : EnemyBase
{
    private GameObject playerReference;

    [Header("Effects")]
    [SerializeField] UnityEvent OnHit; // Not ever invoked since enemy dies in one hit
    [SerializeField] UnityEvent OnDead;

    private void Start()
    {
        playerReference = GameObject.FindGameObjectWithTag("Player");
    }

    public override void Update()
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

    public override void Arrival()
    {
        currentState = EnemyState.Passive;
    }

    /// Detection not needed for drones, only done for temp testing purposes
    public override void Passive()
    {
        if (Vector3.Distance(transform.position, playerReference.transform.position) < playerDetectionRadius)
        {
            Debug.Log("Detect range reached");
        }
    }

    public override void Attacking()
    {

    }

    public override void Dead()
    {
        Debug.Log("Enemy destroyed");

        OnDead.Invoke();

        Destroy(transform.parent.gameObject);
    }

    public override void Flee()
    {

    }
}
