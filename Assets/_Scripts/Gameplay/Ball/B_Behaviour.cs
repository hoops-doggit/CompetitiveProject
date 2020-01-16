using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_Behaviour : MonoBehaviour
{
    private Collider col;
    private Rigidbody rb;
    private bool ballHeld, bullet;
    [SerializeField]private float dropForce;
    [SerializeField]private float dropUpForce, dashDropForce, bulletForce;
    public bool free;
    public Rigidbody heldBy;
    private float ballThrowCooldown = 0.025f;
    public float magnitude, prePauseMagnitude;
    [SerializeField]private float stunCooldown = 0.1f;
    private Vector3 prePauseDirection;
    public Vector3 preVelocity, postVelocity, _preVel, _postVel;
    private Vector2 ballV2, bulletV2;
    

    private void Start()
    {
        SetupBall();
        bullet = false;
    }

    private void FixedUpdate()
    {
        if (!bullet)
        {
            preVelocity = rb.velocity;
        }
        _preVel = rb.velocity;
        
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
            //bullet = true;
            //StartCoroutine(BallSpeedCheck());

        }
    }

    IEnumerator BallSpeedCheck()
    {

        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();

        postVelocity = rb.velocity;

        if (preVelocity.magnitude > postVelocity.magnitude)
        {
            rb.AddForce(rb.velocity.normalized * preVelocity.magnitude *1.1f, ForceMode.VelocityChange);
            if(rb.velocity.magnitude < bulletForce)
            {
                rb.velocity = rb.velocity.normalized * 30;
                Debug.Log("Did secondary ball v change");
            }
            else
            {
                Debug.Log("Did standard ball v change");
            }
            
        }
        bullet = false;
    }

    public void Bullet(Vector2 bulletVector)
    {
        ballV2.x = _preVel.x;
        ballV2.y = _preVel.z;

        bulletV2 = bulletVector;

        Vector2 testVector = new AngleMath().Vector2BetweenTwoVectors(ballV2, bulletV2);

        if (rb.velocity.magnitude > 1)
        {
            Debug.Log("ball velocity propper");
            rb.AddForce(new Vector3(testVector.x, 0, testVector.y).normalized * (bulletForce + (rb.velocity.magnitude / 4)), ForceMode.VelocityChange);
            rb.velocity = new Vector3(testVector.x, 0, testVector.y).normalized * (bulletForce + (rb.velocity.magnitude / 4));
        }
        else
        {
            Debug.Log("ball velocity else");
            //rb.AddForce(new Vector3(bulletV2.x, 0, bulletV2.y).normalized * bulletForce, ForceMode.VelocityChange);
            rb.velocity = (new Vector3(bulletV2.x, 0, bulletV2.y).normalized * bulletForce);
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
            //Debug.Log("prepausemagnitude = " + prePauseMagnitude);
            //Debug.Log("magnitude = " + magnitude);
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
