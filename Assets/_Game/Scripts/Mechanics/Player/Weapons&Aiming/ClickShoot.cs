using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickShoot : MonoBehaviour
{
	public GameObject projectile;
	public Transform spawnPoint;
	public float speed = 10f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetButton("Fire1"))
		{
			GameObject shot = Instantiate(projectile, spawnPoint.position, Quaternion.identity) as GameObject;
			shot.GetComponent<Rigidbody>().AddForce(transform.forward * speed * 10);
		}

	}
}
