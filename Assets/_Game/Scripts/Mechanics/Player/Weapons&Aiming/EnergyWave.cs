using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyWave : MonoBehaviour
{
	[SerializeField] public GameObject projectileVFX;
	[SerializeField] private float chargePercentage = 50f;
	[SerializeField] private float _speed = 0f;
	[SerializeField] protected float _damage = 2;
	[SerializeField] private float _scale = 0f;
	[SerializeField] private float lifeTime = 2f;
	[SerializeField] private float enemiesDamaged = 0;
	[SerializeField] public bool projectileActive = false;

	private Rigidbody rb;
	private float _time = 0;

	protected virtual void OnEnable()
	{
		// Get rigidbody component
		rb = GetComponent<Rigidbody>();
		projectileActive = false;
		_time = 0;
		_speed = 0;
		_damage = 0;
		enemiesDamaged = 0;
	}

	private void Update()
	{
		if (projectileActive)
		{
			// Update velocity
			rb.velocity = transform.forward * _speed;

			// Destroy projectile after lifetime expires or enemy cap reached
			_time += Time.deltaTime;
			if (_time > lifeTime || enemiesDamaged >= 5)
				gameObject.SetActive(false);
		}
	}

	public void SetDamage(float value) { _damage = value; }
	public void SetVelocity(float value) { _speed = value; }
	public void SetScale(float value) { _scale = value; }

	public void ActivateProjectile(float chargeAmount, float damage, float speed, float size)
	{
		// Set damage, speed, and size based on charge time
		SetDamage(damage);
		SetVelocity(speed);
		SetScale(size);

		projectileActive = true;
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
			enemiesDamaged++;
		}
	}
}
