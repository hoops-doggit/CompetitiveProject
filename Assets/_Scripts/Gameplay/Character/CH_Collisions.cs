using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CH_Collisions : MonoBehaviour {

    public bool debug = false;
    public bool front01;
    public bool front02;
    public bool back01;
    public bool back02;
    public bool left01;
    public bool left02;
    public bool right01;
    public bool right02;

    public bool front = false;
    public bool back = false;
    public bool left = false;
    public bool right= false;
    public List<float> collisionPoints = new List<float>(4);

    public float leftDistance;
    public float collisionDistance;

    public Transform tFront01;
    public Transform tFront02;
    public Transform tBack01;
    public Transform tBack02;
    public Transform tLeft01;
    public Transform tLeft02;
    public Transform tRight01;
    public Transform tRight02;
    [SerializeField]
    private float skinDepth;
    public float maxDistance;
    private RaycastHit front01Hit;
    private RaycastHit front02Hit;
    private RaycastHit back01Hit;
    private RaycastHit back02Hit;
    private RaycastHit left01Hit;
    private RaycastHit left02Hit;
    private RaycastHit right01Hit;
    private RaycastHit right02Hit;
    private CH_Input chi;
    public List<float> xFloatRaw = new List<float>();
    public List<float> xFloatList = new List<float>();
    public List<float> yFloatList = new List<float>();
    public float frontColPoint;
    public float backColPoint;
    public float leftColPoint;
    public float rightColPoint;


    private float characterRadius = 0.43f;
    public LayerMask layerMask1;
    public LayerMask layerMask2;
    public LayerMask layerMask3;
    public LayerMask layerMask4;
    private LayerMask layerMask;

    // Use this for initialization
    void Start()
    {
        chi = GetComponent<CH_Input>();
        if (chi.playerNumber == PlayerNumber.player1)
        {
            layerMask = layerMask1;
        }
        else if (chi.playerNumber == PlayerNumber.player2)
        {
            layerMask = layerMask1;
        }
        else if (chi.playerNumber == PlayerNumber.player3)
        {
            layerMask = layerMask3;
        }
        else if (chi.playerNumber == PlayerNumber.player4)
        {
            layerMask = layerMask4;
        }

        layerMask = ~layerMask; 
    }

    // Update is called once per frame
    public void CalculateRays()
    {
        skinDepth = GetComponent<CH_Movement2>().skinDepth;
        xFloatList.Clear();
        yFloatList.Clear();
        xFloatRaw.Clear();
        front = false;
        back = false;
        left = false;
        right = false;


        
        if (Physics.Raycast(tFront01.position, tFront01.forward, out front01Hit, Mathf.Infinity, layerMask))
        {
            if (front01Hit.distance <= skinDepth)
            {
                front01 = true;
                if (debug)
                {
                    Debug.DrawRay(tFront01.position, tFront01.TransformDirection(Vector3.forward) * front01Hit.distance, Color.red);
                }

            }
            else
            {
                front01 = false;
                float distance = maxDistance;
                if (front01Hit.distance < maxDistance) { distance = front01Hit.distance; }
                if (debug)
                {
                    Debug.DrawRay(tFront01.position, tFront01.TransformDirection(Vector3.forward) * distance, Color.green);
                }
            }

        }
        if (Physics.Raycast(tFront02.position, tFront02.forward, out front02Hit, Mathf.Infinity, layerMask))
        {
            if (front02Hit.distance <= skinDepth)
            {
                front02 = true;
                if (debug)
                {
                    Debug.DrawRay(tFront02.position, tFront02.TransformDirection(Vector3.forward) * front02Hit.distance, Color.red);
                }
            }
            else
            {
                front02 = false;
                float distance = maxDistance;
                if (front02Hit.distance < maxDistance) { distance = front02Hit.distance; }
                if (debug)
                {
                    Debug.DrawRay(tFront02.position, tFront02.TransformDirection(Vector3.forward) * distance, Color.green);
                }
            }

        }
        if (Physics.Raycast(tBack01.position, tBack01.forward, out back01Hit, Mathf.Infinity, layerMask))
        {
            if (back01Hit.distance <= skinDepth)
            {
                back01 = true;
                if (debug)
                {
                    Debug.DrawRay(tBack01.position, tBack01.TransformDirection(Vector3.forward) * back01Hit.distance, Color.red);
                }
            }
            else
            {
                back01 = false;
                float distance = maxDistance;
                if (back01Hit.distance < maxDistance) { distance = back01Hit.distance; }
                if (debug)
                {
                    Debug.DrawRay(tBack01.position, tBack01.TransformDirection(Vector3.forward) * distance, Color.green);
                }
            }
  
        }
        if (Physics.Raycast(tBack02.position, tBack02.forward, out back02Hit, Mathf.Infinity, layerMask))
        {
            if (back02Hit.distance <= skinDepth)
            {
                back02 = true;
                if (debug)
                {
                    Debug.DrawRay(tBack02.position, tBack02.TransformDirection(Vector3.forward) * back02Hit.distance, Color.red);
                }
            }
            else
            {
                back02 = false;
                float distance = maxDistance;
                if (back02Hit.distance < maxDistance) { distance = back02Hit.distance; }
                if (debug)
                {
                    Debug.DrawRay(tBack02.position, tBack02.TransformDirection(Vector3.forward) * distance, Color.green);
                }
            }

        }
        if (Physics.Raycast(tLeft01.position, tLeft01.forward, out left01Hit, Mathf.Infinity, layerMask))
        {
            leftDistance = left01Hit.distance;
            if (left01Hit.distance <= skinDepth)
            {
                left01 = true;
                if (debug)
                {
                    Debug.DrawRay(tLeft01.position, tLeft01.TransformDirection(Vector3.forward) * left01Hit.distance, Color.red);
                }
            }
            else
            {
                left01 = false;
                float distance = maxDistance;
                if (left01Hit.distance < maxDistance) { distance = left01Hit.distance; }
                if (debug)
                {
                    Debug.DrawRay(tLeft01.position, tLeft01.TransformDirection(Vector3.forward) * distance, Color.green);
                }
            }

        }
        if (Physics.Raycast(tLeft02.position, tLeft02.forward, out left02Hit, Mathf.Infinity, layerMask))
        {
            if (left02Hit.distance <= skinDepth)
            {
                left02 = true;
                if (debug)
                {
                    Debug.DrawRay(tLeft02.position, tLeft02.TransformDirection(Vector3.forward) * left02Hit.distance, Color.red);
                }
            }
            else
            {
                left02 = false;
                float distance = maxDistance;
                if (left02Hit.distance < maxDistance) { distance = left02Hit.distance; }
                if (debug)
                {
                    Debug.DrawRay(tLeft02.position, tLeft02.TransformDirection(Vector3.forward) * distance, Color.green);
                }
            }

        }
        if (Physics.Raycast(tRight01.position, tRight01.forward, out right01Hit, Mathf.Infinity, layerMask))
        {
            if (right01Hit.distance <= skinDepth)
            {
                right01 = true;
                if (debug)
                {
                    Debug.DrawRay(tRight01.position, tRight01.TransformDirection(Vector3.forward) * right01Hit.distance, Color.red);
                }
            }
            else
            {
                right01 = false;
                float distance = maxDistance;
                if (right01Hit.distance < maxDistance) { distance = right01Hit.distance; }
                if (debug)
                {
                    Debug.DrawRay(tRight01.position, tRight01.TransformDirection(Vector3.forward) * distance, Color.green);
                }
            }

        }
        if (Physics.Raycast(tRight02.position, tRight02.forward, out right02Hit, Mathf.Infinity, layerMask))
        {
            if (right02Hit.distance <= skinDepth)
            {
                right02 = true;
                if (debug)
                {
                    Debug.DrawRay(tRight02.position, tRight02.TransformDirection(Vector3.forward) * right02Hit.distance, Color.red);
                }
            }

            else
            {
                right02 = false;
                float distance = maxDistance;
                if (right02Hit.distance < maxDistance) { distance = right02Hit.distance; }
                if (debug)
                {
                    Debug.DrawRay(tRight02.position, tRight02.TransformDirection(Vector3.forward) * distance, Color.green);
                }
            }
        }

        if(front01 || front02)
        {
            front = true;
        }
        else { front = false; }
        if (back01 || back02)
        {
            back = true;
        }
        else { back = false; }
        if (left01 || left02)
        {
            left = true;
        }
        else { left = false; }
        if (right01 || right02)
        {
            right = true;
        }

        else { right = false; }

        yFloatList.Add(front01Hit.point.z);
        yFloatList.Add(front02Hit.point.z);
        yFloatList.Add(back01Hit.point.z);
        yFloatList.Add(back02Hit.point.z);
        xFloatList.Add(left01Hit.point.x);
        xFloatList.Add(left02Hit.point.x);
        xFloatList.Add(right01Hit.point.x);
        xFloatList.Add(right02Hit.point.x);

        if (xFloatList[0] > xFloatList[1])
        {
            leftColPoint = xFloatList[0];
        }
        else { leftColPoint = xFloatList[1]; }


        if (xFloatList[2] < xFloatList[3])
        {
            rightColPoint = xFloatList[2];
        }
        else { rightColPoint = xFloatList[3]; }


        if (yFloatList[0] < yFloatList[1])
        {
            frontColPoint = yFloatList[0];
        }
        else { frontColPoint = yFloatList[1]; }


        if (yFloatList[2] > yFloatList[3])
        {
            backColPoint = yFloatList[2];
        }
        else { backColPoint = yFloatList[3]; }

        leftColPoint += skinDepth;
        rightColPoint -= skinDepth;
        frontColPoint -= skinDepth;
        backColPoint += skinDepth;

        collisionPoints[0] = frontColPoint;
        collisionPoints[1] = backColPoint;
        collisionPoints[2] = leftColPoint;
        collisionPoints[3] = rightColPoint;
    }
}
