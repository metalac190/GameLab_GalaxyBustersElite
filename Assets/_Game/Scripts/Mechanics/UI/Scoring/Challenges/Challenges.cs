using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Challenges : MonoBehaviour
{
	[SerializeField] private ChallengeBase[] challenges = new ChallengeBase[3];

	private void OnEnable()
	{
		if (GameManager.gm.challenges == null)
			GameManager.gm.challenges = this;
	}

	public ChallengeBase[] GetChallenges(){ return challenges; }
}
