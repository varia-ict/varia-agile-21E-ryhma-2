using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private GameManager gameManager;

    public float speed = 5.0f;
    private float range = 10f;
    private Vector3 startPos;
    private float distance;
    public float projectileDamage = 25f;

    // Start is called before the first frame update
    void Start()
    {
        //gameManager = GameObject.Find("GameObject").GetComponent<GameManager>();
        startPos = transform.position;
    }

    void Update()
    {
        //  Calculate distance traveled
        distance = Vector3.Distance(startPos, transform.position);
        //  if distance is greather than range limit then destroy self
        if (range < distance || distance < -range)
        {
            Destroy(gameObject);
        }
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }
    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.CompareTag("Player"))
        {
            //  Reduce player Health
            collision.gameObject.GetComponent<PlayerController>().health -= projectileDamage/* * gameManager.difficulty*/;
            Destroy(gameObject);
        }
    }
}
