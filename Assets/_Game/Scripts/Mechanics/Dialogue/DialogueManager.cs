using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DialogueManager : MonoBehaviour
{
    private Queue<string> sentences;
    private Queue<Dialogue> dialogueQueue;
    public float typingDelay = .05f;
    public float speakerTransionDelay = .5f;
    public float conversationEndDelay = 1.5f;

    public GameObject DialoguePopUp;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public GameObject PilotImage;
    public GameObject PiledriveImage;
    public GameObject MatryoshkaImage;
    public GameObject TwilightImage;
    public GameObject ZenoxImage;
    public GameObject RandomImage;

    public GameObject DialoguePopUp_STORY;
    public TextMeshProUGUI nameText_STORY;
    public TextMeshProUGUI dialogueText_STORY;
    public GameObject PilotImage_STORY;
    public GameObject PiledriveImage_STORY;
    public GameObject MatryoshkaImage_STORY;
    public GameObject TwilightImage_STORY;
    public GameObject ZenoxImage_STORY;
    public GameObject RandomImage_STORY;


    private bool next = false;
    public int dialoguePriority = 0;  //0 is not active, 1 is reaction. 2 is dialogue
    public bool activeDialogue = false;

    public event Action<string> onDisplayNextSentence;
    Coroutine textCor;
    bool _forcePauseDialogue = false;
    public bool forcePauseDialogue
	{
        get { return _forcePauseDialogue; }
        set
		{
            _forcePauseDialogue = value;
            if (value == false)
			{
               if (textCor == null) DisplayNextSentence();
            }
		}
	}
    

    void Start()
    {
        sentences = new Queue<string>();
    }
    public void StartDialogue(Dialogue dialogue)
    {
        //add dialogue active/priority check            //reaction dialogue
        if (activeDialogue == false && dialoguePriority == 0)
        {
            activeDialogue = true;
            DialoguePopUp.SetActive(true);
            dialoguePriority = 1;
            nameText.text = dialogue.npcName;
            //set portrait
            switch (nameText.text)
            {
                case "Pilot":
                    PilotImage.SetActive(true);
                    RandomImage.SetActive(false);
                    TwilightImage.SetActive(false);
                    PiledriveImage.SetActive(false);
                    MatryoshkaImage.SetActive(false);
                    ZenoxImage.SetActive(false);
                    break;
                case "Commander Twilight":
                    TwilightImage.SetActive(true);
                    RandomImage.SetActive(false);
                    PilotImage.SetActive(false);
                    PiledriveImage.SetActive(false);
                    MatryoshkaImage.SetActive(false);
                    ZenoxImage.SetActive(false);
                    break;
                case "Piledrive":
                    PiledriveImage.SetActive(true);
                    RandomImage.SetActive(false);
                    TwilightImage.SetActive(false);
                    PilotImage.SetActive(false);
                    MatryoshkaImage.SetActive(false);
                    ZenoxImage.SetActive(false);
                    break;
                case "Zenox":
                    ZenoxImage.SetActive(true);
                    RandomImage.SetActive(false);
                    TwilightImage.SetActive(false);
                    PiledriveImage.SetActive(false);
                    MatryoshkaImage.SetActive(false);
                    PilotImage.SetActive(false);
                    break;
                case "Matryoshka":
                    MatryoshkaImage.SetActive(true);
                    RandomImage.SetActive(false);
                    TwilightImage.SetActive(false);
                    PiledriveImage.SetActive(false);
                    PilotImage.SetActive(false);
                    ZenoxImage.SetActive(false);
                    break;
                default:
                    PilotImage.SetActive(false);
                    RandomImage.SetActive(true);
                    TwilightImage.SetActive(false);
                    PiledriveImage.SetActive(false);
                    MatryoshkaImage.SetActive(false);
                    ZenoxImage.SetActive(false);
                    break;
            }
            sentences.Clear();
            foreach (string sentence in dialogue.sentences)
            {
                sentences.Enqueue(sentence);
            }
            DisplayNextSentence();
        }
    }
    public void startDialogueArrayFunction(Dialogue[] dialogue)
    {
        //add dialogue active/priority check
        if (dialoguePriority < 2)
        {
            StopAllCoroutines();
            dialogueText.text = "";
            dialogueText_STORY.text = "";
            next = false;
            sentences.Clear();         

            activeDialogue = true;
            dialoguePriority = 2;
            StartCoroutine(StartDialogueStepping(dialogue));
        }
    }
    public IEnumerator StartDialogueStepping(Dialogue[] dialogue)
    {
        foreach (Dialogue aDialogue in dialogue)
        {
            yield return StartCoroutine(StartDialogueCoroutine(aDialogue));
            while(next == false)
            {
                yield return new WaitForSeconds(.2f);
            }
            next = false;
            yield return new WaitForSeconds(speakerTransionDelay);
        }
        activeDialogue = false;
        dialoguePriority = 0;
        yield return null;
    }
    public IEnumerator StartDialogueCoroutine(Dialogue dialogue)
    {
        if(dialoguePriority == 1)
        {
            DialoguePopUp.SetActive(true);
            DialoguePopUp_STORY.SetActive(false);
            nameText.text = dialogue.npcName;

        }
        else if (dialoguePriority == 2)
        {
            DialoguePopUp_STORY.SetActive(true);
            DialoguePopUp.SetActive(false);
            nameText_STORY.text = dialogue.npcName;
        }
        sentences.Clear();
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
        yield return null;
    }
    public void DisplayNextSentence() //for reaction dialogue
    {
        if (forcePauseDialogue) return;
        if (sentences.Count == 0)
        {
            EndDialogue();
            if(dialoguePriority == 1)
            {
                activeDialogue = false;
                dialoguePriority = 0;
            }
            next = true;
            onDisplayNextSentence?.Invoke("end");
            return;
        }
        string nextSentence = sentences.Dequeue();
        onDisplayNextSentence?.Invoke(nextSentence);
        //StopAllCoroutines();
        StopCoroutine("TypeSentence");
        textCor = StartCoroutine(TypeSentence(nextSentence));
    }
    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        dialogueText_STORY.text = "";
        if (dialoguePriority == 2)
        {
            switch (nameText_STORY.text)
            {
                case "Pilot":
                    PilotImage_STORY.SetActive(true);
                    RandomImage_STORY.SetActive(false);
                    TwilightImage_STORY.SetActive(false);
                    PiledriveImage_STORY.SetActive(false);
                    MatryoshkaImage_STORY.SetActive(false);
                    ZenoxImage_STORY.SetActive(false);
                    break;
                case "Commander Twilight":
                    TwilightImage_STORY.SetActive(true);
                    RandomImage_STORY.SetActive(false);
                    PilotImage_STORY.SetActive(false);
                    PiledriveImage_STORY.SetActive(false);
                    MatryoshkaImage_STORY.SetActive(false);
                    ZenoxImage_STORY.SetActive(false);
                    break;
                case "Piledrive":
                    PiledriveImage_STORY.SetActive(true);
                    RandomImage_STORY.SetActive(false);
                    TwilightImage_STORY.SetActive(false);
                    PilotImage_STORY.SetActive(false);
                    MatryoshkaImage_STORY.SetActive(false);
                    ZenoxImage_STORY.SetActive(false);
                    break;
                case "Zenox":
                    ZenoxImage_STORY.SetActive(true);
                    RandomImage_STORY.SetActive(false);
                    TwilightImage_STORY.SetActive(false);
                    PiledriveImage_STORY.SetActive(false);
                    MatryoshkaImage_STORY.SetActive(false);
                    PilotImage_STORY.SetActive(false);
                    break;
                case "Matryoshka":
                    MatryoshkaImage_STORY.SetActive(true);
                    RandomImage_STORY.SetActive(false);
                    TwilightImage_STORY.SetActive(false);
                    PiledriveImage_STORY.SetActive(false);
                    PilotImage_STORY.SetActive(false);
                    ZenoxImage_STORY.SetActive(false);
                    break;
                default:
                    PilotImage_STORY.SetActive(false);
                    RandomImage_STORY.SetActive(true);
                    TwilightImage_STORY.SetActive(false);
                    PiledriveImage_STORY.SetActive(false);
                    MatryoshkaImage_STORY.SetActive(false);
                    ZenoxImage_STORY.SetActive(false);
                    break;
            }
        }
        if(dialoguePriority == 2)
        {
            foreach (char letter in sentence.ToCharArray())
            {
                dialogueText_STORY.text += letter;
                yield return new WaitForSeconds(typingDelay);
            }
        }
        else if(dialoguePriority == 1)
        {
            foreach (char letter in sentence.ToCharArray())
            {
                dialogueText.text += letter;
                yield return new WaitForSeconds(typingDelay);
            }
        }

        yield return new WaitForSeconds(conversationEndDelay);

        textCor = null;
        DisplayNextSentence();
    }
    void EndDialogue()
    {
        Debug.Log("End of Conversation.");
        DialoguePopUp.SetActive(false);
        DialoguePopUp_STORY.SetActive(false);
    }


}
