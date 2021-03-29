using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyMinion : EnemyBase
{
    [Header("Enemy Minion Attack Rate (Higher # = Longer Shot Delay)")]
    [SerializeField] private float attackRateMin = 0;
    [SerializeField] private float attackRateMax = 0;
    private float attackRate = 0;
    private float shotTime;

    [Header("Enemy Minion Bullet Prefab")]
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform _spawnPoint;
    private List<GameObject> _bulletPool = new List<GameObject>();

    [Header("Effects")]
    [SerializeField] UnityEvent OnShotFired;

    protected override void Start()
    {
        base.Start();
        bullet.GetComponent<EnemyProjectile>().SetDamage(AttackDamage);
    }

    private void OnEnable()
    {
        _currentHealth = maxHealth;
        currentState = EnemyState.Passive;
    }

    //behavior
    protected override void Attacking()
    {
        //player in range
        if (Vector3.Distance(transform.position, GameManager.player.obj.transform.position) < EnemyDetectionRadius)
        {
            //constantly looks to player's position, doing it here to make it less choppy
            transform.LookAt(GameManager.player.obj.transform.position);

            //attack cooldown
            if (shotTime <= 0)
            {
                //fire projectile
                GameObject tempBullet = PoolUtility.InstantiateFromPool(_bulletPool, _spawnPoint, bullet);
                EnemyProjectile tempProjectile = tempBullet.GetComponent<EnemyProjectile>();

                //set damage
                tempProjectile.SetDamage(AttackDamage);

                //adjust RNG attackRate, restrict to 2 decimal places
                attackRate = Random.Range(attackRateMin, attackRateMax);
                attackRate -= (attackRate % 0.01f);

                //set cooldown, invoke
                shotTime = attackRate;
                OnShotFired.Invoke();
            }
            else
            {
                shotTime -= Time.deltaTime;
            }
        }
    }
}
