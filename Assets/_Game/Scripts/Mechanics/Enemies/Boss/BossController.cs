using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossController : EntityBase
{
    //Refer to Ben Friedman for QA/Bugfixing on Boss System scripts

    [Header("Boss Events")]
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
    [SerializeField] private BossSegmentController[] _segmentRefs = new BossSegmentController[0];

    [Header("Timers")]

    [Tooltip("Time in Seconds to wait during Idle state.")]
    [SerializeField] private float _idleTime = 2f;
    [Tooltip("Placeholder.\nAmount of time in Seconds per attack.\nDependant on type of attack chosen.")]
    [SerializeField] private float _attackAnimTime = 2f;
    [Tooltip("Delay between successive attacks\nEg. Bloodied Missiles rapid fire")]
    [SerializeField] private float _delaySeconds = 0.2f;
    [Tooltip("Time in Seconds for Laser animation to Warm Up\nBefore dealing damage.")]
    [SerializeField] private float _laserWarmUpTime = 2f;
    private Vector3 _laserEndPoint = Vector3.zero;
    private List<float> attackAnimTimes = new List<float>();

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

    //Minion Pooling
    [Tooltip("Reference to Minion Prefab.")]
    [SerializeField] private GameObject _minionRef = null;
    [Tooltip("Staring Group of Minions, with Waypoints")]
    [SerializeField] private List<GameObject> _minionWaveRef = new List<GameObject>();
    [Tooltip("Variable Spawn Points\nDefault to 2")]
    [SerializeField] private Transform[] _minionSpawns = new Transform[2];
    [Tooltip("Middle Waypoint Idenfitied, for Minions that spawn in secondary Position[s]")]
    [SerializeField] private int[] _minionMidpoint = new int[1];

    //Ring Attack Pooling
    [Tooltip("Reference to Ring Attack Prefab.")]
    [SerializeField] private GameObject _ringRef = null;
    private List<GameObject> _ringPool = new List<GameObject>();

    //Missile Pooling
    [Tooltip("Reference to Bloodied Boss Missile Prefab")]
    [SerializeField] private GameObject _missileRef = null;
    private List<GameObject> _missilePool = new List<GameObject>();

    [Tooltip("Reference to Game Object where Ring Attack originates from.")]
    [SerializeField] private Transform _projectileSpawn = null;

    [Tooltip("Tracks and Damages player during Laser Attack")]
    [SerializeField] private GameObject _laserTracker = null;

    [Tooltip("Parent Transform for Collision + Art")]
    [SerializeField] private GameObject _bossRoot = null;

    [Tooltip("Animator Controller for Boss Animations")]
    [SerializeField] private Animator _bossAnim = null;

    public bool isInvulnerable = false;
    private bool _segmentsAlive = true;
    private Coroutine _BossBehavior = null;
    private BossState _nextState = BossState.Idle;
    private Vector3 _startPosition = Vector3.zero;

    private void Awake()
    {
        //save position to create bounds during movement behavior
        _startPosition = _bossRoot.transform.position;
        _laserTracker.SetActive(false);

        for (int i=0; i < _segmentRefs.Length; i++)
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
        if (!isInvulnerable)
        {
            _currentHealth -= damage;

            if (_currentHealth <= 0)
            {
                Died.Invoke();

                //Override to implement Boss Death Animation
                _bossAnim.SetTrigger("Death");
            }
            else
            {
                Damaged.Invoke();

                //animation?
                _bossAnim.SetTrigger("TakeDamage");
            }
        }
        else
        {
            //while Segments are alive, play Invulnerable FX isntead.
            InvulnerableHit.Invoke();
            Debug.Log("Boss Invunlerable");
        }
        
    }

    #region Listeners
    private void OnEnable()
    {
        foreach (BossSegmentController segment in _segmentRefs)
        {
            segment.Died.AddListener(OnSegmentDestroyed);
        }
        
        // End game when defeated
        Died.AddListener(() => GameManager.gm.WinGame());
        //AddListener(GameManager.gm.WinGame); ?
    }

    private void OnDisable()
    {
        foreach (BossSegmentController segment in _segmentRefs)
        {
            segment.Died.RemoveListener(OnSegmentDestroyed);
        }
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
        Debug.Log("Fight Invoked");

        if (_BossBehavior == null && isInvulnerable == true)
        {
            Debug.Log("Fight Started");
            isInvulnerable = false;
            _nextState = BossState.Idle;
            _bossAnim.SetTrigger("StartFight");
            NextBossState();
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
        if (_segmentsAlive)
        {
            //animation
            _bossAnim.SetTrigger("SegmentDestroyed");

            //Set to False, and enable if any are still alive
            _segmentsAlive = false;
            foreach (BossSegmentController segment in _segmentRefs)
            {
                if (segment.isActiveAndEnabled)
                    _segmentsAlive = true;
            }

            //If previous check returned alive, but now check returns false, call Bloodied state
            if (_segmentsAlive == false)
            {
                //animation
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

    private void GenerateAttack()
    {
        Debug.Log("boss attack");
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
            case BossAttacks.MissileAttack:
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
    #endregion

    #region Behaviors
    private IEnumerator BossIdle()
    {
        //animation
        //Idle Anim is Unconditional return from other states.

        //wait predetermined amount of time
        yield return new WaitForSeconds(_idleTime);

        NextBossState();
    }

    private IEnumerator Bloodied()
    {
        //animation controller through OnSegmentDestroyed check
        
        //set invulnerable
        isInvulnerable = true;
        
        //get animation time
        yield return new WaitForSeconds(_idleTime);

        isInvulnerable = false;

        NextBossState();
    }

    private IEnumerator MovePattern(int count)
    {
        //animation?
        Debug.Log("Boss is Moving");

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
    #endregion

    #region Attacks
    private IEnumerator MissileAttack()
    {
        //animation
        _bossAnim.SetInteger("AttackType", 1);

        float delayTime = 0;

        //Dependant on Bloodied state
        if (_segmentsAlive)
        {
            
            foreach(BossSegmentController segment in _segmentRefs)
            {
                if (segment.isActiveAndEnabled)
                    delayTime += _delaySeconds;
            }
        }
        else
        {
            _projectileSpawn.LookAt(GameManager.player.obj.transform);

            //amount of missiles determined by Designer
            for (int i = 0; i < _bloodiedProjectileCount; i++)
            {
                GameObject bullet = PoolUtility.InstantiateFromPool(_missilePool, _projectileSpawn, _missileRef);
                BossMissile missile = bullet.GetComponent<BossMissile>();
                missile.SetDamage(_attackDamage);

                delayTime += _delaySeconds;
                yield return new WaitForSeconds(_delaySeconds);
            }
        }

        //get animation time
        yield return new WaitForSeconds(attackAnimTimes[3] - delayTime);

        NextBossState();
    }

    private IEnumerator RingAttack()
    {
        //animation
        _bossAnim.SetInteger("AttackType", 2);

        _projectileSpawn.LookAt(GameManager.player.obj.transform);

        float delayTime = 0;

        //RingAttack behavior dependant on Bloodied state
        if (_segmentsAlive)
        {
            //Single Ring Attack
            Debug.Log("Firing the normal Ring Attack");
            GameObject bullet = PoolUtility.InstantiateFromPool(_ringPool, _projectileSpawn, _ringRef);
            Projectile missile = bullet.GetComponent<Projectile>();
            missile.SetDamage(_attackDamage);
        }
        else 
        {
            //TODO Bloodied??Ring Attack Animation?

            //Up to 3 Rings?
            Debug.Log("Firing bloodied Ring Attack");
            for (int i=0; i < _bloodiedProjectileCount; i++)
            {
                GameObject bullet = PoolUtility.InstantiateFromPool(_ringPool, _projectileSpawn, _ringRef);
                Projectile missile = bullet.GetComponent<Projectile>();
                missile.SetDamage(_attackDamage);

                delayTime += _delaySeconds;
                yield return new WaitForSeconds(_delaySeconds);
            }
        }

        //pass animation time as wait
        yield return new WaitForSeconds(attackAnimTimes[4] - delayTime);
        NextBossState();
    }

    private IEnumerator LaserAttack()
    {
        //animation
        _bossAnim.SetInteger("AttackType", 3);

        //laser find's player position
        _laserEndPoint = GameManager.player.obj.transform.position;

        //delay for player to dodge, while animation warms up
        //set by designer, animation needs to cut short or shrink with warm up time
        yield return new WaitForSeconds(_laserWarmUpTime);

        //laser starts moving towards player, but slow (or traces player path?)
        _laserTracker.SetActive(true);  //TODO Laser VFX?    
        
        //can I have an inactive object just track for Transform purposes, or should I use collision?
        _laserTracker.GetComponent<LaserDamage>().SetDamage(_attackDamage);

        float timeCount = 0;
        while (timeCount < attackAnimTimes[5])
        {
            //laser tracks player position while firing
            float laserSpeed = _laserSpeedModifier * GameManager.player.movement.MoveSpeed * Time.deltaTime;

            _laserEndPoint = Vector3.MoveTowards(_laserEndPoint, GameManager.player.obj.transform.position, laserSpeed);
            _laserTracker.transform.position = _laserEndPoint;

            timeCount += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        //attack time set by animation?     //cut off or shrink any long attack animations, player should be focused on laser VFX though
        _laserTracker.SetActive(false);
        NextBossState();
    }

    private IEnumerator SummonMinions()
    {
        //Dependant if Minions are active
        //Refills current wave, stacks multiple waves if earlier waves are not defeated
        //Summons up to 5 Minions?

        //animation
        _bossAnim.SetInteger("AttackType", 4);

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
            GameObject minionObject = PoolUtility.InstantiateFromPool(_minionWaveRef, _minionSpawns[spawnRand], _minionRef);
            EnemyMovement minionMove = minionObject.GetComponentInChildren<EnemyMovement>();
            minionMove?.RestartPath();

            //if minion in spawned in spawnpoint "2", set minion's next waypoint to "Midpoint"
            if (spawnRand > 0)
                minionMove.SetWaypoint(_minionMidpoint[spawnRand - 1]);

            delayTime += _delaySeconds;
            yield return new WaitForSeconds(_delaySeconds);
        }

        //get animation time
        yield return new WaitForSeconds(attackAnimTimes[7] - delayTime);

        NextBossState();
    }
    #endregion

    private void GetAnimationTimes(Animator bossAnim)
    {
        //attackAnimTimes
        AnimationClip[] bossClips = bossAnim.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in bossClips)
        {
            Debug.Log(clip.name + " " + clip.length);
            attackAnimTimes.Add(clip.length);
        }
        //Boss_Idle(Start), Idle, Damage, A, B, C, D, E, Idle(Phase2?), F, Damage(2?), Damage(Segment?)
    }
}
