using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(FlickerController))]
public class BossSegmentController : EntityBase
{
    //Refer to Ben Friedman for QA/Bugfixing on Boss System scripts

    [Header("References DO NOT TOUCH")]
    [Tooltip("Reference to BossController GameObject object")]
    [SerializeField] private BossController _bossRef = null;
    [Tooltip("The gameobject with the corresponding Mesh to this Segment/Rig position")]
    [SerializeField] private GameObject _meshSegment = null;
    [Tooltip("Reference to Boss Missile Prefab asset")]
    [SerializeField] private GameObject _missileRef = null;
    [Tooltip("Reference to SpawnPoint Transform objet")]
    [SerializeField] private Transform _missileSpawnPoint = null;
    private List<GameObject> _missilePool = new List<GameObject>();

    public float Health { get { return _currentHealth; } }

    private float _myDelay = 0f;
    private int _damage = 1;
    private FlickerController flickerController = null;

    private void Awake()
    {
        //find and override flash material to maintain consistency with boss
        flickerController = GetComponent<FlickerController>();
        flickerController.FlashMaterial = _bossRef.FlashMaterial;
    }

    #region Listeners
    private void OnEnable()
    {
        _bossRef.Attacking.AddListener(OnAttack);
    }

    
    private void OnDisable()
    {
        _bossRef.Attacking.RemoveListener(OnAttack);
    }
    #endregion

    public override void TakeDamage(float damage)
    {
        if (_bossRef.isInvulnerable)
        {
            _bossRef.InvulnerableHit.Invoke();
            return;
        }
        else
        {
            //base.TakeDamage(damage)
            _currentHealth -= damage;

            if (_currentHealth <= 0)
            {
                Died.Invoke();

                //control mesh visibility here, not in UnityEvents
                _meshSegment.SetActive(false);
                gameObject.SetActive(false);
            }
            else
            {
                Damaged.Invoke();

                //control damage flash here
                flickerController.CallFlicker();
            }
        }
    }

    #region Public Accessors
    public void SetHealth(float value)
    {
        if (value > maxHealth)
            maxHealth = value;

        _currentHealth = value;
    }

    public void SetDamage(int value)
    {
        _damage = value;
    }

    public void SetDelay(float time)
    {
        _myDelay = time;
    }
    #endregion

    #region Missile Attack
    //BossAttacks type transfered via int type and not enum
    private void OnAttack(int value)
    {
        BossAttacks attackType = (BossAttacks)value;
        
        switch(attackType)
        {
            case BossAttacks.MissileAttack:
                StartCoroutine(MissileDelay(_myDelay));
                break;

            default:
                //Does not implement Ring, Laser, Minion Attack?
                //Might have animation triggers from Boss.Attacked.Invoke()
                break;
        }
    }
    
    private IEnumerator MissileDelay(float time)
    {
        while (time > 0)
        {
            yield return new WaitForEndOfFrame();
            time -= Time.deltaTime;
        }

        OnMissileAttack();
    }

    private void OnMissileAttack()
    {
        //TODO missile animation
        GameObject missile = PoolUtility.InstantiateFromPool(_missilePool, _missileSpawnPoint, _missileRef);

        BossMissile bullet = missile.GetComponent<BossMissile>();

        bullet.SetTarget(GameManager.player.obj);
        bullet.SetDamage(_damage);
    }
    #endregion
}
