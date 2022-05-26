using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject startUi;
    public GameObject endUi;
    public GameObject SettingsUi;


    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI HealthText;

    public bool pause;
    public bool gameOver;
    private int score;

    public int difficulty = 1;

    private static GameManager instance;
    public Vector3 lastCheckPointPos;

    private Scene sceneLoaded;

    //  Changes the score variable and the score 
    public int Score{
        get { return score; }   // get method
        set { if (!pause) score = value; scoreText.text = "Score: " + score; }  // set method
    }

    public bool GameOver{
        get { return gameOver; }   // get method
        set {
            pause = value;
            gameOver = value; 
        }  // set method
    }
    private void Awake()
    {
        // checks wheather instance is equal to null, if it is then make this the instance
        if (instance == null)
        {
            instance = this;
            // so that the objects does not destroy between scenes
            // and
            // resets all its information such as variable values
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);// if there is instace it will destroy the game object
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        Scene sceneLoaded = SceneManager.GetActiveScene();
    }

    // Update is called once per frame
    void Update()
    {
        //  When all enemies are dead
        //  load next scene
        if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            SceneManager.LoadScene(sceneLoaded.buildIndex + 1);
        }
        
    }


    public void updateHealth(int health)
    {
        HealthText.text = "Health: " + health.ToString();
    }


    









}
