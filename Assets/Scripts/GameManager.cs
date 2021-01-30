using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool _isGameOver = false;

    private bool _isControlsDisabled = false;

    public static GameManager INSTANCE;

    void Start()
    {
        INSTANCE = this;    
    }

    void Update()
    {
        if (_isGameOver)
        {
            Debug.Log("Game over");
        }
        
    }

    public bool IsGameOver()
    {
        return this._isGameOver;
    }

    public void EndGame()
    {
        this._isGameOver = true;

        DisableControls();
    }

    public void DisableControls()
    {
        Debug.Log("Controls disabled");

        this._isControlsDisabled = true;
    }

    public bool IsControlsDisabled()
    {
        return this._isControlsDisabled;
    }
}
