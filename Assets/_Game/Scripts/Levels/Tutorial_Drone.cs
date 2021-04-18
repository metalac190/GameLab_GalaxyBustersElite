using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Drone : EnemyBase
{
    [SerializeField] Amount_Detection amountDetection = null;
    EntityBase entitybase;
    int amount = 1;

    protected override void Passive()
    {
        transform.LookAt(GameManager.player.obj.transform.position);

        if (Vector3.Distance(transform.position, GameManager.player.obj.transform.position) < EnemyDetectionRadius)
        {
            animator.SetBool("InPlayerRange", true);
        }
    }

    protected override void Attacking()
    { }

    protected override void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            animator.SetTrigger("Collide With Object");
            DialogueTrigger.TriggerEnemyDefeatedDialogue();
            col.gameObject.GetComponent<PlayerController>().DamagePlayer(AttackDamage);
            Dead();
        }
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        if (_currentHealth <= 0)
        {
            amountDetection.GetComponent<Amount_Detection>();
            amountDetection.num_of_enemies -= amount;
        }
    }
}
