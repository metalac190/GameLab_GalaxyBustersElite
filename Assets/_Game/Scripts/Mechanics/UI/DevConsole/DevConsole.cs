using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevConsole : MonoBehaviour
{
    private bool isActive = false;
    [SerializeField] GameObject buttons = null;


    private bool isInvincible = false;
    private bool infOverload = false;
    private bool infDodge = false;
    private bool isFF = false;


    void Awake()
    {
        //Find references
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            if (isActive)
            {
                Deactivate();
            }
            else
            {
                Activate();
            }
            isActive = !isActive;
        }
    }


    private void Activate()
    {
        buttons.SetActive(true);
    }
    private void Deactivate()
    {
        buttons.SetActive(false);
    }



    public void ToggleInvincibility()
    {
        isInvincible = !isInvincible;
        Debug.Log("Invincibility: " + isInvincible);
    }

    public void ModifyHP(int mod)
    {
        Debug.Log("HP Change: " + mod);
    }

    public void SpawnEnemy(int enemy)
    {
        switch (enemy)
        {
            case 0:
                Debug.Log("Spawning Drone");
                break;
            case 1:
                Debug.Log("Spawning Minion");
                break;
            case 2:
                Debug.Log("Spawning Bandit");
                break;
            case 3:
                Debug.Log("Spawning Spearhead");
                break;
            case 4:
                Debug.Log("Spawning Minelayer");
                break;
            case 5:
                Debug.Log("Spawning Tank");
                break;
        }
    }

    public void SetLevel(string level)
    {
        Debug.Log("Entering level: " + level);
    }

    public void SetWeapon(int weapon)
    {
        switch (weapon)
        {
            case 0:
                Debug.Log("Swapping to Blaster");
                break;
            case 1:
                Debug.Log("Swapping to Energy Burst");
                break;
            case 2:
                Debug.Log("Swapping to Laser");
                break;
        }
    }

    public void ToggleInfOverload()
    {
        infOverload = !infOverload;
        Debug.Log("Permanent overload: " + infOverload);
    }

    public void ToggleInfDodge()
    {
        infDodge = !infDodge;
        Debug.Log("Infinite dodge: " + infDodge);
    }

    public void SelfDestruct()
    {
        Debug.Log("Self-destruct initiated");
    }

    public void ModifyScore(int mod)
    {
        Debug.Log("Score Change: " + mod);
    }

    public void ClearEnemies()
    {
        Debug.Log("Destroying all enemies");
    }

    public void ToggleFastForward()
    {
        isFF = !isFF;
        Debug.Log(((isFF)?"2.0x":"1.0x")+" speed");
    }

}
