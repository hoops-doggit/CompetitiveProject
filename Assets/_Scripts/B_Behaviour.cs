using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_Behaviour : MonoBehaviour
{
    private Collider col;
    private Rigidbody rb;
    private bool ballHeld;
    [SerializeField]private float dropForce;
    [SerializeField]private float dropUpForce;
    public bool free;
    public Rigidbody heldBy;
    private float ballThrowCooldown = 0.025f;
    private float stunCooldown = 20f;

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

        if (collision.gameObject.tag == "player" && free)
        {
            collision.gameObject.GetComponentInParent<CH_BallInteractions>().PickUpBall(gameObject, this);
        }        
    }

    private void OnCollisionExit(Collision collision)
    {

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

    public void HitBall(Transform playerT, float hitStrength)
    {
        if (!ballHeld)
        {
            Debug.Log("got hit");
            UnfreezeAllRigidbodyConstraints();
            rb.AddForce(playerT.forward * hitStrength, ForceMode.Acceleration);
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

    private IEnumerator StunCooldown()
    {
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
