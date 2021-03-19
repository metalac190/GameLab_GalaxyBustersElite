using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : EntityBase
{
    public enum EnemyState { Passive, Attacking }

    [Header("Enemy State")]
    public EnemyState currentState;

    [Header("Additional Enemy Settings")]
    [SerializeField] private int attackDamage = 0;
    public int AttackDamage { get { return attackDamage; } }

    [SerializeField] private float enemyDetectionRadius = 0;
    public float EnemyDetectionRadius { get { return enemyDetectionRadius; } }

    void Start()
    {
        currentState = EnemyState.Passive;
    }

    protected virtual void UpdateState()
    {
        switch (currentState)
        {
            case EnemyState.Passive:
                break;
            case EnemyState.Attacking:
                break;
            default:
                break;
        }
    }

    protected abstract void Passive();

    protected abstract void Attacking();

    public abstract void Dead();

    public override void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            DialogueTrigger.TriggerEnemyDefeatedDialogue();
            Died.Invoke();
            Dead();
            //disable or destroy as needed?
        }
        else
        {
            Damaged.Invoke();
            //set up FX + AnimationController from Inspector, using Event
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            DialogueTrigger.TriggerEnemyDefeatedDialogue();
            col.gameObject.GetComponent<PlayerController>().DamagePlayer(AttackDamage);
            Dead();
        }
    }

#if UNITY_EDITOR
    /// Visual radius of enemy detection radius if enemy selected in editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, EnemyDetectionRadius);
    }
#endif
}
