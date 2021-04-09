using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossSegmentController : EntityBase
{
    //Refer to Ben Friedman for QA/Bugfixing on Boss System scripts

    [SerializeField] private BossController _bossRef = null;

    [Tooltip("Reference to Normal Boss Missile Prefab")]
    [SerializeField] private GameObject _missileRef = null;
    [SerializeField] private Transform _missileSpawnPoint = null;
    [SerializeField] private float _myDelay = 0f;
    
    public float Health { get { return _currentHealth; } }

    private List<GameObject> _missilePool = new List<GameObject>();
    private int _damage = 1;

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
        if (!_bossRef.isReady)
        {
            _bossRef.InvulnerableHit.Invoke();
            return;
        }

        base.TakeDamage(damage);
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
}
