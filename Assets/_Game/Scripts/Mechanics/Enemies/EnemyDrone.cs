using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyDrone : EnemyBase
{
    private GameObject playerReference = null;

    CamRailManager camRailManager;

    private void Start()
    {
        camRailManager = FindObjectOfType<CamRailManager>();

        playerReference = GameManager.player.obj;
    }

    private void FixedUpdate()
    {
        Passive();
    }

    protected override void Passive()
    {
        transform.LookAt(playerReference.transform.position);
    }

    protected override void Attacking() { }

    public override void Dead()
    {
        Debug.Log("Enemy destroyed");
        camRailManager.IncreaseCamRailSpeed();

        Destroy(transform.parent.gameObject);
    }
}
