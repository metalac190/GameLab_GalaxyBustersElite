using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleChallenge : ChallengeBase
{
	private void Update()
	{
		if (!challengeCompleted && !challengeFailed)
		{
			if (progress >= threshold)
				victory();

			if (GameManager.gm.currentState == GameState.Win || GameManager.gm.currentState == GameState.Fail)
				failure();
		}

	}

	public void IncreaseCollectibles()
	{
		progress++;
	}
}
