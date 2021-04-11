using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTrial : ChallengeBase
{
	[SerializeField] float maxTime;
	[SerializeField] float timer;
	float secondsRemaining;

	private void Start()
	{
		secondsRemaining = maxTime;
	}

	// Update is called once per frame
	void Update()
    {
		if (!challengeCompleted && !challengeFailed)
		{
			if(secondsRemaining > 0)
			{
				secondsRemaining -= Time.deltaTime;
				timer = maxTime - secondsRemaining;
			}
			else
			{
				failure();
			}

			if (GameManager.gm.currentState == GameState.Win)
				victory();

		}
	}
}
