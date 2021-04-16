using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_bossRemote : MonoBehaviour
{
    [SerializeField] private BossController _bossRef = null;
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
    }
}
