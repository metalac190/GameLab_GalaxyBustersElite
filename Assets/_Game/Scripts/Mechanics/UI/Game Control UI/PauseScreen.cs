using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PauseScreen : MonoBehaviour {

    [SerializeField] private Button resume, mainMenu;
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

        gameObject.SetActive(false);
    }
    private void Update()
    {
        referencedGM = GameObject.Find("Game Manager");
        if (referencedGM!= null)
        {
            pauseScore.text = referencedGM.GetComponent<GameManager>().score.ToString("00000");
        }
    }

}