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
    //no timestamps
    public bool EnergyWavePickup = false;
    public bool LaserPickup = false;
    public bool FirstBandit = false;
    public bool FirstSpearhead = false;
    public bool FirstRammer = false;
    public bool FirstMinion = false;

    [Header("Level 2 Story Dialogue")]
    public bool StartOfL2Dialogue = false;
    public bool L2MidDialogue = false;
    public bool ZenoxHackingDialogue = false;
    //no timestamps
    public bool CrazyEnvironment = false;
    public bool GiantFormation = false;
    public bool MinefieldSighting = false;
    public bool CubeSighting = false;

    [Header("Level 3 Story Dialogue")]
    public bool StartOfL3Dialogue = false;
    public bool ZenoxRevealDialogue = false;
    public bool ZenoxHalfHealthDialogue = false;
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
            else if (EnergyWavePickup == true)
            {
                DialogueTrigger.TriggerEnergyWavePickupDialogue();
            }
            else if (LaserPickup == true)
            {
                DialogueTrigger.TriggerLaserPickupDialogue();
            }
            else if (FirstBandit == true)
            {
                DialogueTrigger.TriggerFirstBanditDialogue();
            }
            else if (FirstSpearhead == true)
            {
                DialogueTrigger.TriggerFirstSpearheadDialogue();
            }
            else if (FirstRammer == true)
            {
                DialogueTrigger.TriggerFirstRammerDialogue();
            }
            else if (FirstMinion == true)
            {
                DialogueTrigger.TriggerFirstMinionDialogue();
            }
            //L2 Level Story Dialogue
            else if (StartOfL2Dialogue == true)         
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
            else if (CrazyEnvironment == true)
            {
                DialogueTrigger.TriggerCrazyEnvironmentDialogue();
            }
            else if (GiantFormation == true)
            {
                DialogueTrigger.TriggerGiantFormationDialogue();
            }
            else if (MinefieldSighting == true)
            {
                DialogueTrigger.TriggerMinefieldSightingDialogue();
            }
            else if (CubeSighting == true)
            {
                DialogueTrigger.TriggerCubeSightingDialogue();
            }
            //L2 Level Story Dialogue
            else if (StartOfL3Dialogue == true)
            {
                DialogueTrigger.TriggerL3StartDialogue();
            }
            else if (ZenoxRevealDialogue == true)
            {
                DialogueTrigger.TriggerZenoxRevealDialogue();
            }
            else if (ZenoxHalfHealthDialogue == true)
            {
                DialogueTrigger.TriggerZenoxHalfHealthDialogue();
            }
            else if (AfterBossDialogue == true)
            {
                DialogueTrigger.TriggerAfterBossDialogue();
            }
            Destroy(gameObject);
        }
    }
}
