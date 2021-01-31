using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{

    public GameObject SettingsMenuUI; 
    public GameObject previousMenuUI; // could be main or pause menu

    public void SetMusicVolume(float volume)
    {
        Debug.Log("Music volume: " + volume);
    }

    public void SetSFXVolume(float volume)
    {
        Debug.Log("SFX volume: " + volume);
    }

    public void SetFullScreen(bool isFullscreen)
    {
        Debug.Log("Toggled fullscreen");
        Screen.fullScreen = isFullscreen;
    }

    public void BackButton()
    {
        Debug.Log("Back to previous UI element");
        SettingsMenuUI.SetActive(false);
        previousMenuUI.SetActive(true);
    }
}
