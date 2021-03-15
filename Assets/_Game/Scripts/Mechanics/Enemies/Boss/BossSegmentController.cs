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

    private List<GameObject> missilePool = new List<GameObject>();

    #region Listeners
    private void OnEnable()
    {
        _bossRef.Attacked.AddListener(OnAttacked);
    }

    
    private void OnDisable()
    {
        _bossRef.Attacked.RemoveListener(OnAttacked);
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
    }

    private void OnMissileAttack()
    {
        //missile animation

        //instantiate missile
        if (_missileRef != null)
        {
            InstantiateMissileFromPool();
        }

        //missile.target = player
    }

    //pools missiles instead of instantiating, uses less resources over time
    private void InstantiateMissileFromPool()
    {
        foreach (GameObject missile in missilePool)
        {
            if (missile != null && missile.activeInHierarchy == false)
            {
                missile.SetActive(true);
                missile.transform.position = _missileSpawnPoint.position;
                missile.transform.rotation = _missileSpawnPoint.rotation;
                return;
            }
        }

        GameObject newMissle = Instantiate(_missileRef, _missileSpawnPoint.position, _missileSpawnPoint.rotation, null);
        missilePool.Add(newMissle);
    }

    private void OnLaserAttack()
    {
        //laster attack animation
    }
}
