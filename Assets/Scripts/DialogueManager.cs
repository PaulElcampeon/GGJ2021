using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;
    
    private Queue <string> sentences;

    void Start()
    {
        sentences = new Queue<string>();

    }

    public void StartDialogue(Dialogue dialogue)
    {
        //we can trigger dialogue in different ways in here. 

        nameText.text = dialogue.name;
        sentences.Clear(); //clears sentences from previous conversation

        //adds the sentences to a queue
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        //if the sentence count is equal to 0, we end the dialogue
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        dialogueText.text = sentence;
    }

    void EndDialogue()
    {
        SceneManager.LoadScene("EndCredits");
    }
}
