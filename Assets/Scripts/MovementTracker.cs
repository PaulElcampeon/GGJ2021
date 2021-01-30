using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementTracker : MonoBehaviour
{
    [SerializeField]
    private int _numberMovesAllowed;

    private bool _hasMovementOccured;

    public static MovementTracker INSTANCE;

    void Start()
    {
        INSTANCE = this;

        MovementTrackerUI.INSTANCE.SetNoOfMoves(_numberMovesAllowed);
    }

    void LateUpdate()
    {
        if (_hasMovementOccured)
        {
            _numberMovesAllowed--;

            Debug.Log("Number of moves allowed " + _numberMovesAllowed);

            MovementTrackerUI.INSTANCE.SetNoOfMoves(_numberMovesAllowed);

            _hasMovementOccured = false;
        }    
    }

    public void EnableMovementHasOccured()
    {
        this._hasMovementOccured = true;
    }
}
