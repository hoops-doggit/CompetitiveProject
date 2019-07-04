using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TinyShipBehaviour : MonoBehaviour {

    public Vector3 goal;

    public float acc;
    private int vert;

    private NavMeshAgent me;
    public Transform parent;
    private Rigidbody rb;


    //get a vertex
    //get it's position
    //set transform to position


	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();

;	}
	
	// Update is called once per frame
	void Update () {

        transform.LookAt(parent);
        rb.AddForce(transform.forward * acc);
		
	}
}
