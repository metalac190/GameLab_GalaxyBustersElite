using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BriefingObjectives : MonoBehaviour
{
	[SerializeField] Challenges challengeList;
	[SerializeField] TextMeshProUGUI[] objectivesText = new TextMeshProUGUI[3];
	int level;

	private void OnEnable()
	{
		for (int i = 0; i < challengeList.GetChallenges().Length; i++)
		{
			objectivesText[i].text = challengeList.GetChallenges()[i].challengeText;
		}
	}

}
