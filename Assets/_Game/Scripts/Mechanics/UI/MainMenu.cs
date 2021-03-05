using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public string mission1Scene;
    public string mission2Scene;
    public string mission3Scene;
    // Start is called before the first frame update
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

    public void LoadMission2() {
        GameManager.gm.LoadScene(Levels.Mission2);
    }

    public void LoadMission3()
    {
        GameManager.gm.LoadScene(Levels.Mission3);
    }
}
