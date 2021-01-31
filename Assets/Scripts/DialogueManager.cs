using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    private Queue <string> sentences;

    void Start()
    {
        sentences = new Queue<string>();

    }

    public void StartDialogue(Dialogue dialogue)
    {
        //we can trigger dialogue in different ways in here. 
        Debug.Log("Starting conversation with" + dialogue.name);
    }
}
