using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public Queue<string> sentences;
    public GameObject DialoguePopUp;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
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
            yield return null;
        }
    }
    void EndDialogue()
    {
        Debug.Log("End of Conversation.");
        DialoguePopUp.SetActive(false);
    }


}
