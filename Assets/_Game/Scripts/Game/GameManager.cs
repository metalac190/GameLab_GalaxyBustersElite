using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager gm;

    [Header("Overall Game Control")]
    public GameState currentState;
    [Range(1, 3)] public int unlockedLevel = 1;
    [Range(1, 3)] public int currentLevel = 1;
    public static bool devMode = false;

    [Header("Pause Control")]
    [SerializeField] private GameObject pauseMenu;
    private bool _paused;
    private float lastSavedTimeScale;

    [Header("Game Flow")]
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject loseScreen;
    [SerializeField] private Image blackScreen;
    [HideInInspector] public UnityEvent OnEndLevel;

    [Header("Game Stats")]
    public int score;
	public Challenges challenges;

    [Header("Briefing")]
    [SerializeField] private GameObject missionBriefingGO;
    [SerializeField] private GameObject missionBriefing1;
    [SerializeField] private GameObject missionBriefing2;
    [SerializeField] private GameObject missionBriefing3;
    [HideInInspector] public UnityEvent OnBriefingEnd;

    [Header("Player Reference")]
	public static PlayerReferences player = new PlayerReferences();

	public class PlayerReferences
	{
		public GameObject obj;
		public PlayerMovement movement;
		public PlayerController controller;
	}

    [Header("UI Reference")]
    public GameObject HUD;
    public ComicManager comicScreen;

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
            transform.parent = null;
            gm = this;
            DontDestroyOnLoad(gameObject);
        } else
            Destroy(gameObject);
    }

    // ----------------------------------------------------------------------------------------------------

    private void Update() {
        if(Input.GetButtonDown("Pause"))
            Paused = !Paused;

        if(currentState == GameState.Gameplay && Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.K))
            WinGame();
    }

    // ----------------------------------------------------------------------------------------------------

    #region Pause Control

    private void PauseGame() {
        lastSavedTimeScale = Time.timeScale;
        Time.timeScale = 0;

        Cursor.visible = true;
        if (pauseMenu)
        {
            pauseMenu.SetActive(true);
            HUD.SetActive(false);
        }

    }

    public void UnpauseGame() {
        if (pauseMenu)
        {
            pauseMenu.SetActive(false);
            HUD.SetActive(true);
        }
            
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

    private void EndLevel() {
        Paused = false;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        OnEndLevel.Invoke();
    }

    // -----

    public void WinGame() {
        EndLevel();
        currentState = GameState.Win;
        HUD.SetActive(false);
        //winScreen.SetActive(true);
        comicScreen.StartSequence();
    }

    public void EndComicSequence() {
        winScreen.SetActive(true);
    }

    public void SetWinScreen(GameObject winScreen) {
        this.winScreen = winScreen;
    }

    // -----

    public void LoseGame() {
        EndLevel();
        currentState = GameState.Fail;
        HUD.SetActive(false);
        loseScreen.SetActive(true);
    }

    public void SetLoseScreen(GameObject loseScreen) {
        this.loseScreen = loseScreen;
    }

    // -----

    public void SetMissionBriefing(GameObject briefingGO, GameObject briefing1, GameObject briefing2, GameObject briefing3)
    {
        missionBriefingGO = briefingGO;
        missionBriefing1 = briefing1;
        missionBriefing2 = briefing2;
        missionBriefing3 = briefing3;
    }

    public void EndMissionBriefing() {
        currentState = GameState.Gameplay;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        OnBriefingEnd.Invoke();

        Time.timeScale = 1;
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------

    #region Scene Management

    public void LoadScene(string scene) {
        StartCoroutine(LoadSceneCoroutine(scene));
    }

    public void LoadScene(Levels scene) {
        Cursor.visible = true;
        ScoreSystem.ResetScore();
        switch(scene) {
            case Levels.MainMenu:
                if(currentState == GameState.Win)
                    unlockedLevel = Mathf.Max(unlockedLevel, currentLevel + 1);

                currentState = GameState.MainMenu;
                LoadScene("Main Menu");
                currentLevel = 0;
                break;
            case Levels.Mission1:
                currentState = GameState.Briefing;
                LoadScene("Level1_Final");
                currentLevel = 1;
                break;
            case Levels.Mission2:
                currentState = GameState.Briefing;
                unlockedLevel = Mathf.Max(unlockedLevel, 2);
                LoadScene("Level2_Final");
                currentLevel = 2;
                break;
            case Levels.Mission3:
                currentState = GameState.Briefing;
                unlockedLevel = 3;
                LoadScene("Level3_Final");
                currentLevel = 3;
                break;
            default:
                break;
        }
    }

    private IEnumerator LoadSceneCoroutine(string scene) {
        if(MusicPlayer.instance)
            MusicPlayer.instance.FadeOut();

        // Fade to black
        blackScreen.raycastTarget = true;
        float fraction;
        for(float i = 0f; i <= 0.9f; i += Time.unscaledDeltaTime) {
            fraction = i / 0.9f;
            blackScreen.color = new Color32(0, 0, 0, (byte)(255 * fraction));
            yield return null;
        }
        blackScreen.color = new Color32(0, 0, 0, 255);
        yield return new WaitForSecondsRealtime(0.1f);

        // Set variables
        score = 0;
        _paused = false;
        Time.timeScale = 1;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // Load scene
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Single);
        while(!asyncLoad.isDone)
            yield return null;
        yield return null;

        // Fade out
        for(float i = 0f; i <= 0.6f; i += Time.unscaledDeltaTime) {
            fraction = 1 - (i / 0.6f);
            blackScreen.color = new Color32(0, 0, 0, (byte)(255 * fraction));
            yield return null;
        }
        blackScreen.raycastTarget = false;
        blackScreen.color = new Color32(0, 0, 0, 0);
    }

    #endregion


}