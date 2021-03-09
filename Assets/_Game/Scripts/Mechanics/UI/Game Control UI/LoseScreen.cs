using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoseScreen : MonoBehaviour {

    [SerializeField] private Button mainMenu;

    void Start() {
        GameManager.gm.SetLoseScreen(gameObject);
        mainMenu.onClick.AddListener(() => {
            GameManager.gm.LoadScene(Levels.MainMenu);
        });

        gameObject.SetActive(false);
    }

}