using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Gun_Bullet : MonoBehaviour {

    float age = 0;
    public float maxAge = 800;
    public float initialSpeed = 10000;
    private Rigidbody rb;
    public Vector3 direction;
    private int i = 0, deflectNumber;
    public string owner;
    public Transform ownerT;
    private int bulletTimer = 0;
    public string tempOwner;
    public Transform tempOwnerT;
    
    [SerializeField] private List<Material> bulletMats = new List<Material>();


    // Update is called once per frame
    private void Start()
    {
        GetComponent<MeshRenderer>().material = bulletMats[SetupMaterial()];
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * initialSpeed, ForceMode.VelocityChange);
    }

    private int SetupMaterial()
    {
        if(owner == "p1" || owner == "p3")
        {
            return 0;
        }
        else
        {
            return 1;
        }
    }

    void FixedUpdate () {
        if(i == 0)
        {
            direction = transform.forward;
        }
        if (i < 1)
        {
            i++;
            direction = rb.velocity;
        }
        direction = rb.velocity;
        if (bulletTimer < 2)
        {
            bulletTimer++;
            if (bulletTimer >= 2)
            {
                //THIS NEEDS TO BE CHANGED
                //gameObject.layer = LayerMask.NameToLayer(owner);
            }
        }
        if (tempOwner != null && ownerT != tempOwnerT)
        {
            //owner = tempOwner;
            //ownerT = tempOwnerT;
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        GameObject go = collision.gameObject;

        deflectNumber++;

        if(go.tag != "ball")
        {
            if (deflectNumber <= 1)
            {
            }
            else
            {
                DestroyBullet();
            }
        }
        else
        {
            //go.GetComponent<B_Behaviour>().Bullet(new Vector2(rb.velocity.x, rb.velocity.z));
            //DestroyBullet();
        }             
    }

    public void HitBall(GameObject ball)
    {
        Debug.Log("Running hit ball");
        ball.GetComponent<B_Behaviour>().Bullet(new Vector2(rb.velocity.x, rb.velocity.z));
        DestroyBullet();
    }

    public void DestroyBullet()
    {
        Destroy(gameObject);
    }

    public void HitByBat(Transform t, float hitStrength, string newOwner)
    {
        //this currently sends bullet back in reverse direction. It should send it back to wherever the owner now is
        transform.rotation = Quaternion.Inverse(transform.rotation);
        transform.LookAt(ownerT);
        ownerT = t;
        rb.velocity = transform.forward * rb.velocity.magnitude * 1.5f;
        //rb.AddForce((transform.forward * -1) * rb.velocity.magnitude, ForceMode.Acceleration);
        owner = newOwner;
        gameObject.layer = LayerMask.NameToLayer(newOwner);
    }

    public void GotHitByOwner(Transform lookDirection, float hitStrength, string newOwner)
    {
        transform.rotation = lookDirection.rotation;
        rb.velocity = transform.forward * rb.velocity.magnitude * 1.5f;
        owner = newOwner;
        gameObject.layer = LayerMask.NameToLayer(newOwner);
    }

    public void HitAnotherBullet(Transform t, string newOwner)
    {
        rb = GetComponent<Rigidbody>();
        transform.LookAt(ownerT);
        rb.velocity = Vector3.zero;
        rb.AddForce(transform.forward * initialSpeed, ForceMode.Acceleration);
        tempOwnerT = t;
        tempOwner = newOwner;
        gameObject.layer = LayerMask.NameToLayer(newOwner);
    }

    

}
