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
    bool footstepsPlaying = false;
    public AudioSource moveSounds;
    AudioClip jumpSound, moveSound, landSound;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpSound = Resources.Load<AudioClip>("Sounds/Jump");
        moveSound = Resources.Load<AudioClip>("Sounds/WalkFootsteps");
        landSound = Resources.Load<AudioClip>("Sounds/Land");
    }

    // Update is called once per frame
    void Update()
    {
        onGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, collisionRadius, groundLayer);
        if(onGround == true && landed == false)
        {
            moveSounds.clip = landSound;
            moveSounds.Play();
            landed = true;
        }
        if(rb.velocity.y < 0)
        {
            landed = false;
        }

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector2 movement = new Vector2(x, y);

        if(rb.velocity.x != 0 && onGround == true && footstepsPlaying == false)
        {
            moveSounds.clip = moveSound;
            moveSounds.Play();
            footstepsPlaying = true;
        } else if (rb.velocity.x == 0 && onGround == true)
        {
            moveSounds.Stop();
            footstepsPlaying = false;
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
        moveSounds.clip = jumpSound;
        moveSounds.Play();
    }
}
