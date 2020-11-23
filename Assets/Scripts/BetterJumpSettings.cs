using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterJumpSettings : MonoBehaviour
{

    public float fallModifier = 3;
    public float lowJumpModifier = 2;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallModifier - 1) * Time.deltaTime;
        } else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpModifier - 1) * Time.deltaTime;
        }
    }
}
