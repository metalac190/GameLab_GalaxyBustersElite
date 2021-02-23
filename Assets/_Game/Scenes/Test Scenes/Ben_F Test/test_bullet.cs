using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_bullet : MonoBehaviour
{
    [SerializeField] private float _lifetime = 5f;
    [SerializeField] private float _speed = 10f;
    private float _time = 0f;

    private void OnEnable()
    {
        _time = _lifetime; 
    }

    private void Update()
    {
        _time -= Time.deltaTime;
        if (_time <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        Vector3 distance = transform.forward * _speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, transform.position + distance, _speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        gameObject.SetActive(false);
    }
}
