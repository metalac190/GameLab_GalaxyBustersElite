using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
	[SerializeField] static int comboCounter = 0;
	[SerializeField] static int score = 0;
	
    void Update()
    {
		// For testing purposes only
		//if (Input.GetKeyDown(KeyCode.Space))
		//	IncreaseScore(10);

		score = GameManager.gm.score;
	}

	public static void IncreaseScore(int amount)
	{
		GameManager.gm.score += amount;

		if( GameManager.player.controller.GetOverloadCharge() <= 100 )
			GameManager.player.controller.IncreaseOverload(amount);

		Debug.Log("<color=yellow>" + amount + " Points!</color>");
	}

	public static void IncreaseCombo(){ comboCounter++; }
	public static void ResetCombo(){ comboCounter = -1; }

}
