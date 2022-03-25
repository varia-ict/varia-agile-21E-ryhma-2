using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Variables
    public float speed = 10f;
    public float jumpForce = 5f;

    public bool isGrounded = false;
    public Animator playerAnim;

    private Rigidbody playerRb;
    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        //Getting player inputs
        float horizontal = Input.GetAxis("Horizontal");
        //Allows player movement
        transform.Translate(Vector3.forward * horizontal * speed * Time.deltaTime);
        //Allows player to jump is player is on the ground and plays the jump animation
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            playerAnim.Play("Unarmed-Jump");
        }
        // plays the attack animation
        if (isGrounded && Input.GetKeyDown(KeyCode.LeftShift))
        {
            playerAnim.Play("Unarmed-Attack-L1");
            playerAnim.Play("Unarmed-Run-Forward-Attack1-Right");
        }
        if (isGrounded && Input.GetKeyDown(KeyCode.LeftControl))
        {
            playerAnim.Play("Unarmed-DiveRoll-Forward1");
            playerRb.AddForce(Vector3.forward, ForceMode.Impulse);
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        //checks if players on the ground and plays the landing animation if true
        if (collision.gameObject.CompareTag("Plane"))
        {
            isGrounded = true;
            playerAnim.Play("Unarmed-Land");
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        //checks if player is not on the ground and plays the falling animation if true
        if (collision.gameObject.CompareTag("Plane"))
        {
            isGrounded = false;
            playerAnim.Play("Unarmed-Fall");
        }
    }
}
