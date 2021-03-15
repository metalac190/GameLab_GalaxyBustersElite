using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    /*
    public Dialogue dialogue;
    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }
    */
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))          //random player damage test
        {
            TriggerPlayerDamagedDialogue();
        }
    }



    //Static Triggers
    static public void TriggerPlayerDamagedDialogue()
    {
        if (FindObjectOfType<DialogueList>() && FindObjectOfType<DialogueManager>())
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
        else
        {
            Debug.Log("No DialogueListManager or Dialogue Manager in scene.");
        }
    }
}
