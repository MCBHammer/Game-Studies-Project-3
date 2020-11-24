using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMovement : MonoBehaviour
{

    private Rigidbody2D rb;
    [SerializeField] float speed = 10;
    [SerializeField] float jumpForce = 3;
    public Vector2 bottomOffset;
    public float collisionRadius = 0.5f;
    public LayerMask groundLayer;
    public bool onGround;
    public bool landed = false;
    public AudioSource jumpSound, landSound, moveSound;
    public Animator animator;
    bool isRight = true;
    bool isLeft = false;
    //bool isJumping = false;
    bool isFalling = true;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Animation Parameters
        animator.SetFloat("xVelocity", rb.velocity.x);
        animator.SetBool("Grounded", onGround);
        //animator.SetBool("Jumping", isJumping);
        animator.SetBool("Falling", isFalling);

        //Ground Detection
        onGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, collisionRadius, groundLayer);

        //Landing Detection
        if(onGround == true && landed == false && !landSound.isPlaying)
        {
            landed = true;
            landSound.Play();
        }
        if(rb.velocity.y < 0 && onGround == false)
        {
            landed = false;
        }

        //Movement Vector Setup
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector2 movement = new Vector2(x, y);

        //Movement Detection for Sound
        if(rb.velocity.x != 0 && onGround == true && !moveSound.isPlaying)
        {
            moveSound.Play();
        } else if (rb.velocity.x == 0 && onGround == true)
        {
            moveSound.Stop();
        }

        //Jump Detector
        if (Input.GetButtonDown("Jump") && onGround)
        {
            Jump();
        }

        //Falling and Jumping Detectors for Animations
        if(onGround == false)
        {
            isFalling = true;
        } else if (onGround == true)
        {
            isFalling = false;
        }

        /*
        if(isJumping == true)
        {
            StartCoroutine("JumpAnim");
        }
        */

        //Turning Detector
        if (x > 0 && isRight == false)
        {
            isRight = true;
            isLeft = false;
        }
        if (x < 0 && isLeft == false)
        {
            isLeft = true;
            isRight = false;
        }
        if (isRight == true && isLeft == false)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        if (isRight == false && isLeft == true)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }

        //Walk
        Walk(movement);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, collisionRadius);
    }

    private void Walk(Vector2 movement)
    {
        rb.velocity = new Vector2(movement.x * speed, rb.velocity.y);
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity += Vector2.up * jumpForce;
        jumpSound.Play();
        //isJumping = true;
    }

    /*
    private IEnumerator JumpAnim()
    {
        yield return new WaitForSeconds(0.000005f);
        isJumping = false;
    }
    */
}
