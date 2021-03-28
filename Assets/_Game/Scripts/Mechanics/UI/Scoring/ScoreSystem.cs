using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
	static int score = 0;
	static int comboCounter = 0;
	static int comboMultiplier = 1;
	
    void Update()
    {
		// For testing purposes only
		//if (Input.GetKeyDown(KeyCode.Space))
		//	IncreaseScore(10);

		score = GameManager.gm.score;
	}

	public static void IncreaseScore(int amount)
	{
		GameManager.gm.score += (amount * comboMultiplier);

		if( GameManager.player.controller.GetOverloadCharge() <= 100 && !GameManager.player.controller.IsPlayerOverloaded())
			GameManager.player.controller.IncreaseOverload(amount);

		Debug.Log("<color=yellow>" + (amount * comboMultiplier) + " Points!</color>");
	}

	public static void IncreaseScoreNoMultiplier(int amount)
	{
		GameManager.gm.score += amount;

		if (GameManager.player.controller.GetOverloadCharge() <= 100 && !GameManager.player.controller.IsPlayerOverloaded())
			GameManager.player.controller.IncreaseOverload(amount);

		Debug.Log("<color=yellow>" + amount + " Points!</color>");
	}

	public static void ScorePickup(int amount)
	{
		GameManager.gm.score += amount;
		Debug.Log("<color=yellow>" + amount + " Points!</color>");
	}

	public static void ResetScore()
	{
		GameManager.gm.score = 0;
		Debug.Log("<color=yellow>Points Reset</color>");
	}

	public static void IncreaseCombo()
	{
		comboCounter++;

		if (comboCounter == 16)
			Debug.Log("<color=yellow>x4 Score Multiplier</color>");
		else if (comboCounter == 8)
			Debug.Log("<color=yellow>x3 Score Multiplier</color>");
		else if (comboCounter == 4)
			Debug.Log("<color=yellow>x2 Score Multiplier</color>");

		if (comboCounter >= 16)
			comboMultiplier = 4;
		else if (comboCounter >= 8)
			comboMultiplier = 3;
		else if (comboCounter >= 4)
			comboMultiplier = 2;
		else
			comboMultiplier = 1;
	}
	public static void ResetCombo()
	{
		comboCounter = -1;
		comboMultiplier = 1;
		Debug.Log("<color=yellow>Combo Reset</color>");
		Debug.Log("<color=yellow>x1 Score Multiplier</color>");
	}

	public static int GetScore(){ return score; }
	public static int GetComboCount() { return comboCounter; }
	public static int GetComboMultiplier() { return comboMultiplier; }

}
