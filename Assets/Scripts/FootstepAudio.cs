using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepAudio : MonoBehaviour
{
    public AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //if player is moving and is on the ground play the walking audio
        GameObject Player = GameObject.Find("Player");
        PlayerController playerScript = Player.GetComponent<PlayerController>();
        {
            if (playerScript.isMoving && !audioSource.isPlaying && playerScript.isGrounded)
            {
                audioSource.Play();
            }
            if (!playerScript.isMoving || !playerScript.isGrounded)
            {
                audioSource.Stop();
            }  
        }
    }
}