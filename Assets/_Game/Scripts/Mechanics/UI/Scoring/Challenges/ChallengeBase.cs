using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeBase : MonoBehaviour
{
	[SerializeField] protected bool challengeCompleted = false;
	[SerializeField] protected bool challengeFailed = false;
	[SerializeField] string challengeText;
	[SerializeField] int scoreValue = 0;

	public void victory()
	{
		if (!challengeFailed)
		{
			Debug.Log("<color=green>" + gameObject.name + " Completed!</color>");
			ScoreSystem.IncreaseScoreFlat("Challenge", scoreValue);
			challengeCompleted = true;
		}
	}

	public void failure()
	{
		if (!challengeCompleted)
		{
			Debug.Log("<color=red>" + gameObject.name + " Failed.</color>");
			challengeFailed = true;
		}
	}
}
