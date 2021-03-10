using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager gm;

    [Header("Overall Game Control")]
    public GameState currentState;
    [Range(1, 3)] public int unlockedLevel = 1;
    public int currentLevel = 0;

    [Header("Pause Control")]
    [SerializeField] private GameObject pauseMenu;
    private bool _paused;
    private float lastSavedTimeScale;

    [Header("Game Flow")]
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject loseScreen;

    [Header("Game Stats")]
    public int score;

    [Header("Briefing")]
    [SerializeField] private GameObject missionBriefingGO;
    [SerializeField] private GameObject missionBriefing1;
    [SerializeField] private GameObject missionBriefing2;
    [SerializeField] private GameObject missionBriefing3;

    [Header("Player Reference")]
	public static PlayerReferences player = new PlayerReferences();

	public class PlayerReferences
	{
		public GameObject obj;
		public PlayerMovement movement;
		public PlayerController controller;
	}

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

        Cursor.visible = true;
        if(pauseMenu)
            pauseMenu.SetActive(true);
    }

    public void UnpauseGame() {
        if(pauseMenu)
            pauseMenu.SetActive(false);
        Cursor.visible = false;

        Time.timeScale = lastSavedTimeScale;
    }

    private bool CanPause() {
        return currentState == GameState.Gameplay || currentState == GameState.Paused;
    }

    public void SetPauseMenu(GameObject pauseMenu) {
        this.pauseMenu = pauseMenu;
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------

    #region Game Flow

    public void WinGame() {
        Paused = false;
        Time.timeScale = 0;
        currentState = GameState.Win;
        Cursor.visible = true;
        winScreen.SetActive(true);
    }

    public void SetWinScreen(GameObject winScreen) {
        this.winScreen = winScreen;
    }

    public void LoseGame() {
        // TODO
    }

    public void SetLoseScreen(GameObject loseScreen) {
        this.loseScreen = loseScreen;
    }

    public void SetMissionBriefing(GameObject briefingGO, GameObject briefing1, GameObject briefing2, GameObject briefing3)
    {
        this.missionBriefingGO = briefingGO;
        this.missionBriefing1 = briefing1;
        this.missionBriefing2 = briefing2;
        this.missionBriefing3 = briefing3;
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------

    #region Scene Management

    public void LoadScene(string scene) {
        score = 0;
        Paused = false;
        Time.timeScale = 1;
        SceneManager.LoadScene(scene);
    }

    public void LoadScene(Levels scene) {
        Cursor.visible = true;
        switch(scene) {
            case Levels.MainMenu:
                currentState = GameState.MainMenu;
                Cursor.lockState = CursorLockMode.None;
                LoadScene("Main Menu");
                currentLevel = 0;
                break;
            case Levels.Mission1:
                currentState = GameState.Gameplay;
                LoadScene("Pre-Alpha");
                //LoadScene("Mission 1");
                currentLevel = 1;
                break;
            case Levels.Mission2:
                currentState = GameState.Gameplay;
                unlockedLevel = Mathf.Max(unlockedLevel, 2);
                LoadScene("Mission 2");
                currentLevel = 2;
                break;
            case Levels.Mission3:
                currentState = GameState.Gameplay;
                unlockedLevel = 3;
                LoadScene("Mission 3");
                currentLevel = 3;
                break;
            default:
                break;
        }
    }

    #endregion


}