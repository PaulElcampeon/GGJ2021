using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnPlayerDeath : MonoBehaviour
{

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Initialize()
    {
        GetComponentInParent<Player>().Dead();
    }
}
