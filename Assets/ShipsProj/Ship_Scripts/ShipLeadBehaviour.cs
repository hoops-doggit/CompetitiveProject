using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShipLeadBehaviour : MonoBehaviour
{

    public Vector3 goal;
    public MeshFilter mf;
    private int vert;
    public GameObject sphere;
    private GameObject sphereClone;

    private NavMeshAgent me;
    public float minSpeed = 5;
    public float maxSpeed = 40;

    public BoxCollider rb;


    //get a vertex
    //get it's position
    //set transform to position


    // Use this for initialization
    void Start()
    {
        sphereClone = Instantiate(sphere);
        rb = GetComponent<BoxCollider>();
        me = GetComponent<NavMeshAgent>();
        GenerateRandomDestination();
        if (sphereClone != null)
        {
            sphereClone.transform.position = goal;
        }
        me.speed = Random.Range(minSpeed, maxSpeed);
        ;
    }

    private void GenerateRandomDestination()
    {
        vert = Random.Range(1, mf.mesh.vertexCount);
        goal = mf.mesh.vertices[vert];
        me.SetDestination(goal);
        sphereClone.transform.position = goal;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("words");
        if (other.gameObject == sphereClone)
        {
            GenerateRandomDestination();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
