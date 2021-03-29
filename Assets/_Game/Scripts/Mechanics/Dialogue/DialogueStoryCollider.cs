using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueStoryCollider : MonoBehaviour
{
    [Header("Level 1 Story Dialogue")]
    public bool StartOfL1Dialogue = false;
    public bool FirstEnemyAppearsDialogue = false;
    public bool BreachFirstBecomesVisibleDialogue = false;
    public bool Level1EndingDialogue;

    [Header("Level 2 Story Dialogue")]
    public bool StartOfL2Dialogue = false;
    public bool L2MidDialogue = false;
    public bool ZenoxHackingDialogue = false;

    [Header("Level 3 Story Dialogue")]
    public bool ZenoxRevealDialogue = false;
    public bool AfterBossDialogue = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            //Level 1 Story Dialogue
            if(StartOfL1Dialogue == true)
            {
                DialogueTrigger.TriggerL1StartDialogue();
            }
            else if (FirstEnemyAppearsDialogue == true)
            {
                DialogueTrigger.Trigger1stEnemyDialogue();
            }
            else if (BreachFirstBecomesVisibleDialogue == true)
            {
                DialogueTrigger.TriggerBreachSeenDialogue();
            }
            else if (Level1EndingDialogue == true)
            {
                DialogueTrigger.TriggerL1EndDialogue();
            }
            else if (StartOfL2Dialogue == true)         //L2 Level Story Dialogue
            {
                DialogueTrigger.TriggerL2StartDialogue();
            }
            else if (L2MidDialogue == true)
            {
                DialogueTrigger.TriggerL2MidDialogue();
            }
            else if (ZenoxHackingDialogue == true)
            {
                DialogueTrigger.TriggerZenoxHackingDialogue();
            }
            else if (ZenoxRevealDialogue == true)
            {
                DialogueTrigger.TriggerZenoxRevealDialogue();
            }
            else if (AfterBossDialogue == true)
            {
                DialogueTrigger.TriggerAfterBossDialogue();
            }         
        }
    }
}
