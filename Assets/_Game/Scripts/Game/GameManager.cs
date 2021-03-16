using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager gm;

    [Header("Overall Game Control")]
    public GameState currentState;
    [Range(1, 3)] public int unlockedLevel = 1;

    [Header("Pause Control")]
    [SerializeField] private GameObject pauseMenu;
    private bool _paused;
    private float lastSavedTimeScale;

    [Header("Game Stats")]
    public int score;

    // ----------------------------------------------------------------------------------------------------

    #region Variables

    public bool Paused {
        get {
            return _paused;
        } set {
            if(_paused == value || !CanPause())
                return;

            _paused = value;
            if(_paused)
                PauseGame();
            else
                UnpauseGame();
        }
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------

    private void Awake() {
        // Initialize singleton
        if(gm == null) {
            gm = this;
            DontDestroyOnLoad(gameObject);
        } else
            Destroy(gameObject);
    }

    // ----------------------------------------------------------------------------------------------------

    private void Update() {
        if(Input.GetButtonDown("Pause"))
            Paused = !Paused;
    }

    // ----------------------------------------------------------------------------------------------------

    #region Pause Control

    private void PauseGame() {
        lastSavedTimeScale = Time.timeScale;
        Time.timeScale = 0;

        if(pauseMenu)
            pauseMenu.SetActive(true);
    }

    public void UnpauseGame() {
        if(pauseMenu)
            pauseMenu.SetActive(false);

        Time.timeScale = lastSavedTimeScale;
    }

    private bool CanPause() {
        switch(currentState) {
            case GameState.Gameplay:
            case GameState.Paused:
                return true;
            default:
                return false;
        }
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------

    public void LoadScene(string scene) {
        score = 0;
        Paused = false;
        SceneManager.LoadScene(scene);
    }

    public void LoadScene(Levels scene) {
        switch(scene) {
            case Levels.MainMenu:
                LoadScene("Main Menu");
                break;
            case Levels.Level1:
                LoadScene("Level 1");
                break;
            case Levels.Level2:
                LoadScene("Level 2");
                break;
            case Levels.Level3:
                LoadScene("Level 3");
                break;
            default:
                break;
        }
    }

}