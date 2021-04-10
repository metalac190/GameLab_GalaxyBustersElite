using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Challenge : MonoBehaviour
{
	[SerializeField] ChallengeTypes type;
	[SerializeField] string challengeText;
	[SerializeField] EnemyTypes enemyType;
	[SerializeField] int threshold;
	[SerializeField] string victoryText;
	[SerializeField] int scoreValue = 1000;

	int trackedValue = 0;
	bool challengeCompleted = false;

	void Update()
	{
		// Change behavior depending on challenge type
		switch (type)
		{
			case ChallengeTypes.destruction:

				// Determine which enemy to track
				switch (enemyType)
				{
					case EnemyTypes.any:
						trackedValue = ScoreSystem.destroyedTotal;
						break;

					case EnemyTypes.bandit:
						trackedValue = ScoreSystem.destroyedBandit;
						break;

					case EnemyTypes.drone:
						trackedValue = ScoreSystem.destroyedDrone;
						break;

					case EnemyTypes.minion:
						trackedValue = ScoreSystem.destroyedMinion;
						break;

					case EnemyTypes.rammer:
						trackedValue = ScoreSystem.destroyedRammer;
						break;

					case EnemyTypes.spearhead:
						trackedValue = ScoreSystem.destroyedSpearhead;
						break;

					case EnemyTypes.tank:
						trackedValue = ScoreSystem.destroyedTank;
						break;
				}
				break;

			case ChallengeTypes.survival:
				break;

			case ChallengeTypes.skill:
				break;
		}

		if (!challengeCompleted && trackedValue >= threshold)
			victory();
	}

	public void victory()
	{
		Debug.Log("<color=green>" + victoryText + "</color>");
		ScoreSystem.IncreaseScoreFlat(scoreValue);
		challengeCompleted = true;
	}
}
