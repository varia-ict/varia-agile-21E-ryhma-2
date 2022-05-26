using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoints : MonoBehaviour
{
    private GameManager gameManager;// Getting reffernce from game master script
    void Start()
    {
        // game master script is attached to the game object
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // this function get called as soon as checkpoints collides with player
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //  save the player position in the game manager
            gameManager.lastCheckPointPos = other.transform.position;

        }
    }
}
