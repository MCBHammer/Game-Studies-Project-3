using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NailSwing : MonoBehaviour
{
    private Rigidbody2D rb;
    public float upThrust = 5;
    public float pushBack = 5;
    public LayerMask spikeLayer;
    public GameObject hitPrefab, clankPrefab;
    public bool hitSpikeDown = false;
    public bool hitSpikeSide = false;
    public float[] downValuesA;
    public float[] downValuesB;
    public float[] upValuesA;
    public float[] upValuesB;
    public float[] sideValuesA;
    public float[] sideValuesB;
    public bool hitDown = false;
    public float hitCooldown = 0.3f;
    public float artTime = 0.2f;
    public AudioSource tinkSound, swingSound;
    Vector2 downPointA, downPointB;
    Vector2 upPointA, upPointB;
    Vector2 sidePointA, sidePointB;
    Vector2 clankDown, clankSide;
    Vector3 upHit, downHit, sideHit;
    Quaternion upRotate, downRotate, sideRotate, clankDownRotate, clankSideRotate;
    GameObject SwingEffect, clankEffect;
    public Animator animator;
    public BasicMovement movement;
    bool hittingDown = false;
    bool hittingSide = false;
    bool hittingUp = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Set Animator bools
        animator.SetBool("HittingUp", hittingUp);
        animator.SetBool("HittingSide", hittingSide);
        animator.SetBool("HittingDown", hittingDown);

        //Detects if up or down is being held
        float y = Input.GetAxis("Vertical");

        //Transform and Rotation for up and down hit visuals
        upHit = new Vector3(rb.gameObject.transform.position.x, rb.gameObject.transform.position.y + 0.43f, 0);
        upRotate.eulerAngles = new Vector3(0, 0, 270);
        downHit = new Vector3(rb.gameObject.transform.position.x, rb.gameObject.transform.position.y - 0.43f, 0);
        downRotate.eulerAngles = new Vector3(0, 0, 90);

        //Points for up and down hit detection
        downPointA = new Vector2(rb.gameObject.transform.position.x + downValuesA[0], rb.gameObject.transform.position.y + downValuesA[1]);
        downPointB = new Vector2(rb.gameObject.transform.position.x + downValuesB[0], rb.gameObject.transform.position.y + downValuesB[1]);
        upPointA = new Vector2(rb.gameObject.transform.position.x + upValuesA[0], rb.gameObject.transform.position.y + upValuesA[1]);
        upPointB = new Vector2(rb.gameObject.transform.position.x + upValuesB[0], rb.gameObject.transform.position.y + upValuesB[1]);

        //Point and rotation for down clank visual
        clankDown = new Vector2(rb.gameObject.transform.position.x , rb.gameObject.transform.position.y + downValuesB[1]);
        clankDownRotate.eulerAngles = new Vector3(0, 0, 180);

        //Flips nail values if facing left vs facing right
        if (movement.isRight)
        {
            sideHit = new Vector3(rb.gameObject.transform.position.x + 0.43f, rb.gameObject.transform.position.y, 0);
            sideRotate.eulerAngles = new Vector3(0, 0, 180);
            sidePointA = new Vector2(rb.gameObject.transform.position.x + sideValuesA[0], rb.gameObject.transform.position.y + sideValuesA[1]);
            sidePointB = new Vector2(rb.gameObject.transform.position.x + sideValuesB[0], rb.gameObject.transform.position.y + sideValuesB[1]);
            clankSide = new Vector2(rb.gameObject.transform.position.x + sideValuesB[0], rb.gameObject.transform.position.y);
            clankSideRotate.eulerAngles = new Vector3(0, 0, 90);
        } else
        {
            sideHit = new Vector3(rb.gameObject.transform.position.x - 0.43f, rb.gameObject.transform.position.y, 0);
            sideRotate.eulerAngles = new Vector3(0, 0, 180);
            sidePointA = new Vector2(rb.gameObject.transform.position.x - sideValuesA[0], rb.gameObject.transform.position.y + sideValuesA[1]);
            sidePointB = new Vector2(rb.gameObject.transform.position.x - sideValuesB[0], rb.gameObject.transform.position.y + sideValuesB[1]);
            clankSide = new Vector2(rb.gameObject.transform.position.x - sideValuesB[0], rb.gameObject.transform.position.y);
            clankSideRotate.eulerAngles = new Vector3(0, 0, 270);
        }
        
        //If Down input and attacking, but when grounded it performs a side input
        if (Input.GetKeyDown(KeyCode.Z) && hitDown == false && movement.onGround == false && y < 0)
        {
            hitSpikeDown = Physics2D.OverlapArea(downPointA, downPointB, spikeLayer);
            SwingEffect = Instantiate(hitPrefab, downHit, downRotate);
            hitDown = true;
            //hittingDown = true;
            hittingSide = true;
            StartCoroutine("NailCooldown");
            swingSound.Play();
        } else if (Input.GetKeyDown(KeyCode.Z) && hitDown == false && movement.onGround == true && y < 0)
        {
            hitSpikeSide = Physics2D.OverlapArea(sidePointA, sidePointB, spikeLayer);
            SwingEffect = Instantiate(hitPrefab, sideHit, sideRotate);
            if (movement.isRight == false)
            {
                SpriteRenderer hitSprite = SwingEffect.GetComponent<SpriteRenderer>();
                hitSprite.flipX = true;
            }
            hitDown = true;
            hittingSide = true;
            StartCoroutine("NailCooldown");
            swingSound.Play();
        }

        //If Up input and attacking
            if (Input.GetKeyDown(KeyCode.Z) && hitDown == false && y > 0)
        {
            SwingEffect = Instantiate(hitPrefab, upHit, upRotate);
            hitDown = true;
            //hittingUp = true;
            hittingSide = true;
            StartCoroutine("NailCooldown");
            swingSound.Play();
        }

        //If neutral input and attacking
        if (Input.GetKeyDown(KeyCode.Z) && hitDown == false && y == 0)
        {
            hitSpikeSide = Physics2D.OverlapArea(sidePointA, sidePointB, spikeLayer);
            SwingEffect = Instantiate(hitPrefab, sideHit, sideRotate);
            if(movement.isRight == false)
            {
                SpriteRenderer hitSprite = SwingEffect.GetComponent<SpriteRenderer>();
                hitSprite.flipX = true;
            }
            hitDown = true;
            hittingSide = true;
            StartCoroutine("NailCooldown");
            swingSound.Play();
        }

        //If Nail hit something
        if (hitSpikeDown)
        {
            SpikeBounce();
        }
        if (hitSpikeSide)
        {
            SpikeClank();
        }
    }

    //Nail interactions with Spikes
    void SpikeBounce()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity += Vector2.up * upThrust;
        hitSpikeDown = false;
        tinkSound.Play();
        clankEffect = Instantiate(clankPrefab, clankDown, clankDownRotate);
    }

    void SpikeClank()
    {
        rb.velocity += Vector2.left * pushBack;
        hitSpikeSide = false;
        tinkSound.Play();
        clankEffect = Instantiate(clankPrefab, clankSide, clankSideRotate);
    }

    //Cooldown for the Nail hit and art
    private IEnumerator NailCooldown()
    {
        yield return new WaitForSeconds(artTime);
        Destroy(SwingEffect);
        Destroy(clankEffect);
        yield return new WaitForSeconds(hitCooldown - artTime);
        hitDown = false;
        hittingDown = false;
        hittingSide = false;
        hittingUp = false;
    }

    //Gizmos for Testing hitbox sizes
    void OnDrawGizmos()
    {
        Gizmos.DrawLine(downPointA, downPointB);
        Gizmos.DrawLine(upPointA, upPointB);
        Gizmos.DrawLine(sidePointA, sidePointB);
    }
}
