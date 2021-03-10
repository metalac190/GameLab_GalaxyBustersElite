using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMissile : EntityBase
{
    [SerializeField] private float _lifetime = 5f;
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _turnSpeed = 0.25f;

    private GameObject playerRef = null;
    private float _time = 0f;
    
    private void OnEnable()
    {
        _time = _lifetime; 
    }

    private void Update()
    {
        //setactive timer, instead of Destroy(this, delay), usable for Pooling
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

        //if missile has a playerReference, rotate continuously
        if (playerRef != null)
        {
            //don't return to player once past their Z position...
            //test on track, test with Teni that Boss Arena is Z aligned?
            if (playerRef.transform.position.z > transform.position.z)
                return;

            Vector3 dir = playerRef.transform.position - transform.position;
            Quaternion rot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, _turnSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        gameObject.SetActive(false);
    }

    public void SetTarget(Vector3 point)
    {
        //convert to Coroutine (or add to Update loop), to turn over time, instead of snapping
        transform.LookAt(point);
    }

    public void SetTarget(GameObject player)
    {
        playerRef = player;
    }
}
