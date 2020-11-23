using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NailSwing : MonoBehaviour
{
    private Rigidbody2D rb;
    public float upThrust = 5;
    public LayerMask spikeLayer;
    public GameObject hitArt;
    public bool hitSpike = false;
    public float[] valuesA;
    public float[] valuesB;
    Vector2 pointA, pointB;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        pointA = new Vector2(rb.gameObject.transform.position.x + valuesA[0], rb.gameObject.transform.position.y + valuesA[1]);
        pointB = new Vector2(rb.gameObject.transform.position.x + valuesB[0], rb.gameObject.transform.position.y + valuesB[1]);
        if (Input.GetKeyDown(KeyCode.Z))
        {
            hitSpike = Physics2D.OverlapArea(pointA, pointB, spikeLayer);
            hitArt.SetActive(true);
            //Start enumarator sequences, one to deactivate art and one to allow hitting again
        }
        if (hitSpike)
        {
            SpikeBounce();
        }
    }

    void SpikeBounce()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity += Vector2.up * upThrust;
        hitSpike = false;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawLine(pointA, pointB);
    }
}
