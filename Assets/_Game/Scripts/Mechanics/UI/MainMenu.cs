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

    public void LoadMission2()
    {
        if (mission2Scene == null)
        {
            //SceneManager.LoadScene(mission2Scene);
            Debug.Log("Loading Scene " + mission2Scene);
        }
        else
        {
            Debug.Log("No Scene Name set in MainMenuManager for Mission 2");
        }
    }

    public void LoadMission3()
    {
        if (mission3Scene == null)
        {
            //SceneManager.LoadScene(mission3Scene);
            Debug.Log("Loading Scene " + mission3Scene);
        }
        else
        {
            Debug.Log("No Scene Name set in MainMenuManager for Mission 3");
        }
    }
}
