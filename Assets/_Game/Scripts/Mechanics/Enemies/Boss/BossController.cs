using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(FlickerController))]
public class BossController : EntityBase
{
    //Refer to Ben Friedman for QA/Bugfixing on Boss System scripts

    [Header("Boss Events")]
    [Tooltip("When the Death Animation ends\nNot just zero hp")]
    public UnityEvent FullyDead;
    public UnityEvent InvulnerableHit;
    public IntEvent Attacking;
    

    [Header("Boss Settings")]

    [SerializeField] private float _moveSpeed = 10f;
    [Tooltip("Distance for how far the Boss can move from its center")]
    [SerializeField] private float _moveMax = 20f;
    [Tooltip("Number of times to Move when Bloodied,\nInclusive Min, Exclusive Max")]
    [SerializeField] private Vector2 _numberOfMoves = new Vector2(1, 3);

    [Header("Segment Settings")]

    [Tooltip("Starting Health for each Segment.\nTotal Segment Health derived from\nStarting Health * number of Segments")]
    [SerializeField] private float _segmentHealth = 10;

    [Header("Timers")]

    [Tooltip("Time in Seconds to wait during Idle state.")]
    [SerializeField] private float _idleTime = 2f;
    [Tooltip("Delay between successive attacks\nEg. Missiles rapid fire")]
    [SerializeField] private float _delaySeconds = 0.2f;
    [Tooltip("Time in Seconds for attack animations to Warm Up\nBefore dealing damage.")]
    [SerializeField] private float _attackWarmUpTime = 1f;
    [Tooltip("Minimum time spent in each attack state\nRegardless for animation time")]
    [SerializeField] private float _minAttackTime = 2f;
    private List<float> AnimTimes = new List<float>();

    [Header("Attack Settings")]

    [Tooltip("Standardized amount of damage to apply to Player.")]
    [SerializeField] private int _attackDamage = 1;
    [Tooltip("Number of Minions to spawn during behavior loop.")]
    [SerializeField] private int _minionWaveSize = 5;
    [Tooltip("Number of Missiles to spawn during Bloodied Missile Attack")]
    [SerializeField] private int _bloodiedProjectileCount = 3;
    [Tooltip("The Laser's movespeed as a percentage of the Player's movespeed")]
    [SerializeField] private float _laserSpeedModifier = 0.8f;

    [Header("Asset References! Do Not Touch!")]
    [SerializeField] private BossSegmentController[] _segmentRefs = new BossSegmentController[0];

    [Header("Minion Refs")]
    [Tooltip("Reference to Minion Prefab.")]
    [SerializeField] private GameObject _minionRef = null;
    [Tooltip("Staring Group of Minions, with Waypoints")]
    [SerializeField] private Queue<GameObject> _minionWaveRef = new Queue<GameObject>();
    [Tooltip("Variable Spawn Points\nDefault to 2")]
    [SerializeField] private Transform[] _minionSpawns = new Transform[2];
    [Tooltip("Middle Waypoint Idenfitied, for Minions that spawn in secondary Position[s]")]
    [SerializeField] private int[] _minionMidpoint = new int[1];

    [Header("Attack & Projectile Refs")]
    //Ring Attack Pooling
    [Tooltip("Reference to Ring Attack Prefab.")]
    [SerializeField] private GameObject _ringRef = null;
    private Queue<GameObject> _ringQueue = new Queue<GameObject>();

    //Missile Pooling
    [Tooltip("Reference to Bloodied Boss Missile Prefab")]
    [SerializeField] private GameObject _missileRef = null;
    private Queue<GameObject> _missileQueue = new Queue<GameObject>();

    [Tooltip("Reference to Game Object where Ring Attack originates from.")]
    [SerializeField] private Transform _projectileSpawn = null;

    [Tooltip("Tracks and Damages player during Laser Attack")]
    [SerializeField] private GameObject _laserSource = null;
    [SerializeField] private GameObject _laserBeam = null;
    private Vector3 _laserEndPoint = Vector3.zero;

    [Header("Animation Refs")]
    [Tooltip("Parent Transform for Collision + Art")]
    [SerializeField] private GameObject _bossRoot = null;

    [Tooltip("Animator Controller for Boss Animations")]
    [SerializeField] private Animator _bossAnim = null;

    private FlickerController flickerController = null;

    [HideInInspector]
    public bool isInvulnerable = true;
    [HideInInspector]
    public bool isSegmentsAlive
    {
        get
        {
            if (_isSegAlive)
            {
                foreach (BossSegmentController segment in _segmentRefs)
                    if (segment.isActiveAndEnabled) return true;

                //shortcut to stop counting segs when determined false at least once
                _isSegAlive = false;
            }

            return false;
        }
    }
    private bool _isSegAlive = true;
    private Coroutine _BossBehavior = null;
    private BossState _nextState = BossState.PreFight;
    [HideInInspector]
    public BossState State = BossState.PreFight;
    private Vector3 _startPosition = Vector3.zero;

    private BossAttacks calledAttack = BossAttacks.None;

    private void Awake()
    {
        //save position to create bounds during movement behavior
        _startPosition = _bossRoot.transform.position;
        _laserSource.SetActive(false);
        _laserBeam.SetActive(false);

        //find and override flash material to maintain consistency with segments
        flickerController = GetComponent<FlickerController>();

        for (int i = 0; i < _segmentRefs.Length; i++)
        {
            _segmentRefs[i].SetHealth(_segmentHealth);
            _segmentRefs[i].SetDelay(i * _delaySeconds);
            _segmentRefs[i].SetDamage(_attackDamage);
        }

        GetAnimationTimes(_bossAnim);
    }

    public override void TakeDamage(float damage)
    {
        //when no Segments are left, allow Boss to TakeDamage()
        if (isInvulnerable || isSegmentsAlive)
        {
            //while Segments are alive, play Invulnerable FX isntead.
            InvulnerableHit.Invoke();
            Debug.Log("Boss Invunlerable");
        }
        else
        {
            _currentHealth -= damage;

            if (_currentHealth <= 0)
            {
                Debug.Log("Death");
                Died.Invoke();

                //Override to implement Boss Death Animation
                _nextState = BossState.Dead;
                NextBossState();

                ZenoxFiller.DisableZenoxFiller();
            }
            else
            {
                Damaged.Invoke();

                //control damage flash here
                flickerController.CallFlicker();
            }
        }

    }

    #region Listeners
    private void OnEnable()
    {
        foreach (BossSegmentController segment in _segmentRefs)
            segment.Died.AddListener(OnSegmentDestroyed);

        // End game when defeated
        //Please don't end game when defeated :) Teni F.
        //Died.AddListener(() => GameManager.gm.WinGame());
        //AddListener(GameManager.gm.WinGame); ?
    }

    private void OnDisable()
    {
        foreach (BossSegmentController segment in _segmentRefs)
            segment.Died.RemoveListener(OnSegmentDestroyed);
    }
    #endregion

    #region Public Accessors
    /// <summary> 
    ///     Returns Boss's current health, plus all active Segments' health
    ///
    /// </summary>
    public float TotalHealth
    {
        get
        {
            float value = 0;
            foreach (BossSegmentController segment in _segmentRefs)
            {
                if (segment.isActiveAndEnabled)
                    value += segment.Health;
            }

            return value + _currentHealth;
        }
    }

    /// <summary> Used to kick-start Boss state machine. 
    /// <para>
    ///     Use after Cinematic, Trigger, Animation, or whatever.
    /// </para>
    /// </summary>
    public void StartBossFight()
    {
        if (_nextState == BossState.PreFight)
        {
            ZenoxFiller.EnableZenoxFiller();

            isInvulnerable = false;

            _nextState = BossState.Idle;
            _bossAnim.SetTrigger("StartFight");

            NextBossState();
        }
    }

    public void CallAttack(BossAttacks attack)
    {
        calledAttack = attack;
    }

    public void StartPhaseTwo()
    {
        foreach (BossSegmentController segment in _segmentRefs)
        {
            segment.TakeDamage(999);
        }
    }
    #endregion

    #region State Machine Controllers

    /// <summary> 
    ///     Interjects behavior loop with a check for Bloodied State
    /// <para> 
    ///     Called primarilly by BossSegmentController.Destroyed.Invoke()
    /// </para>
    /// </summary>
    private void OnSegmentDestroyed()
    {
        //If current or previous check returned any alive segments
        if (isSegmentsAlive == true)
        {
            //animation
            _bossAnim.SetTrigger("SegmentDestroyed");

            int numSegments = 0;
            foreach (BossSegmentController segments in _segmentRefs)
            {
                if (segments.isActiveAndEnabled)
                {
                    numSegments++;
                }
            }
            
            if (numSegments > 1)
            {
                DialogueTrigger.TriggerZenoxPartDestroyedDialogue();
            }
            else
            {
                _bossAnim.SetBool("SegmentsAlive", false);
                _nextState = BossState.Bloodied;
            }
        }

        //Function should not be able to be called once all segments are destroyed, but if state is false, can called again, nothing happens
        //Protection from false negative during gameplay, still only runs function once even if segements are alive and accidentally triggers Bloodied.
    }

    private void NextBossState()
    {
        switch (_nextState)
        {
            case BossState.Idle:

                _nextState = BossState.Attack;

                _BossBehavior = StartCoroutine(BossIdle());
                break;

            case BossState.Attack:

                if (isSegmentsAlive)
                    _nextState = BossState.Idle;
                else
                    _nextState = BossState.Moving;

                GenerateAttack();
                break;

            case BossState.Moving:

                _nextState = BossState.Attack;

                int numberOfMoves = (int)Random.Range(_numberOfMoves.x, _numberOfMoves.y);
                _BossBehavior = StartCoroutine(MovePattern(numberOfMoves));
                break;

            case BossState.Bloodied:

                _nextState = BossState.Moving;

                _BossBehavior = StartCoroutine(Bloodied());
                break;

            case BossState.Dead:
                _BossBehavior = StartCoroutine(DeathAnimation());
                break;

            default:
                Debug.Log("Invalid Boss State");
                break;
        }
    }

    private void GenerateAttack()
    {
        State = BossState.Attack;
        BossAttacks randomAttack;

        if (isSegmentsAlive)
        {
            if (calledAttack == BossAttacks.None)
            {
                randomAttack = (BossAttacks)Random.Range(1, 5);
            }
            else
            {
                randomAttack = calledAttack;
                calledAttack = BossAttacks.None;
            }
        }
        else
            randomAttack = (BossAttacks)Random.Range(1, 3);


        //Signals to Segments which Attack is active, to animate/behave accordingly
        //public facing, accessible by animators, fx, other systems?
        Attacking.Invoke((int)randomAttack);

        //Calls Boss animations/behaviors
        switch (randomAttack)
        {
            case BossAttacks.MissileAttack:
                _BossBehavior = StartCoroutine(MissileAttack());
                break;
            case BossAttacks.RingAttack:
                _BossBehavior = StartCoroutine(RingAttack());
                break;
            case BossAttacks.LaserAttack:
                _BossBehavior = StartCoroutine(LaserAttack());
                break;
            case BossAttacks.SummonMinions:
                _BossBehavior = StartCoroutine(SummonMinions());
                break;
            default:
                Debug.Log("Invalid Normal Attack called: " + randomAttack);
                break;
        }
    }
    #endregion

    #region Behaviors
    private IEnumerator BossIdle()
    {
        State = BossState.Idle;
        //animation
        //Idle Anim is Unconditional return from other states.

        //wait predetermined amount of time
        yield return new WaitForSeconds(_idleTime);

        NextBossState();
    }

    private IEnumerator Bloodied()
    {
        State = BossState.Bloodied;
        //animation controller through OnSegmentDestroyed check

        //set invulnerable
        isInvulnerable = true;
        
        DialogueTrigger.TriggerZenoxHalfHealthDialogue();

        //get animation time
        yield return new WaitForSeconds(AnimTimes[3]);

        isInvulnerable = false;

        NextBossState();
    }

    private IEnumerator MovePattern(int count)
    {
        State = BossState.Moving;
        //animation?

        //recursive exit check
        if (count <= 0)
        {
            NextBossState();
            yield return null;
        }
        else
        {
            //animation?
            //TODO Move Animation?
            float moveTime = _idleTime;

            //identifies points on X/Y plane, at Z distance from player
            Vector3 moveAmount = new Vector3(Random.Range(0f, _moveMax), Random.Range(0f, _moveMax), 0);
            Vector3 point = new Vector3(_startPosition.x + moveAmount.x, _startPosition.y + moveAmount.y, _startPosition.z);

            //moveTowards those points, at speed
            while (_bossRoot.transform.position != point)
            {
                //will eventually perfectly equal Point, due to MoveTowards()?
                _bossRoot.transform.position = Vector3.MoveTowards(_bossRoot.transform.position, point, _moveSpeed * Time.deltaTime);
                moveTime -= Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            //calculate difference in time between actual time spent moveing and minimum wait time
            //wait for difference (if greater than 0), or don't wait if move time is excess of wait time
            if (moveTime > 0)
            {
                yield return new WaitForSeconds(_idleTime - moveTime);
            }

            //recursive until 0
            StartCoroutine(MovePattern(count - 1));
        }
    }

    private IEnumerator DeathAnimation()
    {
        _bossAnim.SetTrigger("Death");
        isInvulnerable = true;

        Debug.Log("Dying Animation");

        yield return new WaitForSeconds(AnimTimes[11] + _delaySeconds);

        FullyDead.Invoke();
        _bossRoot.SetActive(false);
    }
    #endregion

    #region Attacks
    private IEnumerator MissileAttack()
    {
        //animation
        _bossAnim.SetInteger("AttackType", 1);

        //warm up animation before attacking
        yield return new WaitForSeconds(_attackWarmUpTime);

        //reset animation trigger to avoid repeat triggers
        _bossAnim.SetInteger("AttackType", 0);

        float delayTime = 0;

        //Dependant on Bloodied state
        if (isSegmentsAlive)
        {
            int segAlive = 0;

            foreach (BossSegmentController segment in _segmentRefs)
            {
                if (segment.isActiveAndEnabled)
                {
                    delayTime += _delaySeconds;
                    segAlive++;
                }

            }
        }
        else
        {
            _projectileSpawn.LookAt(GameManager.player.obj.transform);

            //amount of missiles determined by Designer
            for (int i = 0; i < _bloodiedProjectileCount; i++)
            {
                GameObject bullet = PoolUtility.InstantiateFromQueue(_missileQueue, _projectileSpawn, _missileRef);
                BossMissile missile = bullet.GetComponent<BossMissile>();
                missile.SetTarget(GameManager.player.obj);
                missile.SetDamage(_attackDamage);

                delayTime += _delaySeconds;
                yield return new WaitForSeconds(_delaySeconds);
            }
        }

        //get animation time
        float returnTime = Mathf.Max(_minAttackTime, AnimTimes[4] - _attackWarmUpTime);
        yield return new WaitForSeconds(_minAttackTime);

        NextBossState();
    }

    private IEnumerator RingAttack()
    {
        //animation
        _bossAnim.SetInteger("AttackType", 2);
        
        //warm up animation before attacking
        yield return new WaitForSeconds(_attackWarmUpTime);

        //reset animation trigger to avoid repeat triggers
        _bossAnim.SetInteger("AttackType", 0);

        _projectileSpawn.LookAt(GameManager.player.obj.transform);

        float delayTime = 0;

        //RingAttack behavior dependant on Bloodied state
        if (isSegmentsAlive)
        {
            //Single Ring Attack
            GameObject bullet = PoolUtility.InstantiateFromQueue(_ringQueue, _projectileSpawn, _ringRef);
            Projectile missile = bullet.GetComponent<Projectile>();
            missile.SetDamage(_attackDamage);
        }
        else
        {
            //Up to 3 Rings?
            for (int i = 0; i < _bloodiedProjectileCount; i++)
            {
                GameObject bullet = PoolUtility.InstantiateFromQueue(_ringQueue, _projectileSpawn, _ringRef);
                Projectile missile = bullet.GetComponent<Projectile>();
                missile.SetDamage(_attackDamage);

                delayTime += _delaySeconds;
                yield return new WaitForSeconds(_delaySeconds);
            }
        }

        //get animation time
        float returnTime = Mathf.Max(_minAttackTime, AnimTimes[5] - _attackWarmUpTime);
        yield return new WaitForSeconds(_minAttackTime);

        NextBossState();
    }

    private IEnumerator LaserAttack()
    {
        //animation
        _bossAnim.SetInteger("AttackType", 3);
        
        //laser find's player position
        _laserEndPoint = GameManager.player.obj.transform.position;
        _laserSource.SetActive(true);

        //source follows player without shooting
        float timeCount = 0;
        while (timeCount < _attackWarmUpTime)
        {
            LaserFollowPlayer();

            timeCount += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        //reset animation trigger to avoid repeat triggers
        _bossAnim.SetInteger("AttackType", 0);

        //laserbeam activates, and starts dealing damage
        _laserBeam.SetActive(true);
        _laserBeam.GetComponent<LaserDamage>().SetDamage(_attackDamage);
        
        //source continues to follow player
        while (timeCount < AnimTimes[6])
        {
            LaserFollowPlayer();

            timeCount += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        _laserSource.SetActive(false);
        _laserBeam.SetActive(false);

        NextBossState();
    }

    private void LaserFollowPlayer()
    {
        //laser tracks player position while firing
        float laserSpeed = _laserSpeedModifier * GameManager.player.movement.MoveSpeed * Time.deltaTime;

        _laserEndPoint = Vector3.MoveTowards(_laserEndPoint, GameManager.player.obj.transform.position, laserSpeed);
        _laserSource.transform.LookAt(_laserEndPoint);
    }

    private IEnumerator SummonMinions()
    {
        //Dependant if Minions are active
        //Refills current wave, stacks multiple waves if earlier waves are not defeated
        //Summons up to 5 Minions?

        //animation
        _bossAnim.SetInteger("AttackType", 4);

        //warm up animation before attacking
        yield return new WaitForSeconds(_attackWarmUpTime);

        //reset animation trigger to avoid repeat triggers
        _bossAnim.SetInteger("AttackType", 0);

        int waveCount = 0;
        foreach (GameObject minion in _minionWaveRef)
        {
            if (minion != null && minion.activeInHierarchy)
                waveCount++;
        }

        float delayTime = 0;

        for (int i = waveCount; i < _minionWaveSize; i++)
        {
            //reliant on Minions being Disabled when killed, and not Destroyed()
            int spawnRand = Random.Range(0, _minionSpawns.Length);
            GameObject minionObject = PoolUtility.InstantiateFromQueue(_minionWaveRef, _minionSpawns[spawnRand], _minionRef);
            EnemyMovement minionMove = minionObject.GetComponentInChildren<EnemyMovement>();
            minionMove?.RestartPath();

            //if minion in spawned in spawnpoint "2", set minion's next waypoint to "Midpoint"
            if (spawnRand > 0)
                minionMove.SetWaypoint(_minionMidpoint[spawnRand - 1]);

            delayTime += _delaySeconds;
            yield return new WaitForSeconds(_delaySeconds);
        }

        //get animation time
        float returnTime = Mathf.Max(_minAttackTime, AnimTimes[7] - _attackWarmUpTime);
        yield return new WaitForSeconds(_minAttackTime);

        NextBossState();
    }
    #endregion

    private void GetAnimationTimes(Animator bossAnim)
    {
        //attackAnimTimes
        AnimationClip[] bossClips = bossAnim.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in bossClips)
        {
            //Debug.Log(clip.name + " " + clip.length);
            AnimTimes.Add(clip.length);
        }
        //Idle, Idle, WeaponDestroyed, AllWeaponsDestroy, A, B, C, D, E, CombatStateB, F, DramaticDeath, Damage
    }
}