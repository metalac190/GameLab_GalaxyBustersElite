using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Detect : MonoBehaviour
{
    public float wait_duration;
    [SerializeField] GameObject activate_object;
    bool firstDialogue = false;
    bool finished = false;
    bool waitForMove = false;

    private void OnTriggerEnter(Collider other)
    {
        FindObjectOfType<DialogueManager>().onDisplayNextSentence += NewSentenceDisplayed;

    } 

    private void NewSentenceDisplayed(string sentence)
	{
        print(sentence);
        if (sentence.Contains("Move around the flight path using the keys"))
		{
            // ceheck for keys
            waitForMove = true;

            FindObjectOfType<DialogueManager>().forcePauseDialogue = true;
            FindObjectOfType<DialogueManager>().onDisplayNextSentence -= NewSentenceDisplayed;
        }
	}


    private void Update()
    {
        if (waitForMove)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
            {
                DialogueTrigger.TriggerMoveDialogue();
                activate_object.SetActive(true);
                waitForMove = false;
                FindObjectOfType<DialogueManager>().forcePauseDialogue = false;
            }

        }
    }
}
