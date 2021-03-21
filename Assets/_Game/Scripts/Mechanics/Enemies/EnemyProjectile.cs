using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : Projectile
{
    private GameObject player = null;
    Vector3 target;

    protected override void Awake()
    {
        base.Awake();
        player = GameManager.player.obj;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        transform.LookAt(player.transform);
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        player?.DamagePlayer(_damage);

        gameObject.SetActive(false);
    }
}
