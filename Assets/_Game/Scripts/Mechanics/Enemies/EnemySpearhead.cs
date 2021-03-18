using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpearhead : EnemyBase
{
    private GameObject playerReference = null;

    [Header("Enemy Spearhead Charge Speed")]
    [SerializeField] private float chargingSpeed = 0;

    [Header("Enemy Spearhead Charge-Up Timer")]
    [SerializeField] private float chargeTimerMax = 1;
    private float chargeTimer;
    private bool isCharging;

    [Header("Enemy Spearhead Movement Fix - Don't Touch")]
    [SerializeField] UnityEvent OnChargeAttackEnter;
    [SerializeField] UnityEvent OnChargeAttackExit;

    private Vector3 positionToChargeTowards;

    private void Start()
    {
        playerReference = GameManager.player.obj;
        chargeTimer = chargeTimerMax;
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

            OnChargeAttackEnter.Invoke();
        }
    }

    protected override void Attacking()
    {
        if (!isCharging)
        {
            if (chargeTimer <= 0 && Vector3.Distance(transform.position, playerReference.transform.position) < EnemyDetectionRadius)
            {
                positionToChargeTowards = playerReference.transform.position;
                isCharging = true;
                chargeTimer = chargeTimerMax;

                transform.LookAt(playerReference.transform.position);
            }
            else if (chargeTimer != 0 && Vector3.Distance(transform.position, playerReference.transform.position) < EnemyDetectionRadius)
            {
                chargeTimer -= Time.deltaTime;

                transform.LookAt(playerReference.transform.position);
            }
        }
        else
        {
            Charge();
        }

        if (Vector3.Distance(transform.position, playerReference.transform.position) > EnemyDetectionRadius)
        {
            chargeTimer = chargeTimerMax;
            OnChargeAttackExit.Invoke();
        }
    }

    private void Charge()
    {
        transform.position = Vector3.MoveTowards(transform.position, positionToChargeTowards, chargingSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, positionToChargeTowards) == 0)
        {
            isCharging = false;
        }
    }

    public override void Dead()
    {
        Debug.Log("Enemy destroyed");
        Destroy(transform.parent.gameObject);
    }
}
