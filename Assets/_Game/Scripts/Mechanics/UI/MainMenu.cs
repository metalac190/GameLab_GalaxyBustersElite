using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class MainMenu : MonoBehaviour
{

    private static bool splashScreenDisplayed = false;
    public GameObject splashScreen;

    [SerializeField] private Button mission2, mission3;

    [Header("Hover Options Button")]
    [SerializeField] Button[] optionsButtons;
    [SerializeField] Vector3 hoverScale;
    [SerializeField] float hoverTransitonDuration;
    Coroutine hoverCoroutine;

    [Header("Player Movement UI")]
    [SerializeField] Image menuBackgroundImage;
    [SerializeField] Sprite[] menuBackgroundSprites;
    [SerializeField] CinemachineDollyCart player;
    [SerializeField] GameObject optionsGameObj;
    [SerializeField] GameObject[] optionsGroup;
    [SerializeField] GameObject settingsBox;
    [SerializeField] Button movementButton;
    [SerializeField] GameObject playerMovementUIGroup;

    private void Awake() {
        // Unlocked levels
        if(GameManager.gm.unlockedLevel < 2)
            mission2.interactable = false;
        if(GameManager.gm.unlockedLevel < 3)
            mission3.interactable = false;

        // Splash screen
        if(splashScreenDisplayed)
            splashScreen.SetActive(false);
        else {
            splashScreen.SetActive(true);
            splashScreenDisplayed = true;
            GameManager.gm.currentLevel = 0;
        }
    }

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
        GameManager.gm.LoadScene(Levels.Mission2);
    }

    public void LoadMission3()
    {
        GameManager.gm.LoadScene(Levels.Mission3);
    }

    // Enable level select
    public void UnlockLevels(bool level2 = true, bool level3 = true) {
        if(level2)
            mission2.interactable = true;
        if(level3)
            mission3.interactable = true;
    }

    public void HoverOptionsButton(int index)
    {
        hoverCoroutine = StartCoroutine(HoverOptionsButtonCoroutine(index));
    }

    IEnumerator HoverOptionsButtonCoroutine(int index)
    {
        float currentTime = 0;
        Vector3 originalScale = optionsButtons[index].transform.localScale;

        while (currentTime < hoverTransitonDuration)
        {
            optionsButtons[index].transform.localScale = Vector3.Lerp(originalScale, hoverScale, currentTime / hoverTransitonDuration);
            currentTime += Time.deltaTime;
            yield return null;
        }
    }

    public void UnHoverOptionsButton(int index)
    {
        StopCoroutine(hoverCoroutine);
        optionsButtons[index].transform.localScale = new Vector3(1, 1, 1);
    }

    public void ShowPlayerMovementControlSettings(bool show)
    {
        if (show)
        {
            player.m_Position = 0;

            menuBackgroundImage.sprite = menuBackgroundSprites[0];

            foreach (GameObject o in optionsGroup)
            {
                o.SetActive(false);
            }

            // movement group
            optionsGroup[0].SetActive(true);
            playerMovementUIGroup.SetActive(true);

            StartCoroutine(MovementButtonSelected());

            settingsBox.SetActive(false);
        }
        else
        {
            menuBackgroundImage.sprite = menuBackgroundSprites[1];

            // movement group
            optionsGroup[0].SetActive(false);
            playerMovementUIGroup.SetActive(false);

            settingsBox.SetActive(true);
        }
    }

    // workaround to selecting movement button visually
    IEnumerator MovementButtonSelected()
    {
        yield return null;
        movementButton.Select();
    }

    public void ShowOptionsGroup(bool show)
    {
        if (show)
        {
            optionsGameObj.SetActive(true);
        }
        else
        {
            optionsGameObj.SetActive(false);

            foreach (GameObject o in optionsGroup)
            {
                o.SetActive(false);
            }
        }
    }
}
