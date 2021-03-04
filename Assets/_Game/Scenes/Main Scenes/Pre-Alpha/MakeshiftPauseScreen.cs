using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MakeshiftPauseScreen : MonoBehaviour {

    [SerializeField] private Button resume, mainMenu;

    void Start() {
        GameManager.gm.SetPauseMenu(gameObject);
        resume.onClick.AddListener(() => {
            GameManager.gm.Paused = false;
        });
        mainMenu.onClick.AddListener(() => {
            GameManager.gm.LoadScene(Levels.MainMenu);
        });

        gameObject.SetActive(false);
    }

}