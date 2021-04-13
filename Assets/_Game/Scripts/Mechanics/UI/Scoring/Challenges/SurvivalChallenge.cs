using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivalChallenge : ChallengeBase
{

	private void Update()
	{
		if (!challengeCompleted && !challengeFailed)
		{
			progress = GameManager.player.controller.GetPlayerHealth();

			// If health falls below threshold or player failed mission
			if (progress < threshold || GameManager.gm.currentState == GameState.Fail)
				failure();

			if (GameManager.gm.currentState == GameState.Win)
				victory();

		}

	}

	public override string GetProgress()
	{
		return "";
	}
}
