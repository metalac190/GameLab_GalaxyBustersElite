using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossSegmentController : EntityBase
{
    //implement Boss as Singleton?
    //boss is always prefab, not too much harm rn?
    [SerializeField] private BossController _bossRef = null;

    [Tooltip("Reference to Normal Boss Missile Prefab")]
    [SerializeField] private GameObject _missileRef = null;
    [SerializeField] private Transform _missileSpawnPoint = null;
    [SerializeField] private float _myDelay = 0f;

    public int Health { get { return _currentHealth; } }

    private List<GameObject> _missilePool = new List<GameObject>();

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

    //used by BossController for consistent health across all Segments
    public void SetHealth(int value)
    {
        _currentHealth = value;
    }

    //used by BossController for consistent timing for missile attack 
    public void SetDelay(float time)
    {
        _myDelay = time;
    }

    //BossAttacks type transfered via int type, due to UnityEvents constraints
    private void OnAttack(int value)
    {
        BossAttacks attackType = (BossAttacks)value;
        
        switch(attackType)
        {
            case BossAttacks.MisisleAttack:
                StartCoroutine(MissileDelay(_myDelay));
                break;

            default:
                //Does not implement Ring, Laser, Minion Attack?
                //Might have animation triggers from Boss.Attacked.Invoke()
                break;
        }
    }

    /// <summary> Delays Missile attack, to fire in series with other Segments.
    /// 
    /// </summary>
    /// <param name="time"> Time in Seconds to wait. </param>
    /// <returns></returns>
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
        //missile animation
        GameObject missile = null;

        //instantiate missile
        if (_missileRef != null)
        {
            missile = PoolUtility.InstantiateFromPool(_missilePool, _missileSpawnPoint, _missileRef);
        }

        BossMissile bullet = missile.GetComponent<BossMissile>();

        //bullet?.SetTarget(GameManager.player.obj.transform.position);
        bullet?.SetTarget(GameManager.player.obj);
    }
}
