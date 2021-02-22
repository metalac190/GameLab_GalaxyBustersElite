using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_bullet : MonoBehaviour
{
    [SerializeField] private float _time = 5f;
    [SerializeField] private float _speed = 10f;
    private IEnumerator lifetime = null;

    private void Start()
    {
        Destroy(this.gameObject, _time);
    }

    private void FixedUpdate()
    {
        Vector3 distance = transform.forward * _speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, transform.position + distance, _speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(this.gameObject);
    }
}
