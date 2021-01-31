using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class creditsPage : MonoBehaviour
{
    public GameObject CreditsUI; 
    public GameObject MainMenuUI;

     public void BackButton()
    {
        Debug.Log("Back to previous UI element");
        CreditsUI.SetActive(false);
        MainMenuUI.SetActive(true);
    }
}
