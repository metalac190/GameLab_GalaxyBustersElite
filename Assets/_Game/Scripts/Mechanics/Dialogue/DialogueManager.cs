using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{

    private Queue<string> sentences;
    private Queue<string> dialogueQueue;
    public GameObject DialoguePopUp;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public Image NPCImage;
    public float typingDelay = .05f;
    public float conversationEndDelay = 1.5f;

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
    public void DisplayNexySentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        string nextSentence = sentences.Dequeue();
        StopAllCoroutines();
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
