using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class FollowPlayer : MonoBehaviour
{

    public GameObject playerPrefab;
    private GameObject currentPlayer;
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        // find object with tag
        currentPlayer = GameObject.FindGameObjectWithTag("Player");
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {

        //follows the player on an offset
        if(currentPlayer != null)
        {
            transform.position = currentPlayer.transform.position + new Vector3(0, 4, -10);
        }
        else if (GameObject.FindGameObjectWithTag("Player"))
        {
            currentPlayer = GameObject.FindGameObjectWithTag("Player");
        }
        //  Spawns a player
        else
        {
            Instantiate(playerPrefab, gameManager.lastCheckPointPos, Quaternion.identity);
        }
    }
}
