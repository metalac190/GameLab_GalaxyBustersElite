using System.Collections;
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
    private List<GameObject> bulletPool = new List<GameObject>();

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

    protected override void Attacking()
    {
        if (Vector3.Distance(transform.position, playerReference.transform.position) < EnemyDetectionRadius)
        {
            transform.LookAt(playerReference.transform.position);

            if (shotTime <= 0)
            {
                shotTime = attackRate;
                PoolUtility.InstantiateFromPool(bulletPool, transform, bullet);

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
        Destroy(transform.parent.gameObject);
    }
}
