using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyDrone : EnemyBase
{
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
}
