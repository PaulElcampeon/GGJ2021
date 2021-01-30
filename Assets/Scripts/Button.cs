using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour, Interactable
{
    private bool _flag;

    void Start()
    {
        
    }

    void Update()
    {
        if (_flag)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Pressed Space");
            }
        }
        else
        {
            _flag = false;
        }
        
    }

    public void Interact()
    {
        _flag = true;
    }
}
