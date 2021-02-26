using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class BossController : MonoBehaviour
{
    //Refer to Ben Friedman for QA/Bugfixing on Boss System scripts
    
    public IntEvent Attacked;

    //TODO inherit from EnemyBase?

    //triggerVolume to recieve body hits
    private Collider _triggerVolume = null;
    private Rigidbody _rb = null;

    private BossState _nextState = BossState.Idle;  //enum, 0 = idle, 1 = attackAnimation, 2 = movementAnimation, 3 = bloodiedAnimation, ...?
    private float _wait = 0f;    //time between state being started, and cue for next state queued

    [Header("Boss Statistics")]
    [SerializeField] private int _totalHealth = 100;
    [SerializeField] private int _segmentHealth = 100;
    [SerializeField] private BossSegmentController[] _segmentRefs = new BossSegmentController[0];
    bool _areSegmentsAlive = true;

    [Header("Attack Pattern")]
    [SerializeField] private int _attackDamage = 1;
    [SerializeField] private float _attackAnimTime = 10f;
    [SerializeField] private int _minionWaveSize = 5;
    [SerializeField] private GameObject _minionRef = null;

    [Header("Boss Settings")]
    [SerializeField] private float _idleTime = 100f;

    private void Awake()
    {
        _triggerVolume = GetComponent<Collider>();
        _triggerVolume.isTrigger = true;

        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = false;

        foreach (BossSegmentController segment in _segmentRefs)
        {
            //health per segment
            segment.SetHealth(_segmentHealth);
        }
    }

    #region Listeners
    private void OnEnable()
    {
        foreach (BossSegmentController segment in _segmentRefs)
        {
            segment.Died.AddListener(OnSegmentDestroyed);
        }
    }

    private void OnDisable()
    {
        foreach (BossSegmentController segment in _segmentRefs)
        {
            segment.Died.RemoveListener(OnSegmentDestroyed);
        }
    }
    #endregion

    private void Update()
    {
        if (_wait > 0)
        {
            _wait--;
            if (_wait <= 0)
            {
                NextBossState();
            }
        }
    }

    //for dev-testing purposes, remove before playtest/final
    public void SetBossState(BossState state)
    {
        _wait = 0;
        _nextState = state;
        NextBossState();
    }

    private void NextBossState()
    {
        switch (_nextState)
        {
            case BossState.Idle:
                BossIdle();
                _nextState = BossState.Attack;
                break;

            case BossState.Attack:
                GenerateAttack();
                if (_areSegmentsAlive)
                    _nextState = BossState.Idle;
                else
                    _nextState = BossState.Move;
                break;

            case BossState.Move:
                MovePattern();
                _nextState = (BossState)Random.Range(1, 3); //either 1 or 2
                break;

            case BossState.Bloodied:
                Bloodied();
                _nextState = BossState.Move;
                break;

            default:
                Debug.Log("Invalid Boss State");
                break;
        }
    }

    //interjects behavior loop with check for bloodied state
    private void OnSegmentDestroyed()
    {
        Debug.Log("Segment Destroyed, Boss Updating");

        bool prevAlive = _areSegmentsAlive;
        _areSegmentsAlive = false;

        foreach (BossSegmentController segment in _segmentRefs)
        {
            if (segment.isActiveAndEnabled)
                _areSegmentsAlive = true;
        }

        if (prevAlive)
        {
            //first time segments goes from True to False, play bloodied animation
            if (!_areSegmentsAlive)
                _nextState = BossState.Bloodied;
        }
    }

    private void BossIdle()
    {
        Debug.Log("Boss is Idle");

        //wait predetermined amount of time
        _wait = _idleTime;
    }

    private void Bloodied()
    {
        Debug.Log("Boss is Bloodied");
        //play animation
        //set invulnerable

        //wait for animation to end
        _wait = _idleTime;
    }

    private void MovePattern()
    {
        Debug.Log("Boss is Moving");
        //zig-zag

        //wait to arrive at next position
        _wait = _idleTime;
    }

    private void GenerateAttack()
    {
        BossAttacks randomAttack;

        if (_areSegmentsAlive)
            randomAttack = (BossAttacks)Random.Range(0, 4);
        else
            randomAttack = (BossAttacks)Random.Range(0, 2);

        Attacked.Invoke((int)randomAttack);

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
    }

    private void RingAttack()
    {
        if (_areSegmentsAlive)
        {
            Debug.Log("Firing the normal Ring Attack");
            _wait = _attackAnimTime;
        }
        else 
        {
            Debug.Log("Firing bloodied Ring Attack");
            _wait = _attackAnimTime;
        }
    }

    private void MissileAttack()
    {
        if (_areSegmentsAlive)
        {
            Debug.Log("Triggering the normal Missile Attack");
            _wait = _attackAnimTime;
        }
        else
        {
            Debug.Log("Triggering bloodied Missile Attack");
            _wait = _attackAnimTime;
        }
    }

    private void LaserAttack()
    {
        Debug.Log("Activating the Laser");
        _wait = _attackAnimTime;

    }

    private void SummonMinions()
    {
        Debug.Log("Summoning Minions");
        _wait = _attackAnimTime;
    }
}
