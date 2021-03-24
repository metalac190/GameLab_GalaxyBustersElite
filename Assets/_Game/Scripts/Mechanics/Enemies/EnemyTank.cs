using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyTank : EnemyBase
{
    [Header("Enemy Tank Shots Before Vulnerability Pause")]
    [SerializeField] private int shotsBeforePauseMax = 1;

    [Header("Enemy Tank Attack Rate (Higher # = Longer Shot Delay)")]
    [SerializeField] private float attackRate = 0;

    [Header("Enemy Tank Vulnerability Period")]
    [SerializeField] private float vulnerabilityPeriodMax = 1;

    [Header("Enemy Tank Bullet Prefab")]
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform _spawnPoint;
    private List<GameObject> _bulletPool = new List<GameObject>();

    [Header("Enemy Collider Ref (Don't Touch)")]
    [SerializeField] private BoxCollider invulnToggle;

    private float shotTime;
    private float currentVulnerabilityPeriod;
    private int currentShotsCount;

    [Header("Effects")]
    [SerializeField] UnityEvent OnShotFired;

    //TEMP
    [Header("Temporary Visual Reference (Black = Vulnerable)")]
    [SerializeField] private Material invuln;
    [SerializeField] private Material vuln;

    protected override void Start()
    {
        base.Start();
        currentShotsCount = shotsBeforePauseMax;
        currentVulnerabilityPeriod = vulnerabilityPeriodMax;

        invulnToggle.enabled = false;
    }

    protected override void Attacking()
    {
        bullet.GetComponent<EnemyProjectile>().SetDamage(AttackDamage);

        if (Vector3.Distance(transform.position, playerReference.transform.position) < EnemyDetectionRadius)
        {
            transform.LookAt(playerReference.transform.position);

            if (currentShotsCount > 0)
            {
                //attack cooldown
                if (shotTime <= 0)
                {
                    //when firing, aim at player
                    _spawnPoint.LookAt(playerReference.transform.position);

                    //fire projectile
                    GameObject tempBullet = PoolUtility.InstantiateFromPool(_bulletPool, _spawnPoint, bullet);
                    EnemyProjectile tempProjectile = tempBullet.GetComponent<EnemyProjectile>();

                    //set damage
                    tempProjectile.SetDamage(AttackDamage);

                    //set cooldown, invoke
                    shotTime = attackRate;
                    OnShotFired.Invoke();
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
}
