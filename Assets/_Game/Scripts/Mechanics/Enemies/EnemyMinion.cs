﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyMinion : EnemyBase
{
    private GameObject playerReference = null;

    [Header("Enemy Minion Attack Rate (Higher # = Longer Shot Delay)")]
    [SerializeField] private float attackRate = 0;

    [Header("Enemy Minion Bullet Prefab")]
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform _spawnPoint;
    private List<GameObject> _bulletPool = new List<GameObject>();

    private float shotTime;

    [Header("Effects")]
    [SerializeField] UnityEvent OnShotFired;

    private void Start()
    {
        playerReference = GameManager.player.obj;
        bullet.GetComponent<EnemyProjectile>().SetDamage(AttackDamage);
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
    }

    public override void Dead()
    {
        Debug.Log("Enemy destroyed");

        if (givesPlayerMS)
            camRailManager.IncreaseCamRailSpeed();

        Destroy(transform.parent.gameObject);
    }
}
