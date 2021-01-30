using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableUI : MonoBehaviour
{
    [SerializeField]
    private GameObject panel;

    private GameObject[] playerGameObjects;

    private Player[] players = new Player[2];

    public static InteractableUI INSTANCE;


    private void Awake()
    {
        INSTANCE = this;    
    }

    void Start()
    {
        playerGameObjects = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < playerGameObjects.Length; i++)
        {
            players[i] = playerGameObjects[i].GetComponent<Player>();
        }
    }

    void Update()
    {
        foreach (Player player in players)
        {
            if (player.IsInteracting())
            {
                ShowDialog();

                return;
            }

            CloseDialog();
        } 
    }

    public void ShowDialog()
    {
        Debug.Log("Showing dialog");

        panel.SetActive(true);
    }

    public void CloseDialog()
    {
        Debug.Log("Closing dialog");

        panel.SetActive(false);
    }
}
