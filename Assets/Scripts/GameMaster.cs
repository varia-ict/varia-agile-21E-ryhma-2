using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    private static GameMaster instance;
    public Vector3 lastCheckPointPos;


    // Called right before start
    void Awake()
    {
        // checks wheather instance is equal to null, if it is then make this the instance
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);// so that the objects does not destroy between scenes and resets all its information such as variable values
                
        }
        else
        {
            Destroy(gameObject);// if there is instace it will destroy the game object
        }
    }
}
