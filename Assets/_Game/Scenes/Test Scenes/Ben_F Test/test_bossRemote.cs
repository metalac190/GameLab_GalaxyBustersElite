using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_bossRemote : MonoBehaviour
{
    [Tooltip("Start Fight with Space\n1) Que a Missile Attack\n2) Que a Ring Attack\n3) Que a Laser Attack\n4) Que a Minion Summon")]
    [SerializeField] private BossController _bossRef = null;
    [Tooltip("Damage the selected segment with 'Q'")]
    [SerializeField] private BossSegmentController _segRef = null;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _bossRef?.StartBossFight();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            _segRef?.TakeDamage(1f);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _bossRef?.CallAttack(BossAttacks.MissileAttack);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _bossRef?.CallAttack(BossAttacks.RingAttack);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _bossRef?.CallAttack(BossAttacks.LaserAttack);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            _bossRef?.CallAttack(BossAttacks.SummonMinions);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            _bossRef?.StartPhaseTwo();
        }
    }
}
