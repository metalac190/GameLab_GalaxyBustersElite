using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class BossController : MonoBehaviour
{
    //Refer to Ben Friedman for QA/Bugfixing on Boss System scripts

    //TODO inherit from EnemyBase?
    
    public IntEvent Attacking;

    //triggerVolume to recieve body hits
    private Collider _triggerVolume = null;
    private Rigidbody _rb = null;

    private BossState _nextState = BossState.Idle;  //enum, 0 = idle, 1 = attackAnimation, 2 = movementAnimation, 3 = bloodiedAnimation, ...?
    private float _wait = 0f;    //time between state being started, and cue for next state queued

    [Header("Boss Statistics")]

    [Tooltip("Starting Health for Boss Core\n(Second Phase of fight).")]
    [SerializeField] private int _totalHealth = 100;
    [Tooltip("Starting Health for each Segment.\nTotal Segment Health derived from\nStarting Health * number of Segments")]
    [SerializeField] private int _segmentHealth = 100;
    private BossSegmentController[] _segmentRefs = new BossSegmentController[0];
    bool _isBloodied = true;

    [Header("Boss Settings")]

    [Tooltip("Time in Seconds to wait during Idle state.")]
    [SerializeField] private float _idleTime = 100f;
    [Tooltip("Standardized amount of damage to apply to Player.")]
    [SerializeField] private int _attackDamage = 1;
    [Tooltip("Placeholder.\nAmount of time in Seconds per attack.\nDependant on type of attack chosen.")]
    [SerializeField] private float _attackAnimTime = 10f;
    [Tooltip("Number of Minions to spawn during behavior loop.")]
    [SerializeField] private int _minionWaveSize = 5;
    private List<GameObject> _minionWaveRef = new List<GameObject>();

    [Header("Asset References. Do Not Touch.")]

    [Tooltip("Reference to Minion Prefab.")]
    [SerializeField] private GameObject _minionRef = null;
    [Tooltip("Reference to Ring Attack Prefab.")]
    [SerializeField] private GameObject _ringRef = null;
    [Tooltip("Reference to Game Object where Ring Attack originates from.")]
    [SerializeField] private Transform _ringSpawn = null;
    private List<GameObject> _ringPool = new List<GameObject>();

    private void Awake()
    {
        _triggerVolume = GetComponent<Collider>();
        _triggerVolume.isTrigger = true;

        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = false;

        _segmentRefs = GetComponentsInChildren<BossSegmentController>();
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
            _wait -= Time.deltaTime;
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

                if (_isBloodied)
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

    /// <summary> Interjects behavior loop with check for bloodied state
    /// <para> Called when Segments Destroyed is invoked. </para>
    /// </summary>
    private void OnSegmentDestroyed()
    {
        Debug.Log("Segment Destroyed, Boss Updating");
        
        //If current or previous check returned any alive segments
        if (_isBloodied)
        {
            //Set to False, and enable if any are still alive
            _isBloodied = false;
            foreach (BossSegmentController segment in _segmentRefs)
            {
                if (segment.isActiveAndEnabled)
                    _isBloodied = true;
            }

            //If previous check returned alive, but now check returns false, call Bloodied state
            if (!_isBloodied)
            {
                if (!_isBloodied)
                    _nextState = BossState.Bloodied;
            }
        }
        //Function should not be able to be called once all segments are destroyed, but if state is false, can called again, nothing happens
        //Protection from false negative during gameplay, still only runs function once even if segements are alive and accidentally triggers Bloodied.
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
            //dependant on EnemyBase implementation

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

        if (_isBloodied)
            randomAttack = (BossAttacks)Random.Range(0, 4);
        else
            randomAttack = (BossAttacks)Random.Range(0, 2);

        //Signals to Segments which Attack is active, to animate/behave accordingly
        Attacking.Invoke((int)randomAttack);

        //Calls Boss animations/behaviors
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
        //RingAttack behavior dependant on Bloodied state
        if (_isBloodied)
        {
            //Single Ring Attack
            Debug.Log("Firing the normal Ring Attack");
            PoolUtility.InstantiateFromPool(_ringPool, _ringSpawn, _ringRef);

            //put Boss Animation here.
            //calculate wait time, defined by Animation
            _wait = _attackAnimTime;
        }
        else 
        {
            //Up to 3 Rings?
            Debug.Log("Firing bloodied Ring Attack");
            _wait = _attackAnimTime;
        }
    }

    private void MissileAttack()
    {
        //Dependant on Bloodied state
        if (_isBloodied)
        {
            //Segments each fire a missile to track player
            Debug.Log("Triggering the normal Missile Attack");

            //put Boss Animation here.
            //calculate wait time, defined by Animation
            _wait = _attackAnimTime;
        }
        else
        {
            //Up to 3 Missiles? from core to track player
            Debug.Log("Triggering bloodied Missile Attack");

            

            //put Boss Animation here.
            //calculate wait time, defined by Animation
            _wait = _attackAnimTime;
        }
    }

    private void LaserAttack()
    {
        //Beam that follows Player position
        Debug.Log("Activating the Laser");
        _wait = _attackAnimTime;

    }

    /// <summary> Summons a Wave of Enemy Minions
    /// <para> Minions defined by _minionRef, and wave size defined by _minionWaveSize </para>
    /// </summary>
    private void SummonMinions()
    {
        //Dependant if Minions are active
        //Refills current wave, stacks multiple waves if earlier waves are not defeated
        //Summons up to 5 Minions?
        Debug.Log("Summoning Minions");

        int waveCount = 0;
        foreach (GameObject minion in _minionWaveRef)
        {
            if (minion.activeInHierarchy)
                waveCount++;
        }

        for (int i = waveCount; i < _minionWaveSize; i++)
        {
            //reliant on Minions being Disabled when killed, and not Destroyed()
            PoolUtility.InstantiateFromPool(_minionWaveRef, _ringSpawn, _minionRef);
        }
        
        //put Boss Animation here.
        //calculate wait time, defined by Animation
        _wait = _attackAnimTime;
    }
}
