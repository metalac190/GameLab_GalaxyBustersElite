using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public static DialogueTrigger instance = new DialogueTrigger();
    public void Update()
    {/*
        if (Input.GetKeyDown(KeyCode.Keypad1))          //random player damage test
        {
            ZenoxFiller.EnableZenoxFiller();
        }
        
        if (Input.GetKeyDown(KeyCode.Keypad2))          //random player damage test
        {
            ZenoxFiller.DisableZenoxFiller();
        }     
        ZenoxFiller.EnableZenoxFiller();        //call when you set boss active to start filler dialouge
        ZenoxFiller.DisableZenoxFiller();       //call when boss reaches half health
        DialogueTrigger.TriggerZenoxHalfHealthDialogue();           //call on enemy defeat to stop cycle
        DialogueTrigger.TriggerZenoxPartDestroyedDialogue();        //call when a part of the boss is destroyed
        DialogueTrigger.TriggerPlayerObstacleDamamagedDialogue();   //call when player specifically takes damage from obstacles
        */
    }
    
    //StaticStoryTriggers
        //level 1
    static public void TriggerL1StartDialogue()
    {
        if (FindObjectOfType<DialogueList>() && FindObjectOfType<DialogueManager>())
        {
            DialogueList dialogueListInfo = FindObjectOfType<DialogueList>();
            if (dialogueListInfo.StartOfL1Dialogue != null)
            {
                //StartCoroutine(FindObjectOfType<DialogueManager>().StartDialogueArray(dialogueListInfo.StartOfL1Dialogue));
                FindObjectOfType<DialogueManager>().startDialogueArrayFunction(dialogueListInfo.StartOfL1Dialogue);
            }
            else
            {
                Debug.Log("DialogueListManager's StartOfL1Dialogue is empty.");
            }

        }
        else
        {
            Debug.Log("No DialogueListManager or Dialogue Manager in scene.");
        }
    }
    static public void Trigger1stEnemyDialogue()
    {
        if (FindObjectOfType<DialogueList>() && FindObjectOfType<DialogueManager>())
        {
            DialogueList dialogueListInfo = FindObjectOfType<DialogueList>();
            if (dialogueListInfo.FirstEnemyAppearsDialogue != null)
            {
                //StartCoroutine(FindObjectOfType<DialogueManager>().StartDialogueArray(dialogueListInfo.StartOfL1Dialogue));
                FindObjectOfType<DialogueManager>().startDialogueArrayFunction(dialogueListInfo.FirstEnemyAppearsDialogue);
            }
            else
            {
                Debug.Log("DialogueListManager's FirstEnemyAppearsDialogue is empty.");
            }

        }
        else
        {
            Debug.Log("No DialogueListManager or Dialogue Manager in scene.");
        }
    }
    static public void TriggerBreachSeenDialogue()
    {
        if (FindObjectOfType<DialogueList>() && FindObjectOfType<DialogueManager>())
        {
            DialogueList dialogueListInfo = FindObjectOfType<DialogueList>();
            if (dialogueListInfo.BreachFirstBecomesVisibleDialogue != null)
            {
                //StartCoroutine(FindObjectOfType<DialogueManager>().StartDialogueArray(dialogueListInfo.StartOfL1Dialogue));
                FindObjectOfType<DialogueManager>().startDialogueArrayFunction(dialogueListInfo.BreachFirstBecomesVisibleDialogue);
            }
            else
            {
                Debug.Log("DialogueListManager's BreachFirstBecomesVisibleDialogue is empty.");
            }

        }
        else
        {
            Debug.Log("No DialogueListManager or Dialogue Manager in scene.");
        }
    }
    static public void TriggerL1EndDialogue()
    {
        if (FindObjectOfType<DialogueList>() && FindObjectOfType<DialogueManager>())
        {
            DialogueList dialogueListInfo = FindObjectOfType<DialogueList>();
            if (dialogueListInfo.Level1EndingDialogue != null)
            {
                //StartCoroutine(FindObjectOfType<DialogueManager>().StartDialogueArray(dialogueListInfo.StartOfL1Dialogue));
                FindObjectOfType<DialogueManager>().startDialogueArrayFunction(dialogueListInfo.Level1EndingDialogue);
            }
            else
            {
                Debug.Log("DialogueListManager's Level1EndingDialogue is empty.");
            }

        }
        else
        {
            Debug.Log("No DialogueListManager or Dialogue Manager in scene.");
        }
    }
    static public void TriggerEnergyWavePickupDialogue()
    {
        if (FindObjectOfType<DialogueList>() && FindObjectOfType<DialogueManager>())
        {
            DialogueList dialogueListInfo = FindObjectOfType<DialogueList>();
            if (dialogueListInfo.EnergyWavePickup != null)
            {
                //StartCoroutine(FindObjectOfType<DialogueManager>().StartDialogueArray(dialogueListInfo.StartOfL1Dialogue));
                FindObjectOfType<DialogueManager>().startDialogueArrayFunction(dialogueListInfo.EnergyWavePickup);
            }
            else
            {
                Debug.Log("DialogueListManager's EnergyWavePickup is empty.");
            }

        }
        else
        {
            Debug.Log("No DialogueListManager or Dialogue Manager in scene.");
        }
    }
    static public void TriggerLaserPickupDialogue()
    {
        if (FindObjectOfType<DialogueList>() && FindObjectOfType<DialogueManager>())
        {
            DialogueList dialogueListInfo = FindObjectOfType<DialogueList>();
            if (dialogueListInfo.LaserPickup != null)
            {
                FindObjectOfType<DialogueManager>().startDialogueArrayFunction(dialogueListInfo.LaserPickup);
            }
            else
            {
                Debug.Log("DialogueListManager's LaserPickup is empty.");
            }

        }
        else
        {
            Debug.Log("No DialogueListManager or Dialogue Manager in scene.");
        }
    }
    static public void TriggerFirstBanditDialogue()
    {
        if (FindObjectOfType<DialogueList>() && FindObjectOfType<DialogueManager>())
        {
            DialogueList dialogueListInfo = FindObjectOfType<DialogueList>();
            if (dialogueListInfo.FirstBandit != null)
            {
                FindObjectOfType<DialogueManager>().startDialogueArrayFunction(dialogueListInfo.FirstBandit);
            }
            else
            {
                Debug.Log("DialogueListManager's FirstBandit is empty.");
            }

        }
        else
        {
            Debug.Log("No DialogueListManager or Dialogue Manager in scene.");
        }
    }
    static public void TriggerFirstSpearheadDialogue()
    {
        if (FindObjectOfType<DialogueList>() && FindObjectOfType<DialogueManager>())
        {
            DialogueList dialogueListInfo = FindObjectOfType<DialogueList>();
            if (dialogueListInfo.FirstSpearhead != null)
            {
                FindObjectOfType<DialogueManager>().startDialogueArrayFunction(dialogueListInfo.FirstSpearhead);
            }
            else
            {
                Debug.Log("DialogueListManager's FirstSpearhead is empty.");
            }

        }
        else
        {
            Debug.Log("No DialogueListManager or Dialogue Manager in scene.");
        }
    }
    static public void TriggerFirstRammerDialogue()
    {
        if (FindObjectOfType<DialogueList>() && FindObjectOfType<DialogueManager>())
        {
            DialogueList dialogueListInfo = FindObjectOfType<DialogueList>();
            if (dialogueListInfo.FirstRammer != null)
            {
                FindObjectOfType<DialogueManager>().startDialogueArrayFunction(dialogueListInfo.FirstRammer);
            }
            else
            {
                Debug.Log("DialogueListManager's FirstRammer is empty.");
            }

        }
        else
        {
            Debug.Log("No DialogueListManager or Dialogue Manager in scene.");
        }
    }

    static public void TriggerFirstMinionDialogue()
    {
        if (FindObjectOfType<DialogueList>() && FindObjectOfType<DialogueManager>())
        {
            DialogueList dialogueListInfo = FindObjectOfType<DialogueList>();
            if (dialogueListInfo.FirstMinion != null)
            {
                FindObjectOfType<DialogueManager>().startDialogueArrayFunction(dialogueListInfo.FirstMinion);
            }
            else
            {
                Debug.Log("DialogueListManager's FirstMinion is empty.");
            }

        }
        else
        {
            Debug.Log("No DialogueListManager or Dialogue Manager in scene.");
        }
    }

    //level 2
    static public void TriggerL2StartDialogue()
    {
        if (FindObjectOfType<DialogueList>() && FindObjectOfType<DialogueManager>())
        {
            DialogueList dialogueListInfo = FindObjectOfType<DialogueList>();
            if (dialogueListInfo.StartOfL2Dialogue != null)
            {
                //StartCoroutine(FindObjectOfType<DialogueManager>().StartDialogueArray(dialogueListInfo.StartOfL1Dialogue));
                FindObjectOfType<DialogueManager>().startDialogueArrayFunction(dialogueListInfo.StartOfL2Dialogue);
            }
            else
            {
                Debug.Log("DialogueListManager's StartOfL2Dialogue is empty.");
            }

        }
        else
        {
            Debug.Log("No DialogueListManager or Dialogue Manager in scene.");
        }
    }
    static public void TriggerL2MidDialogue()
    {
        if (FindObjectOfType<DialogueList>() && FindObjectOfType<DialogueManager>())
        {
            DialogueList dialogueListInfo = FindObjectOfType<DialogueList>();
            if (dialogueListInfo.L2MidDialogue != null)
            {
                //StartCoroutine(FindObjectOfType<DialogueManager>().StartDialogueArray(dialogueListInfo.StartOfL1Dialogue));
                FindObjectOfType<DialogueManager>().startDialogueArrayFunction(dialogueListInfo.L2MidDialogue);
            }
            else
            {
                Debug.Log("DialogueListManager's L2MidDialogue is empty.");
            }

        }
        else
        {
            Debug.Log("No DialogueListManager or Dialogue Manager in scene.");
        }
    }
    static public void TriggerZenoxHackingDialogue()
    {
        if (FindObjectOfType<DialogueList>() && FindObjectOfType<DialogueManager>())
        {
            DialogueList dialogueListInfo = FindObjectOfType<DialogueList>();
            if (dialogueListInfo.ZenoxHackingDialogue != null)
            {
                //StartCoroutine(FindObjectOfType<DialogueManager>().StartDialogueArray(dialogueListInfo.StartOfL1Dialogue));
                FindObjectOfType<DialogueManager>().startDialogueArrayFunction(dialogueListInfo.ZenoxHackingDialogue);
            }
            else
            {
                Debug.Log("DialogueListManager's ZenoxHackingDialogue is empty.");
            }

        }
        else
        {
            Debug.Log("No DialogueListManager or Dialogue Manager in scene.");
        }
    }
    static public void TriggerCrazyEnvironmentDialogue()
    {
        if (FindObjectOfType<DialogueList>() && FindObjectOfType<DialogueManager>())
        {
            DialogueList dialogueListInfo = FindObjectOfType<DialogueList>();
            if (dialogueListInfo.CrazyEnvironment != null)
            {
                FindObjectOfType<DialogueManager>().startDialogueArrayFunction(dialogueListInfo.CrazyEnvironment);
            }
            else
            {
                Debug.Log("DialogueListManager's CrazyEnvironment is empty.");
            }

        }
        else
        {
            Debug.Log("No DialogueListManager or Dialogue Manager in scene.");
        }
    }
    static public void TriggerGiantFormationDialogue()
    {
        if (FindObjectOfType<DialogueList>() && FindObjectOfType<DialogueManager>())
        {
            DialogueList dialogueListInfo = FindObjectOfType<DialogueList>();
            if (dialogueListInfo.GiantFormation != null)
            {
                FindObjectOfType<DialogueManager>().startDialogueArrayFunction(dialogueListInfo.GiantFormation);
            }
            else
            {
                Debug.Log("DialogueListManager's GiantFormation is empty.");
            }

        }
        else
        {
            Debug.Log("No DialogueListManager or Dialogue Manager in scene.");
        }
    }
    static public void TriggerMinefieldSightingDialogue()
    {
        if (FindObjectOfType<DialogueList>() && FindObjectOfType<DialogueManager>())
        {
            DialogueList dialogueListInfo = FindObjectOfType<DialogueList>();
            if (dialogueListInfo.MinefieldSighting != null)
            {
                FindObjectOfType<DialogueManager>().startDialogueArrayFunction(dialogueListInfo.MinefieldSighting);
            }
            else
            {
                Debug.Log("DialogueListManager's MinefieldSighting is empty.");
            }

        }
        else
        {
            Debug.Log("No DialogueListManager or Dialogue Manager in scene.");
        }
    }
    static public void TriggerCubeSightingDialogue()
    {
        if (FindObjectOfType<DialogueList>() && FindObjectOfType<DialogueManager>())
        {
            DialogueList dialogueListInfo = FindObjectOfType<DialogueList>();
            if (dialogueListInfo.CubeSighting != null)
            {
                FindObjectOfType<DialogueManager>().startDialogueArrayFunction(dialogueListInfo.CubeSighting);
            }
            else
            {
                Debug.Log("DialogueListManager's CubeSighting is empty.");
            }

        }
        else
        {
            Debug.Log("No DialogueListManager or Dialogue Manager in scene.");
        }
    }
    //level 3
    static public void TriggerL3StartDialogue()
    {
        if (FindObjectOfType<DialogueList>() && FindObjectOfType<DialogueManager>())
        {
            DialogueList dialogueListInfo = FindObjectOfType<DialogueList>();
            if (dialogueListInfo.StartOfL3Dialogue != null)
            {
                //StartCoroutine(FindObjectOfType<DialogueManager>().StartDialogueArray(dialogueListInfo.StartOfL1Dialogue));
                FindObjectOfType<DialogueManager>().startDialogueArrayFunction(dialogueListInfo.StartOfL3Dialogue);
            }
            else
            {
                Debug.Log("DialogueListManager's StartOfL3Dialogue is empty.");
            }

        }
        else
        {
            Debug.Log("No DialogueListManager or Dialogue Manager in scene.");
        }
    }
    static public void TriggerZenoxRevealDialogue()
    {
        if (FindObjectOfType<DialogueList>() && FindObjectOfType<DialogueManager>())
        {
            DialogueList dialogueListInfo = FindObjectOfType<DialogueList>();
            if (dialogueListInfo.ZenoxRevealDialogue != null)
            {
                //StartCoroutine(FindObjectOfType<DialogueManager>().StartDialogueArray(dialogueListInfo.StartOfL1Dialogue));
                FindObjectOfType<DialogueManager>().startDialogueArrayFunction(dialogueListInfo.ZenoxRevealDialogue);
            }
            else
            {
                Debug.Log("DialogueListManager's ZenoxRevealDialogue is empty.");
            }

        }
        else
        {
            Debug.Log("No DialogueListManager or Dialogue Manager in scene.");
        }
    }
    static public void TriggerZenoxHalfHealthDialogue()
    {
        if (FindObjectOfType<DialogueList>() && FindObjectOfType<DialogueManager>())
        {
            DialogueList dialogueListInfo = FindObjectOfType<DialogueList>();
            if (dialogueListInfo.ZenoxHalfHealthDialogue != null)
            {
                FindObjectOfType<DialogueManager>().startDialogueArrayFunction(dialogueListInfo.ZenoxHalfHealthDialogue);
            }
            else
            {
                Debug.Log("DialogueListManager's ZenoxHalfHealthDialogue is empty.");
            }

        }
        else
        {
            Debug.Log("No DialogueListManager or Dialogue Manager in scene.");
        }
    }
    static public void TriggerAfterBossDialogue()
    {
        if (FindObjectOfType<DialogueList>() && FindObjectOfType<DialogueManager>())
        {
            DialogueList dialogueListInfo = FindObjectOfType<DialogueList>();
            if (dialogueListInfo.AfterBossDialogue != null)
            {
                //StartCoroutine(FindObjectOfType<DialogueManager>().StartDialogueArray(dialogueListInfo.StartOfL1Dialogue));
                FindObjectOfType<DialogueManager>().startDialogueArrayFunction(dialogueListInfo.AfterBossDialogue);
            }
            else
            {
                Debug.Log("DialogueListManager's AfterBossDialogue is empty.");
            }

        }
        else
        {
            Debug.Log("No DialogueListManager or Dialogue Manager in scene.");
        }
    }


    //Static Triggers
    static public void TriggerPlayerDamagedDialogue()
    {
        if (FindObjectOfType<DialogueList>() && FindObjectOfType<DialogueManager>())
        {
            int randomChance = Random.Range(1, 17); ///1 in 7 chance 14 percent chance
            if (randomChance == 1)
            {
            DialogueList dialogueListInfo = FindObjectOfType<DialogueList>();
            if (dialogueListInfo.PlayerDamagedDialogue != null)
            {
                int randomDialogue = Random.Range(0, dialogueListInfo.PlayerDamagedDialogue.Length);
                FindObjectOfType<DialogueManager>().StartDialogue(dialogueListInfo.PlayerDamagedDialogue[randomDialogue]);
            }
            else
            {
                Debug.Log("DialogueListManager's Player Damage Dialogue List is empty.");
            }
            }
        }
        else
        {
            Debug.Log("No DialogueListManager or Dialogue Manager in scene.");
        }
    }
    static public void TriggerPlayerObstacleDamamagedDialogue()
    {
        if (FindObjectOfType<DialogueList>() && FindObjectOfType<DialogueManager>())
        {
            int randomChance = Random.Range(1, 17); ///1 in 7 chance 14 percent chance
            if (randomChance == 1)
            {
                DialogueList dialogueListInfo = FindObjectOfType<DialogueList>();
                if (dialogueListInfo.PlayerObstacleDamage != null)
                {
                    int randomDialogue = Random.Range(0, dialogueListInfo.PlayerObstacleDamage.Length);
                    FindObjectOfType<DialogueManager>().StartDialogue(dialogueListInfo.PlayerObstacleDamage[randomDialogue]);
                }
                else
                {
                    Debug.Log("DialogueListManager's PlayerObstacleDamage Dialogue List is empty.");
                }
            }
        }
        else
        {
            Debug.Log("No DialogueListManager or Dialogue Manager in scene.");
        }
    }
    static public void TriggerEnemyDefeatedDialogue()
    {
        if (FindObjectOfType<DialogueList>() && FindObjectOfType<DialogueManager>())
        {
            int randomChance = Random.Range(1, 17); ///1 in 7 chance 14 percent chance
            if (randomChance == 1)
            {
            DialogueList dialogueListInfo = FindObjectOfType<DialogueList>();
            if (dialogueListInfo.EnemyDefeatedDialogue != null)
            {
                int randomDialogue = Random.Range(0, dialogueListInfo.EnemyDefeatedDialogue.Length);
                FindObjectOfType<DialogueManager>().StartDialogue(dialogueListInfo.EnemyDefeatedDialogue[randomDialogue]);
            }
            else
            {
                Debug.Log("DialogueListManager's Enemy Defeated Dialogue List is empty.");
            }
            }
        }
        else
        {
            Debug.Log("No DialogueListManager or Dialogue Manager in scene.");
        }
    }


    static public void TriggerZenoxFillerDialogue()
    {
        if (FindObjectOfType<DialogueList>() && FindObjectOfType<DialogueManager>())
        {
                DialogueList dialogueListInfo = FindObjectOfType<DialogueList>();
                if (dialogueListInfo.ZenoxFillerDialogue != null)
                {
                    int randomDialogue = Random.Range(0, dialogueListInfo.ZenoxFillerDialogue.Length);
                    FindObjectOfType<DialogueManager>().StartDialogue(dialogueListInfo.ZenoxFillerDialogue[randomDialogue]);
                }
                else
                {
                    Debug.Log("DialogueListManager's ZenoxFillerDialogue List is empty.");
                }
        }
        else
        {
            Debug.Log("No DialogueListManager or Dialogue Manager in scene.");
        }
    }
    static public void TriggerZenoxPartDestroyedDialogue()
    {
        if (FindObjectOfType<DialogueList>() && FindObjectOfType<DialogueManager>())
        {
            DialogueList dialogueListInfo = FindObjectOfType<DialogueList>();
            if (dialogueListInfo.ZenoxPartDestroyed != null)
            {
                int randomDialogue = Random.Range(0, dialogueListInfo.ZenoxPartDestroyed.Length);
                FindObjectOfType<DialogueManager>().StartDialogue(dialogueListInfo.ZenoxPartDestroyed[randomDialogue]);
            }
            else
            {
                Debug.Log("DialogueListManager's ZenoxPartDestroyed List is empty.");
            }
        }
        else
        {
            Debug.Log("No DialogueListManager or Dialogue Manager in scene.");
        }
    }
}
