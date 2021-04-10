using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Challenges : MonoBehaviour
{

	[SerializeField] Challenge[] challenges = new Challenge[3];

	private void OnEnable()
	{
		ScoreSystem.challenges = this;
	}
}
