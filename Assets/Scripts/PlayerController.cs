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
    public float rollCooldownTimer = 1f;
    public float projectileCooldownTimer = 0.7f;
    public float wallJumpForce = 5f;
    public float wallJumpAngle = 1f;
    private bool isDashing;
    private int dashDirection;
    public Vector3 offset = new Vector3(.1f, 1, 0);
    private float currentDashTimer;
    private float horizontal;
    private int intHorizontal;

    public bool midWallJump = false;
    public bool wallSliding = false;
    public bool projectileOnCooldown = false;
    public bool isGrounded = false;
    public bool isOnWall = false;
    public bool canWallJump = false;
    public bool wallJumpOnCooldown = false;
    public bool isMoving = false;
    public bool isDoubleJumping = false;
    public bool rollOnCooldown = false;
    public bool doubleJumpUsed = false;
    public bool dashOnCooldown = false;

    public AnimatorControllerScript animControlScript;
    public GameObject projectilePrefab;
    private Rigidbody playerRb;

    public int health = 100;
    public int shieldHealth = 100;
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
       
        //if space is pressed when touching walljumpable wall start wall jumping to the direction of the raycasts hits normal reflection
        if (Input.GetKeyDown(KeyCode.Space) && canWallJump && !isGrounded)
        {
            StartCoroutine("MidWallJump");
            doubleJumpUsed = true;
            Vector3[] Directions = new Vector3[]
            {
                new Vector3(-wallJumpAngle,wallJumpAngle,0),
                new Vector3(wallJumpAngle,wallJumpAngle,0),
            };

            foreach (Vector3 direction in Directions)
            {
                if (Physics.Raycast(transform.position, direction, out RaycastHit hit, 1f))
                {
                    playerRb.velocity = Vector3.zero;
                    playerRb.velocity = direction;
                    playerRb.AddForce(hit.normal * wallJumpForce, ForceMode.Impulse);
                    transform.Rotate(0, 180, 0);
                    break;
                }
            }
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
        if (health <= 0)
        {
            Destroy(gameObject);
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

        //if movements input value is 0 set isMoving boolean to false
        if (horizontal == 0)
        {
            isMoving = false;
        }
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
        //if the player moves during walljump, reset players velocity
        if (isMoving && midWallJump)
        {
            Vector3 vel_x = playerRb.velocity;
            vel_x.x = 0;
            playerRb.velocity = vel_x;
        }
        Vector3 vel_y = playerRb.velocity;
        //if the player is touching jumpalble wall and players velocity is not 0 start wallsliding else not wallslide
        if (vel_y.y != 0 && canWallJump)
        {
            wallSliding = true;
        }
        else
            wallSliding = false;
        if (wallSliding)
        {
            playerRb.drag = 3;
            if (horizontal != 0)
            {
                wallSliding = false;
                playerRb.drag = 0;
            }
        }
        else
            playerRb.drag = 0;
    }
    IEnumerator MidWallJump()
    {
        //walljump timer corountine
        midWallJump = true;
        yield return new WaitForSeconds(0.6f);
        midWallJump = false;
    }
    IEnumerator RollCooldown()
    {
        // roll cooldown coroutine
        rollOnCooldown = true;
        yield return new WaitForSeconds(rollCooldownTimer);
        rollOnCooldown = false;
    }
    IEnumerator DashCooldown()
    {
        //dash cooldown corountine
        dashOnCooldown = true;
        yield return new WaitForSeconds(1);
        dashOnCooldown = false;

    }
    IEnumerator ProjectileCooldown()
    {
        // roll cooldown coroutine
        projectileOnCooldown = true;
        yield return new WaitForSeconds(projectileCooldownTimer);
        projectileOnCooldown = false;
    }

    IEnumerator HealthPower()
    {
        health += 100;
        yield return new WaitForSeconds(duration);
        if (health - 100 < 1) health = 1;
        else health -= 100;
    }
    void spawnProjectile()
    {
        Instantiate(projectilePrefab, transform.position + offset, transform.rotation);
    }

    

    private void OnTriggerEnter(Collider other)
    {
        //checks if players on the ground
        if (other.gameObject.CompareTag("HealthUp"))
        {
            Destroy(other.gameObject);
            StartCoroutine(HealthPower());
        }
        if (other.gameObject.CompareTag("Projectile"))
        {
            health = health - other.gameObject.GetComponent<Projectile>().projectileDamage;
        }
    }






    private void OnCollisionEnter(Collision collision)
    {
        //checks if players on the ground
        if (collision.gameObject.CompareTag("Plane"))
        {
            isGrounded = true;
            isDoubleJumping = false;
            doubleJumpUsed = false;
        }
        //checks if the player is touching a wall
        if (collision.gameObject.CompareTag("Wall"))
        {
            isOnWall = true;
            canWallJump = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        //checks if player is not on the ground
        if (collision.gameObject.CompareTag("Plane"))
        {
            isGrounded = false;
        }
        //checks if player is not touching a wall
        if (collision.gameObject.CompareTag("Wall"))
        {
            isOnWall = false;
            dashOnCooldown = false;
            canWallJump = false;
        }
    }
}
