using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueList : MonoBehaviour
{

    [Header("Level 1 Story Dialogue")]
    public Dialogue[] StartOfL1Dialogue;
    public Dialogue[] FirstEnemyAppearsDialogue;
    public Dialogue[] BreachFirstBecomesVisibleDialogue;
    public Dialogue[] Level1EndingDialogue;

    [Header("Level 2 Story Dialogue")]
    public Dialogue[] StartOfL2Dialogue;
    public Dialogue[] L2MidDialogue;
    public Dialogue[] ZenoxHackingDialogue;

    [Header("Level 3 Story Dialogue")]
    public Dialogue[] ZenoxRevealDialogue;
    public Dialogue[] ZenoxFillerDialogue;
    public Dialogue[] AfterBossDialogue;

    [Header("Reaction Dialogue")]
    public Dialogue[] PlayerDamagedDialogue;
    public Dialogue[] EnemyDefeatedDialogue;



}
