using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PauseObjectives : MonoBehaviour
{
	[SerializeField] Challenges challengeList;
	[SerializeField] PauseObjectiveContainer[] pauseObjectives = new PauseObjectiveContainer[3];
	ChallengeBase currentChallenge;

	private void Update()
	{
		if (GameManager.gm.currentState == GameState.Gameplay || GameManager.gm.currentState == GameState.Win)
		{
			if (challengeList == null)
				challengeList = ScoreSystem.challenges;

			for (int i = 0; i < challengeList.GetChallenges().Length; i++)
			{
				currentChallenge = challengeList.GetChallenges()[i];

				pauseObjectives[i].textBox.text = currentChallenge.challengeText;
				pauseObjectives[i].progressBox.text = currentChallenge.GetProgress();

				if (currentChallenge.challengeCompleted)
					pauseObjectives[i].imgChecked.SetActive(true);

				if (currentChallenge.challengeFailed)
					pauseObjectives[i].imgCross.SetActive(true);

			}
		}

		
	}
}
