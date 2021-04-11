using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructionChallenge : ChallengeBase
{
	[SerializeField] EnemyTypes enemyType;
	[SerializeField] int threshold;
	[SerializeField] int progress = 0;

	private void Update()
	{
		progress = 0;

		if (!challengeCompleted)
		{
			// Determine which enemy to track
			switch (enemyType)
			{
				case EnemyTypes.any:
					progress = ScoreSystem.destroyedTotal;
					break;

				case EnemyTypes.bandit:
					progress = ScoreSystem.destroyedBandit;
					break;

				case EnemyTypes.drone:
					progress = ScoreSystem.destroyedDrone;
					break;

				case EnemyTypes.minion:
					progress = ScoreSystem.destroyedMinion;
					break;

				case EnemyTypes.rammer:
					progress = ScoreSystem.destroyedRammer;
					break;

				case EnemyTypes.spearhead:
					progress = ScoreSystem.destroyedSpearhead;
					break;

				case EnemyTypes.each:
					progress += ScoreSystem.destroyedBandit > 0 ? 1 : 0;
					progress += ScoreSystem.destroyedDrone > 0 ? 1 : 0;
					progress += ScoreSystem.destroyedMinion > 0 ? 1 : 0;
					progress += ScoreSystem.destroyedRammer > 0 ? 1 : 0;
					progress += ScoreSystem.destroyedSpearhead > 0 ? 1 : 0;
					break;
			}

			// If player destroys the necessary number of enemies
			if (progress >= threshold)
				victory();

		}
		
	}
}
