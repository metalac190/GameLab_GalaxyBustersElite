using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyDrone : EnemyBase
{
    private GameObject playerReference = null;

    [Header("Effects")]
    [SerializeField] UnityEvent OnHit; // Not ever invoked since enemy dies in one hit
    [SerializeField] UnityEvent OnDead;

    private void Start()
    {
        playerReference = GameManager.player.obj;
    }

    protected override void Passive() { }

    protected override void Attacking() { }

    protected override void Dead()
    {
        Debug.Log("Enemy destroyed");

        OnDead.Invoke();

        Destroy(transform.parent.gameObject);
    }
}
