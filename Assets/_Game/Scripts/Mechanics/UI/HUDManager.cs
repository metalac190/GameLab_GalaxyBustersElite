using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDManager : MonoBehaviour
{
    public TextMeshProUGUI hudScoreText;
    public Slider playerHealthSlider;
    public Slider statBarSlider;
    public Image hudWepImage;


    [Header("Hud Debugging")]
    public float hudScore;
    public float playerHealth = 100;
    public float lowerStatBar = 100;
    public string currentHUDWeapon;

    public GameObject referencedGM;
    private void Start()
    {
        referencedGM = GameObject.Find("Game Manager");
    }
    void Update()
    {
        //update score
        
        if (referencedGM != null)
        {
            hudScore = referencedGM.GetComponent<GameManager>().score;
            hudScoreText.text = hudScore.ToString("00000");

        }
        
        //if(player and gamemanager exist)
        //update health
        //playerHealthSlider.value = playerHealth;
        //update lower stat bar
        statBarSlider.value = lowerStatBar;

        //update weapon icon


    }
}
