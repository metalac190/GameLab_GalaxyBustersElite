using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupTargetDetector : MonoBehaviour
{
	private Collider capsule;
	[SerializeField] GameObject targetedIcon;
	public List<GameObject> targets = new List<GameObject>();
	public bool pauseTracking = false;

	private void OnEnable()
	{
		capsule = GetComponent<Collider>();
		SetCollider(false);
	}

	public void SetCollider(bool state)
	{
		capsule.enabled = state;
		targets.Clear();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!targets.Contains(other.gameObject) && !pauseTracking)
		{
			targets.Add(other.gameObject);
			Instantiate(targetedIcon, other.transform);
		}
	}

}
