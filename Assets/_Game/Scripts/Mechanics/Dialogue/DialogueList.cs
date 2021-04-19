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
    //no timestamps
    public Dialogue[] EnergyWavePickup;
    public Dialogue[] LaserPickup;
    public Dialogue[] FirstBandit;
    public Dialogue[] FirstSpearhead;
    public Dialogue[] FirstRammer;
    public Dialogue[] FirstMinion;


    [Header("Level 2 Story Dialogue")]
    public Dialogue[] StartOfL2Dialogue;
    public Dialogue[] L2MidDialogue;
    public Dialogue[] ZenoxHackingDialogue;
    //no timestamps
    public Dialogue[] CrazyEnvironment;
    public Dialogue[] GiantFormation;
    public Dialogue[] MinefieldSighting;
    public Dialogue[] CubeSighting;


    [Header("Level 3 Story Dialogue")]
    public Dialogue[] StartOfL3Dialogue;
    public Dialogue[] ZenoxRevealDialogue;
    public Dialogue[] ZenoxHalfHealthDialogue;
    public Dialogue[] AfterBossDialogue;

    [Header("Reaction Dialogue")]
    public Dialogue[] PlayerDamagedDialogue;
    public Dialogue[] EnemyDefeatedDialogue;
    public Dialogue[] ZenoxFillerDialogue;
    public Dialogue[] PlayerObstacleDamage;
    public Dialogue[] ZenoxPartDestroyed;



}
