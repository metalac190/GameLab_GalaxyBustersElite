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
    [SerializeField] private float projectileSpeed;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform _spawnPoint;
    private Queue<GameObject> _bulletQueue = new Queue<GameObject>();

    [Header("Effects")]
    [SerializeField] UnityEvent OnShotFired;

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
            animator.SetBool("InPlayerRange", true);

            //constantly looks to player's position, doing it here to make it less choppy
            transform.LookAt(GameManager.player.obj.transform.position);

            //attack cooldown
            if (shotTime <= 0)
            {
                animator.SetTrigger("isFiring");

                //fire projectile
                GameObject tempBullet = PoolUtility.InstantiateFromQueue(_bulletQueue, _spawnPoint.transform.position, transform.rotation, bullet);
                EnemyProjectile tempProjectile = tempBullet.GetComponent<EnemyProjectile>();

                //set damage and speed
                tempProjectile.SetDamage(AttackDamage);
                tempProjectile.SetVelocity(projectileSpeed);

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
        else
        {
            animator.SetBool("InPlayerRange", false);
        }
    }
}
