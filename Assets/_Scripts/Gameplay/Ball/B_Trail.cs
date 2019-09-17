using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_Trail : MonoBehaviour
{
    private TrailRenderer tr;
    private Rigidbody rb;
    [SerializeField] private float minTime;
    private float speed;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        tr = GetComponent<TrailRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        speed = rb.velocity.magnitude;
        tr.time = speed * minTime/2;


    }
}
