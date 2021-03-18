using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{

    private Queue<string> sentences;
    private Queue<Dialogue> dialogueQueue;
    public GameObject DialoguePopUp;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public Image NPCImage;
    public float typingDelay = .05f;
    public float speakerTransionDelay = .5f;
    public float conversationEndDelay = 1.5f;
    private bool next = false;

    void Start()
    {
        sentences = new Queue<string>();
    }
    public void StartDialogue(Dialogue dialogue)
    {
        DialoguePopUp.SetActive(true);
        nameText.text = dialogue.npcName;
        sentences.Clear();
        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        DisplayNexySentence();
    }
    public void startDialogueArrayFunction(Dialogue[] dialogue)
    {
        //StartCoroutine(StartDialogueArray(dialogue));
        StartCoroutine(StartDialogueStepping(dialogue));
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
        yield return null;
    }
    public IEnumerator StartDialogueCoroutine(Dialogue dialogue)
    {
        DialoguePopUp.SetActive(true);
        nameText.text = dialogue.npcName;
        sentences.Clear();
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        DisplayNexySentence();
        yield return null;
    }
    public void DisplayNexySentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            next = true;
            return;
        }
        string nextSentence = sentences.Dequeue();
        //StopAllCoroutines();
        StopCoroutine("TypeSentence");
        StartCoroutine(TypeSentence(nextSentence));

    }
    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingDelay);
        }
        yield return new WaitForSeconds(conversationEndDelay);
        DisplayNexySentence();
    }
    void EndDialogue()
    {
        Debug.Log("End of Conversation.");
        DialoguePopUp.SetActive(false);
    }


}
