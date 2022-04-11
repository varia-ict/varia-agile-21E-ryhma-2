using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject startUi;
    public GameObject endUi;
    public GameObject SettingsUi;

    public TextMeshProUGUI scoreText;


    private bool gameOver;
    private int score;

    public int Score{
        get { return score; }   // get method
        set { if (GameOver == false) score = value; scoreText.text = "Score: " + score; }  // set method
    }

    public bool GameOver{
        get { return gameOver; }   // get method
        set {
            gameOver = value; 
        }  // set method
    }
    private void Awake()
    {
    }
    // Start is called before the first frame update
    void Start()
    {
        startUi.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
