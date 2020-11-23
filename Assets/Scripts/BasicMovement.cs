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
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        onGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, collisionRadius, groundLayer);
        if(onGround == true && landed == false && !landSound.isPlaying)
        {
            landed = true;
            landSound.Play();
        }
        if(rb.velocity.y < 0 && onGround == false)
        {
            landed = false;
        }

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector2 movement = new Vector2(x, y);

        if(rb.velocity.x != 0 && onGround == true && !moveSound.isPlaying)
        {
            moveSound.Play();
        } else if (rb.velocity.x == 0 && onGround == true)
        {
            moveSound.Stop();
        }

        if (Input.GetButtonDown("Jump") && onGround)
        {
            Jump();
        }

        Walk(movement);
    }

    void OnDrawGizmos()
    {
        //Gizmos.Color = Color.Red;
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
    }
}
