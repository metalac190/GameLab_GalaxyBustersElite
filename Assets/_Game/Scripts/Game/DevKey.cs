using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DevKey : MonoBehaviour {

    [SerializeField] private MainMenu mainMenu;
    [SerializeField] private TextMeshProUGUI text;

    private void Awake() {
        text.enabled = false;
    }

    private void Update() {
        if(GameManager.devMode)
            return;

        if(Input.GetKeyDown(KeyCode.Return)) {
            EnableDevMode();
        }
    }

    private void EnableDevMode() {
        GameManager.devMode = true;
        GameManager.gm.unlockedLevel = 3;
        StartCoroutine(SetText());
        mainMenu.UnlockLevels();
    }

    private IEnumerator SetText() {
        text.enabled = true;
        yield return new WaitForSeconds(3f);
        text.enabled = false;
    }

}