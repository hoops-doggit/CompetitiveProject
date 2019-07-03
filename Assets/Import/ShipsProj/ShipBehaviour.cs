using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShipBehaviour : MonoBehaviour {

    public Vector3 goal;
    public MeshFilter mf;
    private int vert;
    public GameObject sphere;

    private NavMeshAgent me;
    public float minSpeed = 5;
    public float maxSpeed = 40;


    //get a vertex
    //get it's position
    //set transform to position


	// Use this for initialization
	void Start () {
        me = GetComponent<NavMeshAgent>();
        vert = Random.Range(1, mf.mesh.vertexCount);
        goal = mf.mesh.vertices[vert];
        me.SetDestination(goal);
        if (sphere != null)
        {
            sphere.transform.position = goal;
        }
        me.speed = Random.Range(minSpeed, maxSpeed);
;	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
