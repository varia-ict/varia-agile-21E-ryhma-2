using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour

{
    public bool shieldOnCooldown = false;
    public float shieldCooldownTimer = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // blocking feature that does not work yet
        if (Input.GetKeyDown(KeyCode.Z))
        {
            gameObject.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.Z))
        {
            gameObject.SetActive(false);
        }
        if (gameObject.GetComponent<PlayerController>().shieldHealth <= 0)
        {
            StartCoroutine("ShieldStun");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Projectile"))
        {
            gameObject.GetComponent<PlayerController>().shieldHealth -= other.gameObject.GetComponent<Projectile>().projectileDamage;
            Debug.Log("SHIELD HIT!");
        }
    }

    IEnumerator ShieldStun()
    {
        shieldOnCooldown = true;
        yield return new WaitForSecondsRealtime(shieldCooldownTimer);
        shieldOnCooldown = false;
        Debug.Log("Shield On Cooldown!");
    }
}
