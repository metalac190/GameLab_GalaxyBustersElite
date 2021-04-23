using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateOverTime : MonoBehaviour
{
    [SerializeField] private Vector3 _spingAngles = Vector3.zero;
    [SerializeField] private float _spinSpeed = 1f;

    private void Update()
    {
        Vector3 spin = _spingAngles * _spinSpeed * Time.deltaTime;
        transform.Rotate(spin);
    }
}
