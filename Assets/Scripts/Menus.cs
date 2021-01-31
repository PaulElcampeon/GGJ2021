using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menus : MonoBehaviour
{

    /* Will likely delete this as PauseMenu has all the same stuff - 
    I wanted to try and put all the menus in one place, thinking they'd
    have a lot of the same functionality...*/
    public static bool gameIsPaused = false;
    public GameObject mainMenuUI;

    public GameObject settings;

    public GameObject credits;

    public void LoadCredits(){
        Debug.Log("Roll credits");
        mainMenuUI.SetActive(false);
        credits.SetActive(true);
    }

    public void LoadSettings(){
        // would have been better to name it 'DisplaySettings'
        Debug.Log("Opened settings...");
        mainMenuUI.SetActive(false);
        settings.SetActive(true);
    }

    public void QuitGame()
    {
        Debug.Log("Qutting game...");
        Application.Quit();
    }
}
