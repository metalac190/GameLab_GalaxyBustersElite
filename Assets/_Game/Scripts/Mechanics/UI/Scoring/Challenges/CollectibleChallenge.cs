using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleChallenge : ChallengeBase
{
	[SerializeField] int threshold;
	[SerializeField] int progress = 0;

	private void Update()
	{
		if (!challengeCompleted && !challengeFailed)
		{
			if (progress >= threshold)
				victory();
		}

	}

	public void IncreaseCollectibles()
	{
		progress++;
	}
}
