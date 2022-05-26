using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] audioClips;

    public bool jumpedOnce;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    { 

        GameObject Player = GameObject.FindGameObjectWithTag("Player");
        PlayerController playerScript = Player.GetComponent<PlayerController>();
        //if the player is on the ground and presses space play the jumping audio
        if (playerScript.isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            audioSource.PlayOneShot(audioClips[0]);
            jumpedOnce = true;
            audioSource.Play();
        }
        //if the player is alredy jumped once and presses space play the double jump audio
        if (playerScript.isDoubleJumping && jumpedOnce && Input.GetKey(KeyCode.Space))
        {
            audioSource.PlayOneShot(audioClips[1]);
            jumpedOnce = false;
            audioSource.Play();
        }
        //if the player is next to walljumpable wall and presses space plays the walljump audio
        if (!playerScript.isGrounded && playerScript.canWallJump && Input.GetKeyDown(KeyCode.Space))
        {
            audioSource.Play();
            
            audioSource.PlayOneShot(audioClips[2]);
        }
    }
}
