using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10.0f;

    private float range = 10f;
    private Vector3 startPos;
    private float distance;
    private Rigidbody playerRb;

    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        playerRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(startPos, transform.position);
        if( range < distance || distance < -range)
        {
            Destroy(gameObject);
        }
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }
}
