using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_bossRemote : MonoBehaviour
{
    private BossController _bossRef = null;

    private void Awake()
    {
        _bossRef = GetComponent<BossController>();
        if (_bossRef)
            Debug.Log("Boss Found, Remote Active");
        else
            Debug.Log("Boss Not Found, ABORT~!");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _bossRef?.StartBossFight();
        }
    }
}
