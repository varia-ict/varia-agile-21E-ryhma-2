using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttons : MonoBehaviour
{
    private GameManager gameManager;

    public void setDifficulty(int difficulty)
    {
        gameManager = GameObject.Find("GameObject").GetComponent<GameManager>();
        gameManager.difficulty = difficulty;
    }

    public void startGame()
    {
        gameManager = GameObject.Find("GameObject").GetComponent<GameManager>();
        gameManager.pause = false;
        GameObject.Find("Start").SetActive(false);
    }

    public void continueGame()
    {
        gameManager = GameObject.Find("GameObject").GetComponent<GameManager>();
        gameManager.pause = false;
    }


    //public void restartGame()
    //{
    //    gameManager = GameObject.Find("GameObject").GetComponent<GameManager>();
    //}

}
