using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class BossController : MonoBehaviour
{
    //Refer to Ben Friedman for QA/Bugfixing on Boss System scripts

    //TODO inherit from EnemyBase?
    //triggerVolume to recieve body hits
    private Collider triggerVolume = null;
    private Rigidbody rb = null;

    private BossState nextState = BossState.Idle;  //enum, 0 = idle, 1 = attackAnimation, 2 = movementAnimation, 3 = bloodiedAnimation, ...?

    [Header("Boss Statistics")]
    [SerializeField] private int totalBossHealth = 100;
    [SerializeField] private int totalSegmentHealth = 100;
    [SerializeField] private GameObject[] segmentRefs = new GameObject[13];
    bool segmentsAlive = false;

    [Header("Attack Pattern")]
    [SerializeField] private int mainAttackDamage = 1;
    [SerializeField] private float mainAttackSpeed = 10f;
    [SerializeField] private int minionSpawnRate = 5;
    [SerializeField] private GameObject minionPrefabReference = null;

    private void Awake()
    {
        triggerVolume = GetComponent<Collider>();
        triggerVolume.isTrigger = true;

        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

        //foreach (BossSegment segments in segmentRefs) segment.health = segmentRefs.count/totalsegmenthealth
    }

    private void SetBossState()
    {
        switch (nextState)
        {
            case BossState.Idle:
                //wait some time, then move to next state
                nextState = BossState.Attack;
                break;

            case BossState.Attack:
                GenerateAttack();
                if (segmentsAlive)
                    nextState = BossState.Idle;
                else
                    nextState = BossState.Move;
                break;

            case BossState.Move:
                MovePattern();
                nextState = (BossState)Random.Range(1, 3); //either 1 or 2
                break;

            case BossState.Bloodied:
                BloodiedAnimation();
                nextState = BossState.Move;
                break;

            default:
                Debug.Log("Invalid Boss State");
                break;
        }
    }

    private void BossIdle()
    {
        Debug.Log("Boss is Idle");

        //wait predetermined amount of time
        SetBossState();
    }

    private void OnSegmentDestroyed()
    {
        Debug.Log("Segment Destroyed, Boss Updating");

        bool prevAlive = segmentsAlive;
        segmentsAlive = false;

        foreach (GameObject segment in segmentRefs)
        {
            if (segment.activeInHierarchy)
                segmentsAlive = true;
        }

        if (prevAlive)
        {
            //first time segments goes from True to False, play bloodied animation
            if (!segmentsAlive)
                nextState = BossState.Bloodied;
        }
    }

    private void BloodiedAnimation()
    {
        Debug.Log("Boss is Bloodied");
        SetBossState();
    }

    private void MovePattern()
    {
        //zig-zag
        Debug.Log("Boss is Moving");

        //wait to arrive at next position
        SetBossState();
    }

    private void GenerateAttack()
    {
        BossAttacks randomAttack;

        if (segmentsAlive)
            randomAttack = (BossAttacks)Random.Range(0, 4);
        else
            randomAttack = (BossAttacks)Random.Range(0, 2);

        switch (randomAttack)
        {
            case BossAttacks.RingAttack:
                RingAttack();
                break;
            case BossAttacks.MisisleAttack:
                MissileAttack();
                break;
            case BossAttacks.LaserAttack:
                LaserAttack();
                break;
            case BossAttacks.SummonMinions:
                SummonMinions();
                break;
            default:
                Debug.Log("Invalid Normal Attack called");
                break;
        }

        //return data from each Attack, wait for delay?
        SetBossState();
    }

    private void RingAttack()
    {
        if (segmentsAlive)
        {
            Debug.Log("Firing the normal Ring Attack");
        }
        else 
        {
            Debug.Log("Firing bloodied Ring Attack");
        }
    }

    private void MissileAttack()
    {
        if (segmentsAlive)
        {
            Debug.Log("Triggering the normal Missile Attack");
        }
        else
        {
            Debug.Log("Triggering bloodied Missile Attack");
        }
    }

    private void LaserAttack()
    {
        Debug.Log("Activating the Laser");
    }

    private void SummonMinions()
    {
        Debug.Log("Summoning Minions");
    }
}
