using UnityEngine;
using System.Collections;
public class AnimatorController : MonoBehaviour
{
    //Variables
    public bool isGrounded = false;
    public bool isOnWall = false;
    public bool rollOnCooldown = false;
    public bool doubleJumpUsed = false;
    public bool isSheathed = true;
    private Rigidbody playerRb;
    private Animator playerAnim;
    public GameObject backKatana;
    public GameObject handKatana;
    public GameObject swordHitbox;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        //getting player inputs
        float horizontal = Input.GetAxis("Horizontal");
        //plays the jump animation
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            if (isSheathed)
            {
                playerAnim.Play("Unarmed-Jump");
            }
            if (!isSheathed)
            {
                playerAnim.Play("2Hand-Sword-Jump");
            }
        }
        // sheathes and unsheathes the weapon if on the ground
        if (!isSheathed && isGrounded && Input.GetKeyDown(KeyCode.Q))
        {
            playerAnim.Play("2Hand-Sword-Sheath-Back-Unarmed");
            backKatana.SetActive(true);
            handKatana.SetActive(false);
            isSheathed = true;
        }
        if (isSheathed && isGrounded && Input.GetKeyDown(KeyCode.R))
        {
            playerAnim.Play("2Hand-Sword-Unsheath-Back-Unarmed");
            handKatana.SetActive(true);
            backKatana.SetActive(false);
            isSheathed = false;
        }
        // allows player to double jump once before setting the doubleJumpUsed boolean to true, preventing another jump in the air
        if (!isGrounded && !doubleJumpUsed && Input.GetKeyDown(KeyCode.Space))
        {
            if (isSheathed)
            {
                playerAnim.Play("Unarmed-Jump-Flip");
                doubleJumpUsed = true;
            }
            if (!isSheathed)
            {
                playerAnim.Play("2Hand-Sword-Jump-Flip");
                doubleJumpUsed = true;
            }
        }
        // plays the attack animation
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
           
            if (isSheathed)
            {
                playerAnim.Play("Unarmed-Attack-L1");
                playerAnim.Play("Unarmed-Run-Forward-Attack1-Right");
            }
            if (!isSheathed)
            {
                playerAnim.Play("2Hand-Sword-Attack-L1");
                playerAnim.Play("2Hand-Sword-Run-Forward-Attack1-Right");
            }
        }

        // causes the player to roll then activates the roll cooldown coroutine
        if (isGrounded && !rollOnCooldown && Input.GetKeyDown(KeyCode.LeftControl))
        {
            playerAnim.Play("Unarmed-DiveRoll-Forward1");
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        //checks if players on the ground and plays the landing animation if true
        if (collision.gameObject.CompareTag("Plane"))
        {
            isGrounded = true;
            doubleJumpUsed = false;
            if (isSheathed)
            {
                playerAnim.Play("Unarmed-Land");
            }
            if (!isSheathed)
            {
                playerAnim.Play("2Hand-Sword-Land");
            }

        }
    }

    private void OnCollisionExit(Collision collision)
    {
        //checks if player is not on the ground and plays the falling animation if true
        if (collision.gameObject.CompareTag("Plane"))
        {
            isGrounded = false;
            if (isSheathed)
            {
                playerAnim.Play("Unarmed-Fall");
            }
            if (!isSheathed)
            {
                playerAnim.Play("2Hand-Sword-Fall");
            }
        }
    }
}
