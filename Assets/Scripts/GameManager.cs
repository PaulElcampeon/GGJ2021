﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityCore.Scene;
using UnityEngine.SceneManagement;
using UnityCore.Audio;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private string _nextLevel;

    private bool _isGameOver = false;

    private bool _isControlsDisabled = false;

    private WinTile[] winTiles;

    public static GameManager INSTANCE;

    private bool _isSceneLoading;

    private SceneController sceneController;

    void Start()
    {
        INSTANCE = this;

        winTiles = GameObject.FindObjectsOfType<WinTile>();

        sceneController = GameObject.FindObjectOfType<SceneController>();
    }

    void Update()
    {
        if (_isGameOver)
        {
            if (_isSceneLoading) return;

            Debug.Log("Game Over");

            _isSceneLoading = true;

        }

        if (CheckIfPlayersAreInWinningTile())
        {
            if (_isSceneLoading) return;

            Debug.Log("Game Won");

            AudioController.instance.PlayAudio(UnityCore.Audio.AudioType.SFX_03, false, 0f);

            sceneController.LoadSimplified(_nextLevel);

            _isSceneLoading = true;
        }
        
    }

    private bool CheckIfPlayersAreInWinningTile()
    {
        if (winTiles.Length == 0) return false;

        foreach(WinTile winTile in winTiles)
        {
            if (!winTile.IsPlayerPresent()) return false;
        }

        return true;
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

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(_nextLevel);
    }
}
