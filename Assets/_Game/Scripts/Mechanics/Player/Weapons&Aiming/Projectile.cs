using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base class for modifying projectiles and how they behave
public class Projectile : MonoBehaviour
{
    [SerializeField] private float _speed = 20f;
    public float Speed
    {
        get { return _speed; }
        set
        {
            _speed = value;
            if(rb)
                rb.velocity = transform.forward * _speed;
        }
    }
	[SerializeField] protected float _damage = 2;

    [SerializeField] private float lifeTime = 2f;
    [SerializeField] private bool disableOnHit = true;

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

    public void SetDamage(float value)
    {
        _damage = value;
    }
     
    public void SetVelocity(float value) {
        Speed = value;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        //when colliding with any Phsyical Collision, disable projectile
        gameObject.SetActive(false);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        //when hitting an Entity, damage it and disable projectile
        EntityBase entity = other.gameObject.GetComponent<EntityBase>();
        if (entity != null)
        {
            entity?.TakeDamage(_damage);
            if (disableOnHit) gameObject.SetActive(false);
        }
    }

}
