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


    
    void Update()
    {
        if(!GameManager.devMode && !isActive)
            return;

        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            if (isActive)
            {
                Deactivate();
                Cursor.visible = false;
            }
            else
            {
                Activate();
                Cursor.visible = true;
            }
            isActive = !isActive;
        }

        if (infOverload)
        {
            GameManager.player.controller.SetOverload(100);
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
        GameManager.player.controller.isInvincible = isInvincible;
    }
    public void ToggleInvincibility(bool inv)
    {
        isInvincible = inv;
        GameManager.player.controller.isInvincible = isInvincible;
    }

    public void ModifyHP(int mod)
    {
        Debug.Log("HP Change: " + mod);
        if (mod >= 0)
        {
            GameManager.player.controller.HealPlayer(mod);
        }
        else
        {
            GameManager.player.controller.DamagePlayer(-mod);
        }
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
        GameManager.gm.LoadScene(level);
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
        GameManager.player.controller.SetWeapon(GameManager.player.controller.weapons[weapon]);
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
        GameManager.player.movement.infiniteDodge = infDodge;
    }

    public void SelfDestruct()
    {
        Debug.Log("Self-destruct initiated");
        GameManager.player.controller.OnDeath.Invoke();
        GameManager.gm.LoseGame();
    }

    public void ModifyScore(int mod)
    {
        Debug.Log("Score Change: " + mod);
        GameManager.gm.score += mod;
    }

    public void ClearEnemies()
    {
        Debug.Log("Destroying all enemies");
    }

    public void ToggleFastForward()
    {
        isFF = !isFF;
        Debug.Log(((isFF)?"5.0x":"1.0x")+" speed");
        Time.timeScale = isFF ? 5f : 1f;
        ToggleInvincibility(isFF); //If fast-forwarding, become invincible so you don't die along the way
    }

}
