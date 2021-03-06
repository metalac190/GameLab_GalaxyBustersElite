﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class EnemyRammer : EnemyBase
{
    [Header("Enemy Rammer Charge Speed")]
    [SerializeField] private float ramSpeed = 0;

    [Header("Enemy Rammer Collide Event")]
    [SerializeField] UnityEvent OnRamCollide;

    [Header("Enemy Rammer Movement Fix - Don't Touch")]
    [SerializeField] UnityEvent OnRamAttackEnter;
    [SerializeField] UnityEvent OnRamAttackExit;

    protected override void Passive()
    {
        if (Vector3.Distance(transform.position, GameManager.player.obj.transform.position) < EnemyDetectionRadius)
        {
            animator.SetBool("InPlayerRange", true);

            transform.LookAt(GameManager.player.obj.transform.position);

            //when changing state, enable heat seeking behavior
            currentState = EnemyState.Attacking;

            HeatSeeker seeker = GetComponent<HeatSeeker>();
            seeker?.StartFollowing();
            seeker?.SetRotationSpeed(20f);

            OnRamAttackEnter.Invoke();
        }
    }

    protected override void Attacking()
    {
        if (Vector3.Distance(transform.position, GameManager.player.obj.transform.position) < EnemyDetectionRadius)
        {
            //behavior moved to HeatSeeker behavior
            animator.SetTrigger("WindupDistance");
            GetComponent<Rigidbody>().velocity = transform.forward * ramSpeed;
        }
        else if (Vector3.Distance(transform.position, GameManager.player.obj.transform.position) > EnemyDetectionRadius)
        {
            OnRamAttackExit.Invoke();
        }
    }

    protected override void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            OnRamCollide.Invoke();

            DialogueTrigger.TriggerEnemyDefeatedDialogue();
            col.gameObject.GetComponent<PlayerController>().DamagePlayer(AttackDamage);
            Dead();
        }
    }
}
