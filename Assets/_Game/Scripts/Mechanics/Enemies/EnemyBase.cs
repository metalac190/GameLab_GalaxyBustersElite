using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : EntityBase
{
    public enum EnemyState { Arrival, Passive, Attacking, Dead, Flee }

    [Header("Enemy State")]
    public EnemyState currentState;

    [Header("Additional Enemy Settings")]
    [SerializeField] private int attackDamage = 0;
    public int AttackDamage { get { return attackDamage; } }

    [SerializeField] private float attackRate = 0;
    public float AttackRate { get { return attackRate; } }

    [SerializeField] private float enemyMoveSpeed = 0;
    public float EnemyMoveSpeed { get { return enemyMoveSpeed; } }

    [SerializeField] private float enemyDetectionRadius = 0;
    public float EnemyDetectionRadius { get { return enemyDetectionRadius; } }

    [SerializeField] private GameObject bullet;

    float shotTime;

    void Start()
    {
        currentState = EnemyState.Arrival;
    }

    protected virtual void UpdateState()
    {
        switch (currentState)
        {
            case EnemyState.Arrival:
                break;
            case EnemyState.Passive:
                break;
            case EnemyState.Attacking:
                break;
            case EnemyState.Dead:
                break;
            case EnemyState.Flee:
                break;
            default:
                break;
        }
    }

    protected abstract void Arrival();

    protected abstract void Passive();

    protected abstract void Attacking();

    protected abstract void Dead();

    protected abstract void Flee();

    public override void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            Died.Invoke();
            //disable or destroy as needed?
        }
        else
        {
            Damaged.Invoke();
            //set up FX + AnimationController from Inspector, using Event
        }
    }

    protected void FireProjectile()
    {
        if (shotTime <= 0)
        {
            shotTime = AttackRate;
            Instantiate(bullet, transform.position, Quaternion.identity);
            bullet.GetComponent<EnemyProjectile>().SetDamage(AttackDamage);
        }
        else
        {
            shotTime -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            TakeDamage(bullet.GetComponent<EnemyProjectile>().projDamage);
        }
    }

#if UNITY_EDITOR
    /// Visual radius of enemy detection radius if enemy selected in editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(transform.position, EnemyDetectionRadius);
    }
#endif
}
