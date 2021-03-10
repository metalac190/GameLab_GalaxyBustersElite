using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMinion : EnemyBase
{
    private GameObject playerReference = null;

    [Header("Enemy Minion Bullet Prefab")]
    [SerializeField] private GameObject bullet;

    private float shotTime;

    private void Start()
    {
        playerReference = GameManager.player.obj;
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
            Debug.Log("Detect range reached");
            currentState = EnemyState.Attacking;
        }
    }

    protected override void Attacking()
    {
        bullet.GetComponent<EnemyProjectile>().SetDamage(AttackDamage);

        if (shotTime <= 0)
        {
            shotTime = AttackRate;
            Instantiate(bullet, transform.position, Quaternion.identity);
        }
        else
        {
            shotTime -= Time.deltaTime;
        }
    }

    protected override void Dead()
    {
        Debug.Log("Enemy destroyed");
        Destroy(gameObject);
    }
}
