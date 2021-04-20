using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class PauseScreen : MonoBehaviour {

    [SerializeField] private Button resume, mainMenu, restart;
    [SerializeField] private TextMeshProUGUI pauseScore;
    [SerializeField] private GameObject referencedGM;
    void Start() {
        GameManager.gm.SetPauseMenu(gameObject);
        resume.onClick.AddListener(() => {
            GameManager.gm.Paused = false;
        });
        mainMenu.onClick.AddListener(() => {
            GameManager.gm.LoadScene(Levels.MainMenu);
        });
        UnityAction restartFunc;
        switch (GameManager.gm.currentLevel)
        {
            case 1:
                restartFunc = () => GameManager.gm.LoadScene(Levels.Mission1);
                break;
            case 2:
                restartFunc = () => GameManager.gm.LoadScene(Levels.Mission2);
                break;
            case 3:
                restartFunc = () => GameManager.gm.LoadScene(Levels.Mission3);
                break;
            default:
                restartFunc = () => {
                    Debug.LogError("Attempting to restart game from invalid level number: " + GameManager.gm.currentLevel + "\nLoading main menu...");
                    GameManager.gm.LoadScene(Levels.MainMenu);
                };
                break;
        }
        restart.onClick.AddListener(restartFunc);

        gameObject.SetActive(false);
    }
    private void Update()
    {
        referencedGM = GameObject.Find("Game Manager");
        if (referencedGM!= null)
        {
            pauseScore.text = referencedGM.GetComponent<GameManager>().score.ToString("000000");
        }
    }

}