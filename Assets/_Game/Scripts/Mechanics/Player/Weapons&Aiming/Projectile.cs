using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base class for modifying projectiles and how they behave
public class Projectile : MonoBehaviour
{
	public float speed = 20f;
	public int damage = 2;
	public float lifeTime = 2f;
	private Rigidbody rb;
    private float _time = 0;

    private void Awake()
    {
        // Move projectile forwards with set speed
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _time = 0;
        rb.velocity = transform.forward * speed;
    }

    private void Update()
	{
        // Destroy projectile after lifetime expires
        _time += Time.deltaTime;
        if (_time > lifeTime)
            gameObject.SetActive(false);
	}

    private void OnCollisionEnter(Collision collision)
    {
        EntityBase entity = collision.gameObject.GetComponent<EntityBase>();
        entity?.TakeDamage(damage);

        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        player?.DamagePlayer(damage);

        gameObject.SetActive(false);
    }

}
