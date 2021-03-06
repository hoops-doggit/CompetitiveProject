﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShipTrajectoryAdjust : MonoBehaviour
{
    private Vector3 startPos;
    private Vector3 goalPos;

    public float distanceVariability;
    private Vector2 x;
    private Vector2 y;
    private Vector2 z;

    public int maxDirectionChangeTime;
    public int currentTime = 0;
    private int timeToChange;

    public float adjustmentSpeed;

    //old below
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
        startPos = transform.localPosition;
        x = new Vector2(startPos.x - distanceVariability, startPos.x + distanceVariability);
        y = new Vector2(startPos.y - distanceVariability, startPos.y + distanceVariability);
        z = new Vector2(startPos.z - distanceVariability, startPos.z + distanceVariability);

        GenerateRandomDestination();
    }

    private void GenerateRandomDestination()
    {
        currentTime = 0;
        timeToChange = Random.Range(0, maxDirectionChangeTime);
        goalPos = new Vector3(Random.Range(x.x, x.y), Random.Range(y.x, y.y), Random.Range(z.x, z.y));
    }

    //


    // Update is called once per frame
    void Update()
    {
       
        //move closer to the position
        if(gameObject.transform.localPosition.x < goalPos.x)
        {
            gameObject.transform.localPosition += new Vector3(adjustmentSpeed,0,0) * Time.deltaTime;
        }
        else
        {
            gameObject.transform.localPosition -= new Vector3(adjustmentSpeed, 0, 0) * Time.deltaTime;
        }

        if (gameObject.transform.localPosition.y < goalPos.y)
        {
            gameObject.transform.localPosition += new Vector3(0, adjustmentSpeed, 0) * Time.deltaTime;
        }
        else
        {
            gameObject.transform.localPosition -= new Vector3(0, adjustmentSpeed, 0) * Time.deltaTime;
        }

        if (gameObject.transform.localPosition.z < goalPos.z)
        {
            gameObject.transform.localPosition += new Vector3(0, 0,adjustmentSpeed) * Time.deltaTime;
        }
        else
        {
            gameObject.transform.localPosition -= new Vector3(0, 0, adjustmentSpeed) * Time.deltaTime;
        }

        currentTime++;
        if(currentTime >= timeToChange)
        {
            GenerateRandomDestination();
        }

    }
}
