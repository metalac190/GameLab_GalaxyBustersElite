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

    protected override void OnCollisionEnter(Collision collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        player?.DamagePlayer(_damage);

        gameObject.SetActive(false);
    }
}
