using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	public float speed = 20f;
	public int damage = 2;
	public float lifeTime = 2f;
	private Rigidbody rb;

	private void Start()
	{
		rb = GetComponent<Rigidbody>();
		rb.velocity = transform.forward * speed;
	}

	private void Awake()
	{
		Destroy(gameObject, lifeTime);
	}

}
