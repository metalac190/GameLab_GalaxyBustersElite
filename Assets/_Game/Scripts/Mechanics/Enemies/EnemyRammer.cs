using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class EnemyRammer : EnemyBase
{
    private GameObject playerReference = null;

    [Header("Enemy Rammer Charge Speed")]
    [SerializeField] private float ramSpeed = 0;

    [Header("Enemy Rammer Movement Fix - Don't Touch")]
    [SerializeField] UnityEvent OnRamAttackEnter;
    [SerializeField] UnityEvent OnRamAttackExit;

    protected override void Start()
    {
        base.Start();
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

            //when changing state, enable heat seeking behavior
            currentState = EnemyState.Attacking;
            HeatSeeker seeker = GetComponent<HeatSeeker>();
            seeker?.StartFollowing();
            seeker?.SetRotationSpeed(20f);

            OnRamAttackEnter.Invoke();
        }
    }

    protected override void Attacking()
    {
        if (Vector3.Distance(transform.position, playerReference.transform.position) < EnemyDetectionRadius)
        {
            //behavior moved to HeatSeeker behavior
            GetComponent<Rigidbody>().velocity = transform.forward * ramSpeed;
        }
        else if (Vector3.Distance(transform.position, playerReference.transform.position) > EnemyDetectionRadius)
        {
            OnRamAttackExit.Invoke();
        }
    }

    public override void Dead()
    {
        Debug.Log("Enemy destroyed");

        if (givesPlayerMS)
            camRailManager.IncreaseCamRailSpeed();

        Destroy(transform.parent.gameObject);
    }
}
