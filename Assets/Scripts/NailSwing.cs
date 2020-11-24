using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NailSwing : MonoBehaviour
{
    private Rigidbody2D rb;
    public float upThrust = 5;
    public LayerMask spikeLayer;
    public GameObject hitPrefab;
    public bool hitSpike = false;
    public float[] valuesA;
    public float[] valuesB;
    public bool hitDown = false;
    public float hitCooldown = 0.3f;
    public float artTime = 0.2f;
    public AudioSource tinkSound, swingSound;
    Vector2 pointA, pointB;
    Transform upHit, downHit, sideHit;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        upHit = (rb.gameObject.transform.position.x, rb.gameObject.transform.position.y + 0.43f);
        downHit = (rb.gameObject.transform.position.x, rb.gameObject.transform.position.y + 0.43f);
        pointA = new Vector2(rb.gameObject.transform.position.x + valuesA[0], rb.gameObject.transform.position.y + valuesA[1]);
        pointB = new Vector2(rb.gameObject.transform.position.x + valuesB[0], rb.gameObject.transform.position.y + valuesB[1]);
        if (Input.GetKeyDown(KeyCode.Z) && hitDown == false)
        {
            hitSpike = Physics2D.OverlapArea(pointA, pointB, spikeLayer);
            Instantiate(hitPrefab, downHit);
            hitDown = true;
            StartCoroutine("NailCooldown");
            swingSound.Play();
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
        tinkSound.Play();
    }

    private IEnumerator NailCooldown()
    {
        yield return new WaitForSeconds(artTime);
        //hitArt.SetActive(false);
        yield return new WaitForSeconds(hitCooldown - artTime);
        hitDown = false;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawLine(pointA, pointB);
    }
}
