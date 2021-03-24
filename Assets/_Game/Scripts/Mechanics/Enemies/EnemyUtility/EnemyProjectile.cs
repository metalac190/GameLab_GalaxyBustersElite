using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : Projectile
{
    protected override void OnEnable()
    {
        base.OnEnable();
        transform.LookAt(GameManager.player.obj.transform);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.gameObject.GetComponent<PlayerController>();
        
        if (player != null)
        {
            player.DamagePlayer(_damage);
            gameObject.SetActive(false);
        }
    }
}
