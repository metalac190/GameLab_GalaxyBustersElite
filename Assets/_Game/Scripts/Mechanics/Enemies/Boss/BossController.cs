﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class BossController : EntityBase
{
    //Refer to Ben Friedman for QA/Bugfixing on Boss System scripts

    //TODO inherit from EnemyBase?
    
    [Header("Boss Settings")]
    public IntEvent Attacking;

    //triggerVolume to recieve body hits
    private Collider _triggerVolume = null;
    private Rigidbody _rb = null;

    private BossState _nextState = BossState.Idle;

    [Header("Boss Statistics")]

    [SerializeField] private float _moveSpeed = 10f;
    [Tooltip("Number of times to Move when Bloodied,\nInclusive Min, Exclusive Max")]
    [SerializeField] private Vector2 _numberOfMoves = new Vector2(1, 3);

    [Tooltip("Starting Health for each Segment.\nTotal Segment Health derived from\nStarting Health * number of Segments")]
    [SerializeField] private int _segmentHealth = 100;
    private BossSegmentController[] _segmentRefs = new BossSegmentController[0];
    bool _segmentsAlive = true;

    [Tooltip("Time in Seconds to wait during Idle state.")]
    [SerializeField] private float _idleTime = 2f;
    [Tooltip("Placeholder.\nAmount of time in Seconds per attack.\nDependant on type of attack chosen.")]
    [SerializeField] private float _attackAnimTime = 2f;

    [Header("Attack Settings")]

    [Tooltip("Standardized amount of damage to apply to Player.")]
    [SerializeField] private int _attackDamage = 1;
    [Tooltip("Number of Minions to spawn during behavior loop.")]
    [SerializeField] private int _minionWaveSize = 5;
    [Tooltip("Number of Missiles to spawn during Bloodied Missile Attack")]
    [SerializeField] private int _bloodiedProjectileCount = 3;

    [Tooltip("The Laser's movespeed as a percentage of the Player's movespeed")]
    [SerializeField] private float _laserSpeedModifier = 0.8f;
    [Tooltip("For Testing Purposes,\nRemove when FX implemented")]
    [SerializeField] private GameObject _laserTracker = null;

    [Tooltip("Time in Seconds for Laser animation to Warm Up\nBefore dealing damage.")]
    [SerializeField] private float _laserWarmUpTime = 2f;
    private Vector3 _laserEndPoint = Vector3.zero;
    private bool _isLaser = false;
    private BossState _prevNext;

    [Header("Asset References! Do Not Touch!")]

    [Tooltip("Reference to Minion Prefab.")]
    [SerializeField] private GameObject _minionRef = null;
    private List<GameObject> _minionWaveRef = new List<GameObject>();

    [Tooltip("Reference to Ring Attack Prefab.")]
    [SerializeField] private GameObject _ringRef = null;
    private List<GameObject> _ringPool = new List<GameObject>();

    [Tooltip("Reference to Bloodied Boss Missile Prefab")]
    [SerializeField] private GameObject _missileRef = null;
    private List<GameObject> _missilePool = new List<GameObject>();

    [Tooltip("Reference to Game Object where Ring Attack originates from.")]
    [SerializeField] private Transform _projectileSpawn = null;

    private Coroutine _BossBehavior = null;
    

    private void Awake()
    {
        _triggerVolume = GetComponent<Collider>();
        _triggerVolume.isTrigger = true;

        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = false;

        _segmentRefs = GetComponentsInChildren<BossSegmentController>();
        for (int i=0; i < _segmentRefs.Length; i++)
        {
            _segmentRefs[i].SetHealth(_segmentHealth);
            _segmentRefs[i].SetDelay(i * 0.1f);
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
    
    /// <summary> 
    ///     Returns Boss's current health, plus all active Segments' health
    ///
    /// </summary>
    public int TotalHealth
    {
        get
        {
            int value = 0;
            foreach (BossSegmentController segment in _segmentRefs)
            {
                if (segment.isActiveAndEnabled)
                    value += segment.Health;
            }

            return value + _currentHealth;
        }
    }

    //for testing purposes. TODO: Remove
    public void SetBossState(BossState state)
    {
        _nextState = state;
        NextBossState();
    }

    //for testing purposes
    public void TestBlooded()
    {
        _segmentsAlive = false;
        SetBossState(BossState.Move);
    }

    /// <summary> 
    ///     Interjects behavior loop with a check for Bloodied State
    /// <para> 
    ///     Called primarilly by BossSegmentController.Destroyed.Invoke()
    /// </para>
    /// </summary>
    private void OnSegmentDestroyed()
    {
        Debug.Log("Segment Destroyed, Boss Updating");

        //If current or previous check returned any alive segments
        if (_segmentsAlive)
        {
            //Set to False, and enable if any are still alive
            _segmentsAlive = false;
            foreach (BossSegmentController segment in _segmentRefs)
            {
                if (segment.isActiveAndEnabled)
                    _segmentsAlive = true;
            }

            //If previous check returned alive, but now check returns false, call Bloodied state
            if (!_segmentsAlive)
            {
                if (!_segmentsAlive)
                    _nextState = BossState.Bloodied;
            }
        }
        //Function should not be able to be called once all segments are destroyed, but if state is false, can called again, nothing happens
        //Protection from false negative during gameplay, still only runs function once even if segements are alive and accidentally triggers Bloodied.
    }

    private void NextBossState()
    {
        //disable laser tracking, after moving away from Laser Attack state
        //somewhat repetitive when not using Laser Attack state? better place for this?
        _isLaser = false;
        _laserTracker.SetActive(false);

        switch (_nextState)
        {
            case BossState.Idle:

                _nextState = BossState.Attack;

                _BossBehavior = StartCoroutine(BossIdle());
                break;

            case BossState.Attack:

                if (_segmentsAlive)
                    _nextState = BossState.Idle;
                else
                    _nextState = BossState.Move;

                GenerateAttack();
                break;

            case BossState.Move:

                _nextState = BossState.Attack;

                int numberOfMoves = (int)Random.Range(_numberOfMoves.x, _numberOfMoves.y);
                _BossBehavior = StartCoroutine(MovePattern(numberOfMoves));
                break;

            case BossState.Bloodied:

                _nextState = BossState.Move;

                _BossBehavior = StartCoroutine(Bloodied());
                break;

            default:
                Debug.Log("Invalid Boss State");
                break;
        }
    }

    private IEnumerator BossIdle()
    {
        Debug.Log("Boss is Idle");

        //wait predetermined amount of time
        OnSegmentDestroyed();
        yield return new WaitForSeconds(_idleTime);

        NextBossState();
    }

    private IEnumerator Bloodied()
    {
        Debug.Log("Boss is Bloodied");
        //play animation
        //set invulnerable
        //dependant on EnemyBase implementation

        //wait for animation to end
        yield return new WaitForSeconds(_idleTime);

        NextBossState();
    }

    private IEnumerator MovePattern(int count)
    {
        Debug.Log("Boss is Moving");

        //signal when movement is done
        if (count <= 0)
        {
            NextBossState();
            yield return null;
        }
        else
        {
            //identifies points on X/Y plane, at Z distance from player
            Vector3 point = new Vector3(Random.Range(0f, 10f), Random.Range(0f, 10f), transform.position.z);

            //moveTowards those points, at speed
            while (transform.position != point)
            {
                //will eventually perfectly equal Point, due to MoveTowards()?
                transform.position = Vector3.MoveTowards(transform.position, point, _moveSpeed);
                yield return new WaitForEndOfFrame();
            }

            //wait at point, for (small)time
            yield return new WaitForSeconds(_idleTime * 0.25f);

            //repeat
            StartCoroutine(MovePattern(count - 1));
        }
    }

    private void GenerateAttack()
    {
        BossAttacks randomAttack;

        if (_segmentsAlive)
            randomAttack = (BossAttacks)Random.Range(0, 4);
        else
            randomAttack = (BossAttacks)Random.Range(0, 2);

        //Signals to Segments which Attack is active, to animate/behave accordingly
        //public facing, accessible by animators, fx, other systems?
        Attacking.Invoke((int)randomAttack);

        //Calls Boss animations/behaviors
        switch (randomAttack)
        {
            case BossAttacks.RingAttack:
                _BossBehavior = StartCoroutine(RingAttack());
                break;
            case BossAttacks.MisisleAttack:
                _BossBehavior = StartCoroutine(MissileAttack());
                break;
            case BossAttacks.LaserAttack:
                _BossBehavior = StartCoroutine(LaserAttack());
                break;
            case BossAttacks.SummonMinions:
                _BossBehavior = StartCoroutine(SummonMinions());
                break;
            default:
                Debug.Log("Invalid Normal Attack called");
                break;
        }
    }

    private IEnumerator RingAttack()
    {
        _projectileSpawn.LookAt(GameManager.player.obj.transform);

        //RingAttack behavior dependant on Bloodied state
        if (_segmentsAlive)
        {
            //Single Ring Attack
            Debug.Log("Firing the normal Ring Attack");
            PoolUtility.InstantiateFromPool(_ringPool, _projectileSpawn, _ringRef);

            //put Boss Animation here.
            //or have BossAnimator listen to IntEvent Attacked
            //calculate wait time, defined by Animation
            yield return new WaitForSeconds(_attackAnimTime);
        }
        else 
        {
            //TODO
            //Up to 3 Rings?
            Debug.Log("Firing bloodied Ring Attack");
            for (int i=0; i < _bloodiedProjectileCount; i++)
            {
                PoolUtility.InstantiateFromPool(_ringPool, _projectileSpawn, _ringRef);
                yield return new WaitForSeconds(0.2f);
            }
                

            yield return new WaitForSeconds(_attackAnimTime);
        }
        NextBossState();
    }

    private IEnumerator MissileAttack()
    {
        _projectileSpawn.LookAt(GameManager.player.obj.transform);

        //Dependant on Bloodied state
        if (_segmentsAlive)
        {
            //Segments each fire a missile to track player
            Debug.Log("Triggering the normal Missile Attack");

            //functionality driven by BossSegmentController
        }
        else
        {
            Debug.Log("Triggering bloodied Missile Attack");

            //amount of missiles determined by Designer
            for (int i = 0; i < _bloodiedProjectileCount; i++)
            {
                yield return new WaitForSeconds(0.2f);
                PoolUtility.InstantiateFromPool(_missilePool, _projectileSpawn, _missileRef);
            }    
        }

        //put Boss Animation here.
        //or have BossAnimator listen to IntEvent Attacked
        //calculate wait time, defined by Animation
        yield return new WaitForSeconds(_attackAnimTime);

        NextBossState();
    }

    private IEnumerator LaserAttack()
    {
        //Beam that follows Player position
        Debug.Log("Priming the Laser");

        //laser find's player position
        _laserEndPoint = GameManager.player.obj.transform.position;

        //delay for player to dodge, while animation warms up
        yield return new WaitForSeconds(_laserWarmUpTime);
        
        Debug.Log("Firing the Laser");

        //laser starts moving towards player, but slow (or traces player path?)
        _laserTracker.SetActive(true);
        float timeCount = 0;

        while (timeCount < _attackAnimTime)
        {
            //laser tracks player position while firing
            float laserSpeed = _laserSpeedModifier * GameManager.player.movement.MoveSpeed * Time.deltaTime;

            _laserEndPoint = Vector3.MoveTowards(_laserEndPoint, GameManager.player.obj.transform.position, laserSpeed);
            _laserTracker.transform.position = _laserEndPoint;

            timeCount += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        NextBossState();
    }

    private IEnumerator SummonMinions()
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
            PoolUtility.InstantiateFromPool(_minionWaveRef, _projectileSpawn, _minionRef);
            yield return new WaitForSeconds(0.2f);
        }

        //put Boss Animation here.
        //calculate wait time, defined by Animation
        yield return new WaitForSeconds(_idleTime);

        NextBossState();

    }
}
