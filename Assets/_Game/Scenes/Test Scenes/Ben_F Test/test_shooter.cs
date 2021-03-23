using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_shooter : MonoBehaviour
{
    [SerializeField] private GameObject _bullet_ref = null;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Fire();
        }
    }

    private void Fire()
    {
        if (_bullet_ref != null)
        {
            Instantiate(_bullet_ref, transform);
            Debug.Log("Fire!");
        }
    }
}
