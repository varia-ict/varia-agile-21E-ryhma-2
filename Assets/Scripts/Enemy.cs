using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//In order for the script to work correctly, enemy object requires an additional box collider thar is marked as trigger and functions as the attack range
//Both enemy and player objects have rotation constraints in order to avoid them from falling on collision
public class Enemy : MonoBehaviour
{
    //variables
    public float speed = 5.0f;
    private GameObject player;
    private Rigidbody enemyRb;
    public bool playerInRange;
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
    }

    //When player collides with enemy attack range, enemy attacks player   
    void Update()
    {
        if (playerInRange == true)
        {
            enemyRb.AddForce((player.transform.position - transform.position) * speed);
        }
    }

    //Checks if player is colliding with enemy attack range
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Player")
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.name == "Player")
        {
            playerInRange = false;
        }
    }




}
