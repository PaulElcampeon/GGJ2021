using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinTile : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    private bool _isPlayerPresent;

    void Start()
    {

    }

    void Update()
    {
        CheckIfPlayerIsOnTile();
    }

    private void CheckIfPlayerIsOnTile()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < .1f)
        {
            _isPlayerPresent = true;
        }
        else
        {
            _isPlayerPresent = false;

        }
    }

    public bool IsPlayerPresent()
    {
        return _isPlayerPresent;
    }
}
