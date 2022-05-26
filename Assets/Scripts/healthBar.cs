using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthBar : MonoBehaviour
{
    private Transform bar;
    private GameObject alivePlayer;
    private PlayerController playerController;
    public float scaledHealth;

    // Start is called before the first frame update
    private void Start()
    {
        bar = transform.Find("Bar");
    }
    void Update()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            scaledHealth = playerController.health / 100;
            bar.localScale = new Vector3(scaledHealth, 1f);
        }
        else if (GameObject.FindGameObjectWithTag("Player"))
        {
            alivePlayer = GameObject.FindGameObjectWithTag("Player");
        }
      

       
    }
}
