using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//In order for the script to work correctly, enemy object requires an additional collider thar is marked as trigger and functions as the attack range
//Both enemy and player objects have rotation constraints in order to avoid them from falling on collision
public class Enemy : MonoBehaviour
{
    //variables
    public float speed = 5.0f;

    private GameObject player;
    private Rigidbody enemyRb;
    public GameObject projectilePrefab;
    public Transform target;

    private Vector3 offset = new Vector3(.1f, 1, 0);

    private float projectileDelay = 100;
    private float projectileTimer = Time.time;

    public int enemyHealth = 5;
    public int swordDamage = 1;

    public bool playerInRange;
    public bool enemyInvulnerable = false;
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
    }

    void Update()
    {
        if (playerInRange && projectileTimer >= projectileDelay)
        {
            Invoke("spawnProjectile", 0);

            projectileTimer = 0;

            transform.LookAt(player.transform.position, Vector3.forward);

            var point = target.position;
            point.y = transform.position.y;
            transform.LookAt(point);
        }

        if (projectileTimer < projectileDelay)
        {
            projectileTimer++;
        }
    }

    //Checks if player is colliding with enemy attack range
    void OnTriggerEnter(Collider other)
    {
        // checks if "Player" game object entered Enemy's box collider with the trigger on.
        if (other.gameObject.name == "Player")
        {
            playerInRange = true;
        }
    }


    void OnTriggerExit(Collider other)
    {
        // checks if "Player" game object exits Enemy's box collider with the trigger on.
        if (other.gameObject.name == "Player")
        {
            playerInRange = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // checks if collides with Sword tagged object, removes sword damage amount of health, destroys if enemyHealth becomes 0
        if (collision.gameObject.CompareTag("Sword") && !enemyInvulnerable)
        {
            enemyHealth = enemyHealth - swordDamage;
            StartCoroutine(damageCooldown());
            if (enemyHealth <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
    IEnumerator damageCooldown()
    {
        enemyInvulnerable = true;
        yield return new WaitForSeconds(1);
        enemyInvulnerable = false;
    }
    void spawnProjectile()
    {
        Instantiate(projectilePrefab, transform.position + offset, transform.rotation);
        projectileTimer = Time.deltaTime;
        if (projectileTimer >= projectileDelay)
        {
            projectileTimer = 0;
        }
    }
}
