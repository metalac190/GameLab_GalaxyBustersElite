using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueList : MonoBehaviour
{

    [Header("Story Dialogue")]
    public Dialogue[] Level1StoryDialogue;
    public Dialogue[] Level2StoryDialogue;
    public Dialogue[] Level3StoryDialogue;

    [Header("Reaction Dialogue")]
    public Dialogue[] PlayerDamagedDialogue;
    public Dialogue[] EnemyDefeatedDialogue;

}
