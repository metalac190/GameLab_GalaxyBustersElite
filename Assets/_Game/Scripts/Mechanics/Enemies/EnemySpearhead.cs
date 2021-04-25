using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpearhead : EnemyBase
{
    [Header("Enemy Spearhead Charge Speed")]
    [SerializeField] private float chargingSpeed = 0;

    [Header("Enemy Spearhead Charge-Up Timer")]
    [SerializeField] private float chargeTimerMax = 1;
    private float chargeTimer;
    private bool isCharging;
    private bool singleChargeDone;

    [Header("Enemy Spearhead Despawn Time After Passing Player")]
    [SerializeField] private float despawnTime = 1f;
    private float currentTime = 0;

    [Header("Enemy Spearhead Movement Fix - Don't Touch")]
    [SerializeField] UnityEvent OnChargeAttackEnter;
    [SerializeField] UnityEvent OnChargeAttackExit;

    private Vector3 positionToChargeTowards;

    protected override void Start()
    {
        base.Start();
        chargeTimer = chargeTimerMax;
    }

    protected override void Passive()
    {
        if (Vector3.Distance(transform.position, GameManager.player.obj.transform.position) < EnemyDetectionRadius)
        {
            animator.SetBool("InPlayerRange", true);

            transform.LookAt(GameManager.player.obj.transform.position);

            currentState = EnemyState.Attacking;

            OnChargeAttackEnter.Invoke();
        }
    }

    protected override void Attacking()
    {
        if (!isCharging)
        {
            if (!singleChargeDone)
            {
                if (chargeTimer <= 0 && Vector3.Distance(transform.position, GameManager.player.obj.transform.position) < EnemyDetectionRadius)
                {
                    positionToChargeTowards = GameManager.player.obj.transform.position;
                    transform.LookAt(positionToChargeTowards);

                    chargeTimer = chargeTimerMax;

                    isCharging = true;
                }
                else if (chargeTimer != 0 && Vector3.Distance(transform.position, GameManager.player.obj.transform.position) < EnemyDetectionRadius)
                {
                    animator.SetTrigger("DamageTaken");
                    chargeTimer -= Time.deltaTime;
                }
            }
            else
            {
                transform.position += transform.forward * (chargingSpeed / 2) * Time.deltaTime;
                currentTime += Time.deltaTime;
                if (currentTime > despawnTime)
                    transform.parent.gameObject.SetActive(false);
            }
        }
        else
        {
            Charge();
        }

        if (Vector3.Distance(transform.position, GameManager.player.obj.transform.position) > EnemyDetectionRadius)
        {
            chargeTimer = chargeTimerMax;
            OnChargeAttackExit.Invoke();

            // Fixes bug of enemy charging at player, but player leaving range before charge finished
            if (isCharging == true)
            {
                currentTime += Time.deltaTime;
                if (currentTime > despawnTime)
                    transform.parent.gameObject.SetActive(false);
            }
        }
    }

    private void Charge()
    {
        transform.position = Vector3.MoveTowards(transform.position, positionToChargeTowards, chargingSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, positionToChargeTowards) == 0)
        {
            isCharging = false;
            singleChargeDone = true;
        }
    }

    protected override void OnTriggerEnter(Collider col)
    {
        // Two spearheads hitting each other will destroy each other, keep this in mind
        if (col.gameObject.layer == LayerMask.NameToLayer("Enemies"))
        {
            DialogueTrigger.TriggerEnemyDefeatedDialogue();
            col.gameObject.GetComponent<EnemyBase>().Dead();

            col.gameObject.GetComponent<EnemyBase>().Died.Invoke();
        }
        else if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            DialogueTrigger.TriggerEnemyDefeatedDialogue();
            col.gameObject.GetComponent<PlayerController>().DamagePlayer(AttackDamage);
            Dead();
        }
    }
}
