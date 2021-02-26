using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossSegmentController : MonoBehaviour
{
    public UnityEvent Died;

    [SerializeField] private BossController _bossRef = null;

    [SerializeField] private GameObject _missileRef = null;
    [SerializeField] private Transform _missileSpawnPoint = null;
    [SerializeField] private float _myDelay = 0f;
    private float _wait = 0f;

    private int _health = 0;
    public int Health { get { return _health; } }

    private List<GameObject> _missilePool = new List<GameObject>();

    #region Listeners
    private void OnEnable()
    {
        _bossRef.Attacking.AddListener(OnAttacked);
    }

    
    private void OnDisable()
    {
        _bossRef.Attacking.RemoveListener(OnAttacked);
    }
    #endregion

    //used exclusivly in conjunction with OnMissileAttack()
    //used instead of IEnumerator, might change?
    private void Update()
    {
        if (_wait > 0)
        {
            _wait -= Time.deltaTime;
            if (_wait <= 0)
            {
                OnMissileAttack();
            }
        }
    }

    //currently unused, inherit from EnemyBase?
    public void SetHealth(int value)
    {
        _health = value;
    }
    
    public void TakeDamage(int value)
    {
        _health -= value;
        if (_health <= 0)
        {
            OnDied();
        }
    }

    private void OnDied()
    {
        Died.Invoke();

        //play death animation
    }

    //BossAttacks type transfered via int type, due to UnityEvents constraints
    private void OnAttacked(int value)
    {
        BossAttacks attackType = (BossAttacks)value;
        
        switch(attackType)
        {
            case BossAttacks.RingAttack:
                OnRingAttack();
                break;

            case BossAttacks.MisisleAttack:
                _wait = _myDelay;
                break;

            case BossAttacks.LaserAttack:
                OnLaserAttack();
                break;

            default:
                break;
        }
    }

    private void OnRingAttack()
    {
        //ring attack animation
        //if any
    }

    private void OnMissileAttack()
    {
        //missile animation

        //instantiate missile
        if (_missileRef != null)
        {
            PoolUtility.InstantiateFromPool(_missilePool, _missileSpawnPoint, _missileRef);
        }

        //missile.target = player
    }

    private void OnLaserAttack()
    {
        //laser attack animation
        //if any
    }
}
