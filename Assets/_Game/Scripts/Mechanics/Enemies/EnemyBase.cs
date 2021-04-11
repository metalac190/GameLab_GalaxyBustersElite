using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : EntityBase
{
    public enum EnemyState { Passive, Attacking }

    [Header("Enemy State")]
    public EnemyState currentState;

    [Header("Additional Enemy Settings")]
	[SerializeField] private string id;
	public string EnemyID { get { return id; } }

	[SerializeField] private int attackDamage = 0;
	public int AttackDamage { get { return attackDamage; } }

	[SerializeField] private float enemyDetectionRadius = 0;
    public float EnemyDetectionRadius { get { return enemyDetectionRadius; } }

    [SerializeField] private int enemyScore = 0;
    private float cdInvuln;

    [SerializeField] protected bool givesPlayerMS;
    protected CamRailManager camRailManager;

    protected Animator animator;

    protected virtual void Awake()
    {
        camRailManager = FindObjectOfType<CamRailManager>();
    }

    protected override void Start()
    {
        base.Start();
        animator = GetComponentInChildren<Animator>();
        currentState = EnemyState.Passive;
    }

    private void FixedUpdate()
    {
        UpdateState();
    }

    protected virtual void UpdateState()
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

    protected virtual void Passive()
    {
        if (Vector3.Distance(transform.position, GameManager.player.obj.transform.position) < EnemyDetectionRadius)
        {
            transform.LookAt(GameManager.player.obj.transform.position);

            currentState = EnemyState.Attacking;
        }
    }

    protected abstract void Attacking();

    public virtual void Dead()
    {
        if (givesPlayerMS)
            camRailManager.IncreaseCamRailSpeed();

        transform.parent.gameObject.SetActive(false);
    }

    public override void TakeDamage(float damage)
    {
		// Prevent enemies from taking damage multiple times in the same frame
		if (Time.time - cdInvuln > 0.01f)
		{
			_currentHealth -= damage;
			if (_currentHealth <= 0)
			{
				DialogueTrigger.TriggerEnemyDefeatedDialogue();
				Died.Invoke();
				Dead();
				ScoreSystem.IncreaseCombo();
				ScoreSystem.IncreaseScore(id, enemyScore);
				ScoreSystem.DestroyedEnemyType(id);
				//disable or destroy as needed?
			}
			else
			{
                cdInvuln = Time.time;

                // Done here instead of overriding in each child class
                if (GetComponent<EnemyDrone>())
                {
                    animator.SetTrigger("DamageTaken");
                }
                else if (GetComponent<EnemyMinion>())
                {
                    animator.SetTrigger("Damaged");
                }
                else if (GetComponent<EnemyRammer>())
                {
                    animator.SetTrigger("DamageTaken");
                }

                Damaged.Invoke();
				//set up FX + AnimationController from Inspector, using Event
			}
		}
    }

    protected virtual void OnTriggerEnter(Collider col)
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
