using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class WinScreen : MonoBehaviour {

    [SerializeField] private Button nextLevel, mainMenu;

    void Start() {
        GameManager.gm.SetWinScreen(gameObject);
        mainMenu.onClick.AddListener(() => {
            GameManager.gm.LoadScene(Levels.MainMenu);
        });

		// Set high score for level
		ScoreSystem.SetHighScore();

        // Next level
        UnityAction nextLevelFunc;
        switch(GameManager.gm.currentLevel) {
            case 1:
                nextLevelFunc = () => GameManager.gm.LoadScene(Levels.Mission2);
                break;
            case 2:
                nextLevelFunc = () => GameManager.gm.LoadScene(Levels.Mission3);
                break;
            case 3:
                nextLevel.gameObject.SetActive(false);
                nextLevelFunc = () => { };
                break;
            default:
                nextLevelFunc = () => {
                    Debug.LogError("Attempting to load next level from invalid level number: " + GameManager.gm.currentLevel + "\nLoading main menu...");
                    GameManager.gm.LoadScene(Levels.MainMenu);
                };
                break;
        }
        if(nextLevel.gameObject.activeSelf)
            nextLevel.onClick.AddListener(nextLevelFunc);

        gameObject.SetActive(false);
    }

}