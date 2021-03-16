using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BossMissile : EntityBase
{
    private Rigidbody rb = null;

    [SerializeField] private float _lifetime = 5f;
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _turnSpeed = 0.25f;

    private GameObject playerRef = null;
    private float _time = 0f;
    private int _damage = 1;
    
    private void OnEnable()
    {
        _time = _lifetime; 
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        //setactive timer, instead of Destroy(this, delay), usable for Pooling
        _time -= Time.deltaTime;
        if (_time <= 0)
        {
            gameObject.SetActive(false);
        }

        rb.velocity = transform.forward * _speed;
        
        //if missile has a playerReference, rotate continuously
        if (playerRef != null)
        {
            //don't return to player once past their "Z" position
            //TODO create dynamic comparison...
            if (playerRef.transform.position.z > transform.position.z)
                return;

            Vector3 dir = playerRef.transform.position - transform.position;
            Quaternion rot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, _turnSpeed * Time.deltaTime);
        }
    }

    //set values when Instantiated, able to adjust on the fly
    public void SetTarget(Vector3 point)
    {
        //convert to Coroutine (or add to Update loop), to turn over time, instead of snapping
        transform.LookAt(point);
    }

    public void SetTarget(GameObject player)
    {
        playerRef = player;
    }

    public void SetDamage(int value)
    {
        _damage = value;
    }

    private void OnCollisionEnter(Collision collision)
    {
        EntityBase entity = collision.gameObject.GetComponent<EntityBase>();
        entity?.TakeDamage(_damage);

        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        player?.DamagePlayer(_damage);

        //self destruct
        TakeDamage(999);
    }
}
