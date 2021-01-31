using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTile : MonoBehaviour
{
    [SerializeField]
    private GameObject _playerToDetect;

    [SerializeField]
    private GameObject _objectToModify;

    void Start()
    {

    }

    void Update()
    {
        CheckIfPlayerIsOnTile();
    }

    private void CheckIfPlayerIsOnTile()
    {
        if (Vector3.Distance(transform.position, _playerToDetect.transform.position) < .2f)
        {
            Debug.Log("Player Detected on Button Tile");

            Destroy(_objectToModify);
        }
    }
}
