using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TinyShipBehaviour : MonoBehaviour {

    public Vector3 goal;

    public float acc;
    private int vert;
    public float maxMagnitude;

    private NavMeshAgent me;
    public Transform parent;
    private Rigidbody rb;

    public Transform myActualParent;


    //get a vertex
    //get it's position
    //set transform to position


	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        if (myActualParent != null)
        {
            myActualParent.position = parent.position;
        }

;	}
	
	// Update is called once per frame
	void FixedUpdate () {

        transform.LookAt(parent);
        if (rb.velocity.magnitude < maxMagnitude)
        {
            rb.AddForce(transform.forward * acc,ForceMode.Acceleration);
        }
		
	}
}
