using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupTargetDetector : MonoBehaviour
{
	private Collider capsule;
	public List<GameObject> targets = new List<GameObject>();

	private void OnEnable()
	{
		capsule = GetComponent<Collider>();
		SetCollider(false);
		targets.Clear();
	}

	public void SetCollider(bool state)
	{
		capsule.enabled = state;
		targets.Clear();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!targets.Contains(other.gameObject))
		{
			targets.Add(other.gameObject);
		}
	}

}
