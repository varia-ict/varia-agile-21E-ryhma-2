using UnityEngine;
using System.Collections;
public class PlayerController : MonoBehaviour
{
    //Variables
    public float speed = 10f;
    public float jumpForce = 5f;
    public float maxSpeed = 15f;
    public float sprintAcceleration = 4f;
    public float currentSpeed;
    public float dashForce = 5f;
    public float startDashTimer = 0.5f;
    public float dashCooldown = 0f;
    private bool isDashing;
    private int dashDirection;
    public Vector3 offset = new Vector3(.1f, 1, 0);
    private float currentDashTimer;
    private float horizontal;
    private int intHorizontal;

    public bool projectileOnCooldown = false;
    public bool isGrounded = false;
    public bool isOnWall = false;
    public bool rollOnCooldown = false;
    public bool doubleJumpUsed = false;
    public bool dashOnCooldown = false;

    public AnimatorControllerScript animControlScript;
    public GameObject projectilePrefab;
    private Rigidbody playerRb;

    public int health = 100;
    public float duration = 4f;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        currentSpeed = speed;
    }
    // Update is called once per frame
    void Update()
    {
        //Getting player inputs
        float horizontal = Input.GetAxis("Horizontal");

        //allows player movement and rotates the player character based on movement direction
        Vector3 movementDirection = new Vector3(horizontal, 0, 0);
        movementDirection.Normalize();
        if (horizontal != 0)
        {
            transform.forward = movementDirection;
            transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);
        }

        //Allows player to jump is player is on the ground
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        // allows player to double jump once before setting the doubleJumpUsed boolean to true, preventing another jump in the air
        if (!isGrounded && !doubleJumpUsed && Input.GetKeyDown(KeyCode.Space))
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            doubleJumpUsed = true;
        }

        // causes the player to roll then activates the roll cooldown coroutine
        if (isGrounded && !rollOnCooldown && Input.GetKeyDown(KeyCode.LeftControl))
        {
            playerRb.AddForce(Vector3.right * speed, ForceMode.Impulse);
            StartCoroutine("RollCooldown");
        }

        // causes the player to dash if dash is not on cooldown
        if (Input.GetKeyDown(KeyCode.E) && horizontal != 0 && !dashOnCooldown)
        {
            isDashing = true;
            dashOnCooldown = true;
            dashCooldown = +0.5f;
            intHorizontal = Mathf.RoundToInt(horizontal);
            dashDirection = intHorizontal;
            playerRb.velocity = Vector3.zero;
            currentDashTimer = startDashTimer;
        }
        // spawns a projectile
        if (Input.GetKeyDown(KeyCode.LeftAlt) && animControlScript.isSheathed && !projectileOnCooldown)
        {
            Invoke("spawnProjectile", 0);
            StartCoroutine(ProjectileCooldown());
        }

        //applies force to the players movement direction 
        if (isDashing)
        {
            playerRb.velocity = transform.forward * dashForce * dashDirection;
            currentDashTimer -= Time.deltaTime;
            if (currentDashTimer <= 0)
            {
                isDashing = false;
            }
        }

        // todo optimize this by making it a coroutine, use IENumerator and yield return new WaitForSeconds
        if (dashCooldown <= 0)
        {
            dashCooldown = 0;
            dashOnCooldown = false;
        }
        if (dashOnCooldown)
        {
            dashCooldown -= Time.deltaTime;
        }
    }
    private void FixedUpdate()
    {

        //Increases players speed at sprintAccelarations value every frame if sprint is pressed and player is on the ground
        if (Input.GetKey(KeyCode.LeftShift) && isGrounded)
        {
            currentSpeed += sprintAcceleration;
        }

        //Caps the players maximum sprintspeed at maxSpeeds value
        if (currentSpeed > maxSpeed)
        {
            currentSpeed = maxSpeed;
        }

        //When left shift is not pressed decreases players speed by the sprintAccelarations value everyframe
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed -= sprintAcceleration;
        }

        //Caps the players minimum speed at speeds value
        if (currentSpeed < speed)
        {
            currentSpeed = speed;
        }
    }
    IEnumerator RollCooldown()
    {
        // roll cooldown coroutine
        rollOnCooldown = true;
        yield return new WaitForSeconds(1);
        rollOnCooldown = false;
    }

    IEnumerator ProjectileCooldown()
    {
        // roll cooldown coroutine
        projectileOnCooldown = true;
        yield return new WaitForSeconds(0.7f);
        projectileOnCooldown = false;
    }
    void spawnProjectile()
    {
        Instantiate(projectilePrefab, transform.position + offset, transform.rotation);
    }

    IEnumerator HealthPower()
    {
        health += 100;
        yield return new WaitForSeconds(duration);
        if (health - 100 < 1) health = 1;
        else health -= 100;
    }

    private void OnTriggerEnter(Collider other)
    {
        //checks if players on the ground
        if (other.gameObject.CompareTag("HealthUp"))
        {
            Destroy(other.gameObject);
            StartCoroutine(HealthPower());
        }

    }

    




    private void OnCollisionEnter(Collision collision)
    {
        //checks if players on the ground
        if (collision.gameObject.CompareTag("Plane"))
        {
            isGrounded = true;
            doubleJumpUsed = false;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        //checks if player is not on the ground
        if (collision.gameObject.CompareTag("Plane"))
        {
            isGrounded = false;
        }
        if (collision.gameObject.CompareTag("Wall"))
        {
            isOnWall = false;
        }
    }
}
