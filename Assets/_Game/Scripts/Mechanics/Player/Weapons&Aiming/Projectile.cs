﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base class for modifying projectiles and how they behave
public class Projectile : MonoBehaviour
{
    [SerializeField] private float _speed = 20f;
	[SerializeField] protected float _damage = 2;

    [SerializeField] private float lifeTime = 2f;

    private Rigidbody rb;
    private float _time = 0;

    protected virtual void Awake()
    {
        // Move projectile forwards with set speed
        rb = GetComponent<Rigidbody>();
		rb.velocity = transform.forward * _speed;
	}

    protected virtual void OnEnable()
    {
        _time = 0;
        rb.velocity = transform.forward * _speed;
    }

    private void Update()
	{
        // Destroy projectile after lifetime expires
        _time += Time.deltaTime;
        if (_time > lifeTime)
            gameObject.SetActive(false);
	}

    protected virtual void OnCollisionEnter(Collision collision)
    {
        EntityBase entity = collision.gameObject.GetComponent<EntityBase>();
        entity?.TakeDamage(_damage);

        gameObject.SetActive(false);
    }

    public void SetDamage(float value)
    {
        _damage = value;
    }

    public void SetVelocity(float value)
    {
        _speed = value;
    }
        

}
