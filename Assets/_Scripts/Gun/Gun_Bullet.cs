﻿using UnityEngine;

public class Gun_Bullet : MonoBehaviour {

    float age = 0;
    public float maxAge = 800;
    public float initialSpeed = 200;
    private Rigidbody rb;
    public Vector3 direction;
    private int i = 0;
    public string owner;


    // Update is called once per frame
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * initialSpeed, ForceMode.Acceleration);
    }
    void FixedUpdate () {
        age++;
        if (age > maxAge)
        {
            Death();
        }

        if (i < 1)
        {
            i++;
            direction = rb.velocity;
        }
    }

    private void Update()
    {
        direction = rb.velocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.tag == "swingZone") 
        //{
        //    HitByBat(collision.transform, 3, "p2");
        //}
        //else
        //{
            
        //}

        Collider col = GetComponent<Collider>();
        col.enabled = false;
        if (collision.gameObject.GetComponent<Block_Destructible>() != null)
        {
            collision.gameObject.GetComponent<Block_Destructible>().Bumped();
        }
        Death();

    }

    private void Death()
    {
        Destroy(gameObject);
    }

    public void HitByBat(Transform t, float hitStrength, string newOwner)
    {
        //this currently sends bullet back in reverse direction. It should send it back to wherever the owner now is
        transform.rotation = Quaternion.Inverse(transform.rotation);
        rb.velocity = transform.forward * rb.velocity.magnitude;
        //rb.AddForce((transform.forward * -1) * rb.velocity.magnitude, ForceMode.Acceleration);
        owner = newOwner;
    }

    

}
