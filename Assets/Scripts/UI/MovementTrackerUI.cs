using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovementTrackerUI : MonoBehaviour
{
    [SerializeField]
    private Text _noOfMovesText;

    public static MovementTrackerUI INSTANCE;

    private void Awake()
    {
        INSTANCE = this;
    }

    void Start()
    {
    }

    void Update()
    {
        
    }

    public void SetNoOfMoves(int movesLeft)
    {
        _noOfMovesText.text = movesLeft.ToString();
    }
}
