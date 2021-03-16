using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameManager), true)]
public class GameManagerEditor : Editor {

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        GameManager gm = target as GameManager;
        
        if(GUILayout.Button("Win Game")) {
            gm.WinGame();
        }
        if(GUILayout.Button("Lose Game")) {
            gm.LoseGame();
        }
    }

}
