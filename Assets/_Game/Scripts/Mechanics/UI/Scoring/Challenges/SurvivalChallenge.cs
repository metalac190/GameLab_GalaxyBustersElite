using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivalChallenge : ChallengeBase
{
	[SerializeField] int healthThreshold;
	[SerializeField] float currentHealth;

	private void Update()
	{

		if (!challengeCompleted && !challengeFailed)
		{
			currentHealth = GameManager.player.controller.GetPlayerHealth();

			// If health falls below threshold
			if (currentHealth < healthThreshold)
				failure();

			if (GameManager.gm.currentState == GameState.Win)
				victory();

		}

	}
}
