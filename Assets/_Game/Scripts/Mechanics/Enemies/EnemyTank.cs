using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTank : EnemyBase
{
    private GameObject playerReference = null;

    [Header("Enemy Tank Shots Before Vulnerability Pause")]
    [SerializeField] private int shotsBeforePauseMax = 1;

    [Header("Enemy Tank Attack Rate (Higher # = Longer Shot Delay)")]
    [SerializeField] private float attackRate = 0;

    [Header("Enemy Tank Vulnerability Period")]
    [SerializeField] private float vulnerabilityPeriodMax = 1;

    [Header("Enemy Tank Bullet Prefab")]
    [SerializeField] private GameObject bullet;

    [Header("Enemy Collider Ref (Don't Touch)")]
    [SerializeField] private BoxCollider invulnToggle;

    private float shotTime;
    private float currentVulnerabilityPeriod;
    private int currentShotsCount;

    //TEMP
    [Header("Temporary Visual Reference (Black = Vulnerable)")]
    [SerializeField] private Material invuln;
    [SerializeField] private Material vuln;

    private void Start()
    {
        playerReference = GameManager.player.obj;

        currentShotsCount = shotsBeforePauseMax;
        currentVulnerabilityPeriod = vulnerabilityPeriodMax;

        invulnToggle.enabled = false;
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
        }
    }

    protected override void Attacking()
    {
        bullet.GetComponent<EnemyProjectile>().SetDamage(AttackDamage);

        if (Vector3.Distance(transform.position, playerReference.transform.position) < EnemyDetectionRadius)
        {
            transform.LookAt(playerReference.transform.position);

            if (currentShotsCount > 0)
            {
                if (shotTime <= 0)
                {
                    shotTime = attackRate;
                    Instantiate(bullet, transform.position, transform.rotation);
                    currentShotsCount--;
                    Debug.Log(currentShotsCount);
                }
                else
                {
                    shotTime -= Time.deltaTime;
                }
            }
            else if (currentShotsCount == 0)
            {
                if (currentVulnerabilityPeriod <= 0)
                {
                    currentShotsCount = shotsBeforePauseMax;
                    currentVulnerabilityPeriod = vulnerabilityPeriodMax;
                    invulnToggle.enabled = false;

                    gameObject.GetComponentInChildren<Renderer>().material = invuln;
                }
                else
                {
                    gameObject.GetComponentInChildren<Renderer>().material = vuln;

                    invulnToggle.enabled = true;
                    currentVulnerabilityPeriod -= Time.deltaTime;
                }
            }
        }
    }

    public override void Dead()
    {
        Debug.Log("Enemy destroyed");
        Destroy(transform.parent.gameObject);
    }
}
