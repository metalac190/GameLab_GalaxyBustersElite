using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class MainMenu : MonoBehaviour
{

    private static bool splashScreenDisplayed = false;
    public GameObject splashScreen;

    [SerializeField] private Button mission2, mission3;

    [Header("Player Movement UI")]
    [SerializeField] Image menuBackgroundImage;
    [SerializeField] CinemachineDollyCart player;
    [SerializeField] GameObject[] stuffToTurnOnForPM;
    [SerializeField] GameObject[] stuffToTurnOffForPM;
    [SerializeField] Sprite[] menuBackgroundSprites;

    private void Awake() {
        // Unlocked levels
        if(GameManager.gm.unlockedLevel < 2)
            mission2.interactable = false;
        if(GameManager.gm.unlockedLevel < 3)
            mission3.interactable = false;

        // Splash screen
        if(splashScreenDisplayed)
            splashScreen.SetActive(false);
        else {
            splashScreen.SetActive(true);
            splashScreenDisplayed = true;
            GameManager.gm.currentLevel = 0;
        }
    }

    public void QuitGame()
    {
        Debug.Log(" Application.Quit() called. Does not quit while in inspector.");
        Application.Quit();
    }

    // Loading Mission Scenes
    public void LoadMission1()
    {
        GameManager.gm.LoadScene(Levels.Mission1);
    }

    public void LoadMission2()
    {
        GameManager.gm.LoadScene(Levels.Mission2);
    }

    public void LoadMission3()
    {
        GameManager.gm.LoadScene(Levels.Mission3);
    }

    // Enable level select
    public void UnlockLevels(bool level2 = true, bool level3 = true) {
        if(level2)
            mission2.interactable = true;
        if(level3)
            mission3.interactable = true;
    }

    public void ShowPlayerMovementControlSettings(bool show)
    {
        if (show)
        {
            menuBackgroundImage.sprite = menuBackgroundSprites[0];
            player.m_Position = 0;

            foreach (GameObject o in stuffToTurnOnForPM)
            {
                o.SetActive(true);
            }
            foreach (GameObject o in stuffToTurnOffForPM)
            {
                o.SetActive(false);
            }
        }
        else
        {
            menuBackgroundImage.sprite = menuBackgroundSprites[1];

            foreach (GameObject o in stuffToTurnOnForPM)
            {
                o.SetActive(false);
            }
            foreach (GameObject o in stuffToTurnOffForPM)
            {
                o.SetActive(true);
            }
        }
    }
}
