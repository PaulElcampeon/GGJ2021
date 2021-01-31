using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PaperNote : MonoBehaviour
{

    [SerializeField]
    private GameObject _player;

    [SerializeField]
    private GameObject _canvas;

    [SerializeField]
    private float _distanceToDetect = 2f;

    private bool _canClick = false;


    private bool _flag = false;

    void Update()
    {
        if (Input.anyKey && _canClick)
        {
            SceneManager.LoadScene("Level1");
            Debug.Log("Loading scene 1");
        }

        if (Vector3.Distance(transform.position, _player.transform.position) <= _distanceToDetect)
        {

            _canvas.SetActive(true);

            if (!_flag)
            {
                StartCoroutine(NoName());
            }
        }
    }

    private IEnumerator NoName()
    {
        _flag = true;

        yield return new WaitForSeconds(2f);

        _canClick = true;
    }
}
