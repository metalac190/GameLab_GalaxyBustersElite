using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DevKey : MonoBehaviour {

    [SerializeField] private MainMenu mainMenu;
    [SerializeField] private TextMeshProUGUI text;

    private static bool keyTriggered = false;
    private static readonly KeyCode[] konamiCode = {KeyCode.UpArrow, KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow,
            KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.B, KeyCode.A, KeyCode.Return};
    private static int konamiIndex = 0;

    private void Awake() {
        text.enabled = false;
    }

    private void Update() {
        if(keyTriggered)
            return;

        if(Input.GetKeyDown(konamiCode[konamiIndex])) {
            konamiIndex++;
            if(konamiIndex >= konamiCode.Length)
                EnableDevMode();
        } else if(/*Input.anyKeyDown*/Input.inputString != "") {
            konamiIndex = 0;
        }
    }

    private void EnableDevMode() {
        keyTriggered = true;

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