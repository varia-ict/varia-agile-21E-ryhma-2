using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerPos : MonoBehaviour
{
    private GameMaster gm;
    void Start()
    {
        // grabs a reffernce from the game master
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        transform.position = gm.lastCheckPointPos;

    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N)) // need to put player dying condition here
        {
            // loads scene from last checkpoint position
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        }
    }
}
