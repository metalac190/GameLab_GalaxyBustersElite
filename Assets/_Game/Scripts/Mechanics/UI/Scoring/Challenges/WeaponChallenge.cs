using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponChallenge : ChallengeBase
{
	[SerializeField] WeaponChallengeType type;

	bool blaster, energyBurst, laserBeam = false;
	string weaponID;
	bool isOverloading = false;

	private void Update()
	{
		if (!challengeCompleted && !challengeFailed)
		{
			switch (type)
			{
				case WeaponChallengeType.collectWeapons:
					progress = 0;
					weaponID = GameManager.player.controller.GetCurrentWeaponID();

					switch (weaponID)
					{
						case "Blaster": blaster = true; break;
						case "Energy Burst": energyBurst = true; break;
						case "Laser Beam": laserBeam = true; break;
					}

					progress += blaster ? 1 : 0;
					progress += energyBurst ? 1 : 0;
					progress += laserBeam ? 1 : 0;
					break;

				case WeaponChallengeType.useOverload:
					if (!isOverloading && GameManager.player.controller.isOverloaded)
						progress++;

					isOverloading = GameManager.player.controller.isOverloaded;
					break;
			}

			if (progress >= threshold)
				victory();

			if (GameManager.gm.currentState == GameState.Win)
				failure();
		}

	}
}

public enum WeaponChallengeType
{
	collectWeapons = 1,
	useOverload = 2
};
