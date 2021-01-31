using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityCore.Audio;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject settings;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            AudioController.instance.PlayAudio(UnityCore.Audio.AudioType.SFX_01, false, 0f);

            if (!gameIsPaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
    }

    void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        settings.SetActive(false); // this may be temporary
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    public void ResetLevel()
    {
        Debug.Log("Reloading level...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }

    public void LoadMainMenu()
    {
        Debug.Log("Loading main menu...");
        SceneManager.LoadScene(0);// Menu is scene 0 in build settings
        Time.timeScale = 1f;
    }

    public void LoadSettings(){
        Debug.Log("Opened settings...");
        pauseMenuUI.SetActive(false);
        settings.SetActive(true);
    }

    public void QuitGame()
    {
        Debug.Log("Qutting game...");
        Application.Quit();
    }
}
