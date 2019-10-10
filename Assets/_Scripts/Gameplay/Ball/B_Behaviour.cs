using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_Behaviour : MonoBehaviour
{
    private Collider col;
    private Rigidbody rb;
    private bool ballHeld;
    [SerializeField]private float dropForce;
    [SerializeField]private float dropUpForce, dashDropForce, bulletForce;
    public bool free;
    public Rigidbody heldBy;
    private float ballThrowCooldown = 0.025f;
    public float magnitude, prePauseMagnitude;
    [SerializeField]private float stunCooldown = 0.1f;
    private Vector3 prePauseDirection;
    

    private void Start()
    {
        SetupBall();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (rb.IsSleeping())
        {
            rb.WakeUp();
            UnfreezeAllRigidbodyConstraints();
        }
        else
        {
            magnitude = rb.velocity.magnitude;
        }

        if (collision.gameObject.tag == "player" && free)
        {
            collision.gameObject.GetComponentInParent<CH_BallInteractions>().PickUpBall(gameObject, this);
        }        
        else if(collision.gameObject.tag == "bullet")
        {
            rb.AddForce(rb.velocity + (collision.gameObject.GetComponent<Gun_Bullet>().direction.normalized * bulletForce), ForceMode.VelocityChange);
            collision.gameObject.GetComponent<Gun_Bullet>().DestroyBullet();
        }
    }   

    public void SetupBall()
    {
        ballHeld = false;
        col = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        rb.Sleep();
        //FreezeAllRigidbodyConstraints();
        free = true;
    }

    public void HitBall(Transform playerT, float ballHitStrength)
    {
        if (!ballHeld)
        {
            Debug.Log("prepausemagnitude = " + prePauseMagnitude);
            Debug.Log("magnitude = " + magnitude);
            if(magnitude < 20)
            {                
                rb.velocity = playerT.forward * 30;
                //rb.AddForce(playerT.forward * 1500, ForceMode.Acceleration);
            }
            else
            {
                rb.velocity = playerT.forward * magnitude * ballHitStrength;
                //rb.AddForce(playerT.forward * magnitude * ballHitStrength, ForceMode.Acceleration);
            }            
        }        
    }

    public void BallPickedUp(Rigidbody r)
    {
        ballHeld = true;
        FreezeAllRigidbodyConstraints();
        free = false;
        heldBy = r;
        col.enabled = false;
    }

    public void BallThrown()
    {
        ballHeld = false;
        UnfreezeAllRigidbodyConstraints();
        free = true;
        col.enabled = true;
        StartCoroutine("ThrowCooldown");
    }

    private IEnumerator ThrowCooldown()
    {
        yield return new WaitForSeconds(ballThrowCooldown);
        heldBy = null;
    }

    public void PauseBallSwing()
    {
        prePauseMagnitude = rb.velocity.magnitude;
        prePauseDirection = rb.velocity.normalized;        
        FreezeAllRigidbodyConstraints();
        free = false;
    }

    public void WakeBallSwing()
    {
        UnfreezeAllRigidbodyConstraints();
        rb.velocity = prePauseDirection * prePauseMagnitude;
        free = true;
    }

    public void BallDroppedBullet(Vector3 hitDirection)
    {
        ballHeld = false;
        transform.parent = null;
        UnfreezeAllRigidbodyConstraints();
        free = true;
        col.enabled = true;

        Vector3 stunnedDirection = new Vector3(hitDirection.x, 1, hitDirection.z).normalized;
        
        stunnedDirection *= -1;
        stunnedDirection.x *= dropForce;
        stunnedDirection.z *= dropForce;
        stunnedDirection.y *= dropUpForce;

        rb.AddForce(stunnedDirection, ForceMode.Acceleration);
        StartCoroutine("StunCooldown");
    }

    public void BallDroppedBat(Vector3 hitDirection)
    {
        ballHeld = false;
        transform.parent = null;
        
        UnfreezeAllRigidbodyConstraints();
        free = true;
        col.enabled = true;

        Vector3 stunnedDirection = new Vector3(hitDirection.x - transform.position.x, 1, hitDirection.z - transform.position.z);

        stunnedDirection.x *= -1;
        stunnedDirection.z *= -1;

        stunnedDirection.x *= dropForce;
        stunnedDirection.z *= dropForce;

        rb.AddForce(stunnedDirection, ForceMode.Acceleration);
        StartCoroutine("StunCooldown");
    }

    public void BallDroppedDash(Vector3 headDirection)
    {
        ballHeld = false;
        transform.parent = null;
        UnfreezeAllRigidbodyConstraints();
        free = true;
        col.enabled = true;
        rb.AddForce(Vector3.up * dashDropForce + (headDirection.normalized * 2f), ForceMode.VelocityChange);
        StartCoroutine("ThrowCooldown");
    }

    private IEnumerator StunCooldown()
    {
        //this is to stop the player who just got stunned from picking up the ball while they're stunned
        yield return new WaitForSeconds(stunCooldown);
        heldBy = null;
    }    

    public void FreezeAllRigidbodyConstraints()
    {
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    private void UnfreezeAllRigidbodyConstraints()
    {
        rb.constraints = RigidbodyConstraints.None;
    }


}
