using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class RingCollectible : MonoBehaviour
{
	[SerializeField] CollectibleChallenge challengeScript;

	private void OnTriggerEnter(Collider other)
	{
		challengeScript.IncreaseCollectibles();
	}
}
