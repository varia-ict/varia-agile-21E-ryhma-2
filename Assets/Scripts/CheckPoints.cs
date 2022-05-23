using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoints : MonoBehaviour
{
    private GameMaster gm;// Getting reffernce from game master script
    void Start()
    {
        // game master script is attached to the game object
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
    }
    // this function get called as soon as checkpoints collides with player
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gm.lastCheckPointPos = transform.position;

        }
    }
}
