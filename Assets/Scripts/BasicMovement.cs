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
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        onGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, collisionRadius, groundLayer);

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector2 movement = new Vector2(x, y);

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
    }
}
