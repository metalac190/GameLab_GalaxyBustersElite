using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            /// Eventual hookup to deal damage to player

            gameObject.GetComponentInParent<EnemyDrone>().Dead();
        }
        else if (col.gameObject.layer == LayerMask.NameToLayer("Projectile"))
        {
            gameObject.GetComponentInParent<EnemyDrone>().enemyHealth -= 1;

            Debug.Log("Enemy lost 1 health, current health: " + gameObject.GetComponentInParent<EnemyDrone>().enemyHealth);

            if (gameObject.GetComponentInParent<EnemyDrone>().enemyHealth <= 0)
            {
                gameObject.GetComponentInParent<EnemyDrone>().Dead();
            }
        }
    }
}
