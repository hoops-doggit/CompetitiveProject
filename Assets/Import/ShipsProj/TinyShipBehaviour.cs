using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TinyShipBehaviour : MonoBehaviour {

    public Vector3 goal;
    public MeshFilter mf;
    private int vert;

    private NavMeshAgent me;


    //get a vertex
    //get it's position
    //set transform to position


	// Use this for initialization
	void Start () {
        me = GetComponent<NavMeshAgent>();
        vert = Random.Range(1, mf.mesh.vertexCount);
        goal = mf.mesh.vertices[vert];
        me.SetDestination(goal);
        me.height = Random.Range(1, 6);

;	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
