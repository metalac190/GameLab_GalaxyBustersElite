using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeEnable : MonoBehaviour
{
	[SerializeField] private GameObject challengesLevel1, challengesLevel2, challengesLevel3;
	void OnEnable()
	{
		switch (GameManager.gm.currentLevel)
		{
			case 1:
				challengesLevel1.SetActive(true);
				break;
			case 2:
				challengesLevel2.SetActive(true);
				break;
			case 3:
				challengesLevel3.SetActive(true);
				break;
		}
	}
}
