using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyBandit : EnemyBase
{
    [Header("Enemy Bandit Burst Shot Amount")]
    [SerializeField] private int burstShotAmt = 0;
    private float currentBurstShots;

    [Header("Enemy Bandit Time Between Burst Shots (Ideal Range Below 1 - Ex: 0.##)")]
    [SerializeField] private float attackRate = 0;
    private float burstTimer;

    [Header("Enemy Bandit Time Until Next Burst (Higher # = Longer Delay)")]
    [SerializeField] private float nextBurstMin = 0;
    [SerializeField] private float nextBurstMax = 0;
    private float nextBurstTime = 0;
    private float nextBurst;

    [Header("Enemy Bandit Bullet Prefab")]
    [SerializeField] private float projectileSpeed;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform _spawnPoint;
    private List<GameObject> _bulletPool = new List<GameObject>();

    [Header("Effects")]
    [SerializeField] UnityEvent OnShotFired;

    //behavior
    protected override void Attacking()
    {
        //player in range
        if (Vector3.Distance(transform.position, GameManager.player.obj.transform.position) < EnemyDetectionRadius)
        {
            animator.SetBool("InPlayerRange", true);

            //constantly looks to player's position, doing it here to make it less choppy
            transform.LookAt(GameManager.player.obj.transform.position);

            if (currentBurstShots > 0)
            {
                //attack cooldown
                if (burstTimer <= 0)
                {
                    animator.SetTrigger("IsFiring");

                    //fire projectile
                    GameObject tempBullet = PoolUtility.InstantiateFromPool(_bulletPool, _spawnPoint, bulletPrefab);
                    EnemyProjectile tempProjectile = tempBullet.GetComponent<EnemyProjectile>();

                    //set damage and speed
                    tempProjectile.SetDamage(AttackDamage);
                    tempProjectile.SetVelocity(projectileSpeed);

                    //set cooldown, invoke
                    burstTimer = attackRate;
                    currentBurstShots -= 1;
                    OnShotFired.Invoke();
                }
                else
                {
                    burstTimer -= Time.deltaTime;
                }
            }
            else
            {
                if (nextBurst <= 0)
                {
                    //adjust RNG attackRate, restrict to 2 decimal places
                    nextBurstTime = Random.Range(nextBurstMin, nextBurstMax);
                    nextBurstTime -= (nextBurstTime % 0.01f);

                    //set cooldown
                    nextBurst = nextBurstTime;
                    currentBurstShots = burstShotAmt;
                }
                else
                {
                    nextBurst -= Time.deltaTime;
                }
            }
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
