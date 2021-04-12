using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
	public static event Action<string, int> onScoreIncreased;
	public static event Action onMultiplierChanged;
	public static event Action onScoreReset;

	static int score = 0;
	static int comboCounter = 0;
	static int comboMultiplier = 1;
	static int nearMisses = 0;
	static int nearMissScore = 20;
	static float scoreToOverchargeMultiplier = 0.1f;

	// Tracking for each destroyed enemy type
	public static int destroyedTotal = 0;
	public static int destroyedBandit = 0;
	public static int destroyedDrone = 0;
	public static int destroyedMinion = 0;
	public static int destroyedRammer = 0;
	public static int destroyedSpearhead = 0;

	// List of Challenges
	public static Challenges challenges;

	void Update()
    {
		score = GameManager.gm.score;
	}

	public static void IncreaseScore(string source, int amount)
	{
		GameManager.gm.score += (amount * comboMultiplier);

		if (!GameManager.player.controller.IsPlayerOverloaded())
			GameManager.player.controller.IncreaseOverload(amount * scoreToOverchargeMultiplier);

		Debug.Log("<color=yellow>" + (amount * comboMultiplier) + " Points!</color>");
		onScoreIncreased?.Invoke(source, amount * comboMultiplier);
	}

	public static void IncreaseScoreNoMultiplier(string source, int amount)
	{
		GameManager.gm.score += amount;

		if (!GameManager.player.controller.IsPlayerOverloaded())
			GameManager.player.controller.IncreaseOverload(amount * scoreToOverchargeMultiplier);

		Debug.Log("<color=yellow>" + amount + " Points!</color>");
		onScoreIncreased?.Invoke(source, amount);
	}

	public static void IncreaseScoreFlat(string source, int amount)
	{
		GameManager.gm.score += amount;

		Debug.Log("<color=yellow>" + amount + " Points!</color>");
		onScoreIncreased?.Invoke(source, amount);
	}

	public static void ScorePickup(int amount)
	{
		GameManager.gm.score += amount;

		Debug.Log("<color=yellow>" + amount + " Points!</color>");
		onScoreIncreased?.Invoke("", amount);
	}

	public static void ResetScore()
	{
		GameManager.gm.score = 0;
		Debug.Log("<color=yellow>Points Reset</color>");
		onScoreReset?.Invoke();
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

		onMultiplierChanged?.Invoke();
	}

	public static void ResetCombo()
	{
		comboCounter = -1;
		comboMultiplier = 1;
		Debug.Log("<color=yellow>Combo Reset</color>");
		Debug.Log("<color=yellow>x1 Score Multiplier</color>");
		onMultiplierChanged?.Invoke();
	}

	public static void NearMiss()
	{
		nearMisses++;
		IncreaseScore("NearMiss", nearMissScore);
	}

	public static void DestroyedEnemyType(EnemyTypes type)
	{
		switch (type)
		{
			case EnemyTypes.Bandit:
				destroyedTotal++;
				destroyedBandit++;
				break;

			case EnemyTypes.Drone:
				destroyedTotal++;
				destroyedDrone++;
				break;

			case EnemyTypes.Minion:
				destroyedTotal++;
				destroyedMinion++;
				break;

			case EnemyTypes.Rammer:
				destroyedTotal++;
				destroyedRammer++;
				break;

			case EnemyTypes.Spearhead:
				destroyedTotal++;
				destroyedSpearhead++;
				break;
		}
	}

	public static void SetHighScore()
	{
		switch (GameManager.gm.currentLevel)
		{
			case 1:
				PlayerPrefs.SetInt("HighScoreL1", score);
				break;
			case 2:
				PlayerPrefs.SetInt("HighScoreL2", score);
				break;
			case 3:
				PlayerPrefs.SetInt("HighScoreL3", score);
				break;
		}
	}

	public static int GetScore(){ return score; }
	public static int GetComboCount() { return comboCounter; }
	public static int GetComboMultiplier() { return comboMultiplier; }

}
