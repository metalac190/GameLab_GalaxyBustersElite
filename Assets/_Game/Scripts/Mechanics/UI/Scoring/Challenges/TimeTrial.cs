using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTrial : ChallengeBase
{
	float secondsRemaining;

	private void Start()
	{
		secondsRemaining = threshold;
	}

	// Update is called once per frame
	void Update()
    {
		if (!challengeCompleted && !challengeFailed)
		{
			if(secondsRemaining > 0)
			{
				secondsRemaining -= Time.deltaTime;
				progress = threshold - secondsRemaining;
			}
			else
			{
				failure();
			}

			if (GameManager.gm.currentState == GameState.Win)
				victory();

			if (GameManager.gm.currentState == GameState.Fail)
				failure();

		}
	}

	public override string GetProgress()
	{
		return FormatTime(progress) + "/" + FormatTime(threshold);
	}

	private string FormatTime(float time)
	{
		int minutes = (int)time / 60;
		int seconds = (int)time - 60 * minutes;
		return string.Format("{0:0}:{1:00}", minutes, seconds);
	}
}
