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
    private float currentDashTimer;
    private float horizontal;
    private int intHorizontal;

    public bool isGrounded = false;
    public bool isOnWall = false;
    public bool rollOnCooldown = false;
    public bool doubleJumpUsed = false;
    public bool dashOnCooldown = false;

    private Rigidbody playerRb;
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

        if (isOnWall && Input.GetKeyDown(KeyCode.Space))
        {

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
        // todo optimize this by makaing it a coroutine, use IENumerator and yield return new WaitForSeconds
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

    private void OnCollisionEnter(Collision collision)
    {
        //checks if players on the ground
        if (collision.gameObject.CompareTag("Plane"))
        {
            isGrounded = true;
            doubleJumpUsed = false;
        }
        if (collision.gameObject.CompareTag("Wall"))
        {
            isOnWall = true;
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
