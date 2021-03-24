using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyBandit : EnemyBase
{
    [Header("Enemy Bandit Attack Rate (Higher # = Longer Shot Delay)")]
    [SerializeField] private float attackRate = 0;

    [Header("Enemy Bandit Bullet Prefab")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform _spawnPoint;
    private List<GameObject> _bulletPool = new List<GameObject>();

    private float shotTime;

    [Header("Effects")]
    [SerializeField] UnityEvent OnShotFired;

    //behavior
    protected override void Attacking()
    {
        //player in range
        if (Vector3.Distance(transform.position, playerReference.transform.position) < EnemyDetectionRadius)
        {
            //attack cooldown
            if (shotTime <= 0)
            {
                //when firing, aim at player
                _spawnPoint.LookAt(playerReference.transform.position);

                //fire projectile
                GameObject tempBullet = PoolUtility.InstantiateFromPool(_bulletPool, _spawnPoint, bulletPrefab);
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
    }

    public override void Dead()
    {
        Debug.Log("Enemy destroyed");

        if (givesPlayerMS)
            camRailManager.IncreaseCamRailSpeed();

        Destroy(transform.parent.gameObject);
    }
}
