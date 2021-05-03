using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreChallenge : ChallengeBase
{
	private void Update()
	{
		progress = ScoreSystem.GetScore();

		if (!challengeCompleted && !challengeFailed)
		{
			if (progress >= threshold)
				victory();

			if (GameManager.gm.currentState == GameState.Win || GameManager.gm.currentState == GameState.Fail)
				failure();
		}

	}

	public override string GetProgress()
	{
		return progress + "\n/" + threshold;
	}
}
