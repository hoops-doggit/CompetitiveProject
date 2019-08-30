﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Gun_Bullet : MonoBehaviour {

    float age = 0;
    public float maxAge = 800;
    public float initialSpeed = 200;
    private Rigidbody rb;
    public Vector3 direction;
    private int i = 0;
    public string owner;
    private int bulletTimer = 0;
    public Transform ownerT;
    [SerializeField] private List<Material> bulletMats = new List<Material>();


    // Update is called once per frame
    private void Start()
    {
        GetComponent<MeshRenderer>().material = bulletMats[SetupMaterial()];
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * initialSpeed, ForceMode.Acceleration);
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
        if(bulletTimer < 2)
        {
            bulletTimer++;
            if (bulletTimer >= 2)
            {
                gameObject.layer = LayerMask.NameToLayer(owner);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        //Collider col = GetComponent<Collider>();
        //col.enabled = false;
        GameObject go = collision.gameObject;
        if (collision.gameObject.GetComponent<Block_Destructible>() != null)
        {
            collision.gameObject.GetComponent<Block_Destructible>().Bumped();            
        }
        
        if(collision.gameObject.tag == "bullet")
        {
            HitAnotherBullet(ownerT, go.GetComponent<Gun_Bullet>().owner);
        }
        else
        {
            Death();
        }

        
    }

    private void Death()
    {
        Destroy(gameObject);
    }

    public void HitByBat(Transform t, float hitStrength, string newOwner)
    {
        //this currently sends bullet back in reverse direction. It should send it back to wherever the owner now is
        transform.rotation = Quaternion.Inverse(transform.rotation);
        transform.LookAt(ownerT);
        ownerT = t;
        rb.velocity = transform.forward * rb.velocity.magnitude;
        //rb.AddForce((transform.forward * -1) * rb.velocity.magnitude, ForceMode.Acceleration);
        owner = newOwner;
        gameObject.layer = LayerMask.NameToLayer(newOwner);
    }

    public void HitAnotherBullet(Transform t, string newOwner)
    {
        rb = GetComponent<Rigidbody>();
        float mag = rb.velocity.magnitude;
        transform.rotation = Quaternion.Inverse(transform.rotation);
        transform.LookAt(ownerT);
        ownerT = t;
        rb.velocity = transform.forward * mag;
        //rb.AddForce((transform.forward * -1) * rb.velocity.magnitude, ForceMode.Acceleration);
        owner = newOwner;
        gameObject.layer = LayerMask.NameToLayer(newOwner);
    }

    

}
