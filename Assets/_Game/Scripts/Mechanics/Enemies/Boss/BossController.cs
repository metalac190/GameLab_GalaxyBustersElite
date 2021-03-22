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

    [Header("Boss Statistics")]

    [SerializeField] private float _moveSpeed = 10f;
    [Tooltip("Number of times to Move when Bloodied,\nInclusive Min, Exclusive Max")]
    [SerializeField] private Vector2 _numberOfMoves = new Vector2(1, 3);
    [Tooltip("Starting Health for each Segment.\nTotal Segment Health derived from\nStarting Health * number of Segments")]
    [SerializeField] private int _segmentHealth = 100;
    private BossSegmentController[] _segmentRefs = new BossSegmentController[0];

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
    private List<GameObject> _minionWaveRef = new List<GameObject>();

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

    bool _segmentsAlive = true;
    private Coroutine _BossBehavior = null;
    private BossState _nextState = BossState.Idle;
    private Vector3 _startPosition = Vector3.zero;

    private void Awake()
    {
        //save position to create bounds during movement behavior
        _startPosition = transform.position;
        
        _segmentRefs = GetComponentsInChildren<BossSegmentController>();
        for (int i=0; i < _segmentRefs.Length; i++)
        {
            _segmentRefs[i].SetHealth(_segmentHealth);
            _segmentRefs[i].SetDelay(i * _delaySeconds);
            _segmentRefs[i].SetDamage(_attackDamage);
        }
    }

    public override void TakeDamage(float damage)
    {
        //when no Segments are left, allow Boss to TakeDamage()
        if (!_segmentsAlive)
        {
            base.TakeDamage(damage);
        }
        else
        {
            //while Segments are alive, play Invulnerable FX isntead.
            InvulnerableHit.Invoke();
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
        if (_BossBehavior == null)
        {
            _nextState = BossState.Idle;
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
    #endregion

    #region Behaviors

    private IEnumerator BossIdle()
    {   
        //wait predetermined amount of time
        //TODO Idle Animation?

        //run check each idle loop to catch an errors
        OnSegmentDestroyed();
        yield return new WaitForSeconds(_idleTime);

        NextBossState();
    }

    private IEnumerator Bloodied()
    {
        ////TODO Bloodied Animation
        //set invulnerable
        
        //get animation time
        yield return new WaitForSeconds(_idleTime);

        NextBossState();
    }

    private IEnumerator MovePattern(int count)
    {
        Debug.Log("Boss is Moving");

        //recursive exit check
        if (count <= 0)
        {
            NextBossState();
            yield return null;
        }
        else
        {
            //TODO Move Animation?

            //identifies points on X/Y plane, at Z distance from player
            Vector3 moveAmount = new Vector3(Random.Range(0f, 10f), Random.Range(0f, 10f), 0);
            Vector3 point = new Vector3(_startPosition.x + moveAmount.x, _startPosition.y + moveAmount.y, _startPosition.z);

            //moveTowards those points, at speed
            while (transform.position != point)
            {
                //will eventually perfectly equal Point, due to MoveTowards()?
                transform.position = Vector3.MoveTowards(transform.position, point, _moveSpeed);
                yield return new WaitForEndOfFrame();
            }

            //calculate difference in time between move animation and time it takes to move
            //wait for difference (if greater than 0)
            yield return new WaitForSeconds(_idleTime * _delaySeconds);

            //recursive until 0
            StartCoroutine(MovePattern(count - 1));
        }
    }
    #endregion

    #region Attacks

    private IEnumerator RingAttack()
    {
        //TODO Ring Attack Animation

        _projectileSpawn.LookAt(GameManager.player.obj.transform);

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
                yield return new WaitForSeconds(_delaySeconds);
            }
        }

        //pass animation time as wait
        //can we get a event for when animation ends, and listen?
        yield return new WaitForSeconds(_attackAnimTime);
        NextBossState();
    }

    private IEnumerator MissileAttack()
    {
        //TODO Missile Attack Animation

        _projectileSpawn.LookAt(GameManager.player.obj.transform);

        //Dependant on Bloodied state
        if (!_segmentsAlive)
        {
            //TODO Bloodied??Missile Attack Animation
            
            //amount of missiles determined by Designer
            for (int i = 0; i < _bloodiedProjectileCount; i++)
            {
                yield return new WaitForSeconds(_delaySeconds);
                GameObject bullet = PoolUtility.InstantiateFromPool(_missilePool, _projectileSpawn, _missileRef);
                BossMissile missile = bullet.GetComponent<BossMissile>();
                missile.SetDamage(_attackDamage);
            }    
        }

        //get animation time
        yield return new WaitForSeconds(_attackAnimTime);

        NextBossState();
    }

    private IEnumerator LaserAttack()
    {
        //TODO Laser Warm Up Animation

        //laser find's player position
        _laserEndPoint = GameManager.player.obj.transform.position;

        //delay for player to dodge, while animation warms up
        //set by designer, animation needs to cut short or shrink with warm up time
        yield return new WaitForSeconds(_laserWarmUpTime);

        //TODO Laser ATTACK Animation

        //laser starts moving towards player, but slow (or traces player path?)
        _laserTracker.SetActive(true);  //TODO Laser VFX?    
        
        //can I have an inactive object just track for Transform purposes, or should I use collision?
        _laserTracker.GetComponent<LaserDamage>().SetDamage(_attackDamage);

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

        //attack time set by designers, needs to cut off or shrink any long attack animations, player should be focused on laser VFX though
        _laserTracker.SetActive(false);
        NextBossState();
    }

    private IEnumerator SummonMinions()
    {
        //Dependant if Minions are active
        //Refills current wave, stacks multiple waves if earlier waves are not defeated
        //Summons up to 5 Minions?

        //TODO Summon Animation? Replay Idle?

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
            yield return new WaitForSeconds(_delaySeconds);
        }

        //get animation time
        yield return new WaitForSeconds(_idleTime);

        NextBossState();
    }
    #endregion
}
