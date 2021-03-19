using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDManager : MonoBehaviour
{
    public TextMeshProUGUI hudScoreText;
    public Slider playerHealthSlider;
    public Slider overloadMeterSlider;
    public Image hudWepImage;


    [Header("Hud Debugging")]
    public float hudScore;
    public float playerHealth = 100;
    public float overloadMeter = 100;
    public string currentHUDWeapon;

    public GameObject referencedGM;
    public GameObject referencedPlayer;
    public static void SetHUDActive()
    {
        GameObject.Find("Game Manager").SetActive(true);
    }
    public static void SetHUDDeactive()
    {
        GameObject.Find("Game Manager").SetActive(false);
    }
    public void OnEnable()
    {
        referencedGM = GameObject.Find("Game Manager");
        referencedPlayer = GameObject.FindGameObjectWithTag("Player");
        if (GameManager.gm.currentState == GameState.Gameplay)
        {
            //its good to stay active
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    void Update()
    {
        //update score
        
        if (referencedGM != null)
        {
            hudScore = referencedGM.GetComponent<GameManager>().score;
            hudScoreText.text = hudScore.ToString("00000");
        }
        if (referencedPlayer != null)
        {
            playerHealth = referencedPlayer.GetComponent<PlayerController>().GetPlayerHealth();
            overloadMeter = referencedPlayer.GetComponent<PlayerController>().GetOverloadCharge();
            currentHUDWeapon = referencedPlayer.GetComponent<PlayerController>().GetCurrentWeaponID();
        }

        playerHealthSlider.value = playerHealth;
        overloadMeterSlider.value = overloadMeter;

        //update weapon icon


    }
}
