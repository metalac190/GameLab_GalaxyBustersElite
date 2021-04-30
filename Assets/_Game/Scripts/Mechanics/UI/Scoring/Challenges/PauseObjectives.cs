using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PauseObjectives : MonoBehaviour
{
	[SerializeField] Challenges challengeList;
	[SerializeField] PauseObjectiveContainer[] pauseObjectives = new PauseObjectiveContainer[3];
	ChallengeBase currentChallenge;
	GameState currentState;
	bool notplaying;

	private void Update()
	{
		currentState = GameManager.gm.currentState;
		notplaying = currentState == GameState.Gameplay || currentState == GameState.Win || currentState == GameState.Fail;
		if (notplaying)
		{
			if (challengeList == null)
				challengeList = GameManager.gm.challenges;

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
