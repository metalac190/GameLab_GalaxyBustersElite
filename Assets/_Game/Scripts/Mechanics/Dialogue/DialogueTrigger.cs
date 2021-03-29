using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public static DialogueTrigger instance = new DialogueTrigger();
    public void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.Keypad1))          //random player damage test
        {
            TriggerPlayerDamagedDialogue();
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))          //random player damage test
        {
            TriggerEnemyDefeatedDialogue();
        }
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

        //level 3
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
            int randomChance = Random.Range(1, 8); ///1 in 7 chance 14 percent chance
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
    static public void TriggerEnemyDefeatedDialogue()
    {
        if (FindObjectOfType<DialogueList>() && FindObjectOfType<DialogueManager>())
        {
            int randomChance = Random.Range(1, 8); ///1 in 7 chance 14 percent chance
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
}
