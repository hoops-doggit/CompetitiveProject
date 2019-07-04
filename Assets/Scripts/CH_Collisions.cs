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

    public Transform tFront01;
    public Transform tFront02;
    public Transform tBack01;
    public Transform tBack02;
    public Transform tLeft01;
    public Transform tLeft02;
    public Transform tRight01;
    public Transform tRight02;
    public float skinDepth;
    public float maxDistance;
    private RaycastHit front01Hit;
    private RaycastHit front02Hit;
    private RaycastHit back01Hit;
    private RaycastHit back02Hit;
    private RaycastHit left01Hit;
    private RaycastHit left02Hit;
    private RaycastHit right01Hit;
    private RaycastHit right02Hit;
    public List<float> xFloatRaw = new List<float>();
    public List<float> xFloatList = new List<float>();
    public List<float> yFloatList = new List<float>();
    public float frontColPoint;
    public float backColPoint;
    public float leftColPoint;
    public float rightColPoint;
    public GameObject fColGo;
    public GameObject bColGo;
    public GameObject lColGo;
    public GameObject rColGo;

    private float characterRadius = 0.43f;

    public GameObject Front01Debug;
    public GameObject Front02Debug;
    public GameObject Back01Debug;
    public GameObject Back02Debug;
    public GameObject Left01Debug;
    public GameObject Left02Debug;
    public GameObject Right01Debug;
    public GameObject Right02Debug;


    // Use this for initialization
    void Start()
    {
        if (!debug)
        {
            fColGo.SetActive(false);
            bColGo.SetActive(false);
            lColGo.SetActive(false);
            rColGo.SetActive(false);
            Front01Debug.SetActive(false);
            Front02Debug.SetActive(false);
            Back01Debug.SetActive(false);
            Back02Debug.SetActive(false);
            Left01Debug.SetActive(false);
            Left02Debug.SetActive(false);
            Right01Debug.SetActive(false);
            Right02Debug.SetActive(false);
        }
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        xFloatList.Clear();
        yFloatList.Clear();
        xFloatRaw.Clear();
        
        if (Physics.Raycast(tFront01.position, tFront01.forward, out front01Hit, Mathf.Infinity))
        {
            if (front01Hit.distance <= skinDepth)
            {
                front01 = true;
                Debug.DrawRay(tFront01.position, tFront01.TransformDirection(Vector3.forward) * front01Hit.distance, Color.red);
            }
            else
            {
                front01 = false;
                float distance = maxDistance;
                if (front01Hit.distance < maxDistance) { distance = front01Hit.distance; }
                Debug.DrawRay(tFront01.position, tFront01.TransformDirection(Vector3.forward) * distance, Color.green);
            }
        }
        if (Physics.Raycast(tFront02.position, tFront02.forward, out front02Hit, Mathf.Infinity))
        {
            if (front02Hit.distance <= skinDepth)
            {
                front02 = true;
                Debug.DrawRay(tFront02.position, tFront02.TransformDirection(Vector3.forward) * front02Hit.distance, Color.red);
            }
            else
            {
                front02 = false;
                float distance = maxDistance;
                if (front02Hit.distance < maxDistance) { distance = front02Hit.distance; }
                Debug.DrawRay(tFront02.position, tFront02.TransformDirection(Vector3.forward) * distance, Color.green);
            }
        }
        if (Physics.Raycast(tBack01.position, tBack01.forward, out back01Hit, Mathf.Infinity))
        {
            if (back01Hit.distance <= skinDepth)
            {
                back01 = true;
                Debug.DrawRay(tBack01.position, tBack01.TransformDirection(Vector3.forward) * back01Hit.distance, Color.red);
            }
            else
            {
                back01 = false;
                float distance = maxDistance;
                if (back01Hit.distance < maxDistance) { distance = back01Hit.distance; }
                Debug.DrawRay(tBack01.position, tBack01.TransformDirection(Vector3.forward) * distance, Color.green);
            }
        }
        if (Physics.Raycast(tBack02.position, tBack02.forward, out back02Hit, Mathf.Infinity))
        {
            if (back02Hit.distance <= skinDepth)
            {
                back02 = true;
                Debug.DrawRay(tBack02.position, tBack02.TransformDirection(Vector3.forward) * back02Hit.distance, Color.red);
            }
            else
            {
                back02 = false;
                float distance = maxDistance;
                if (back02Hit.distance < maxDistance) { distance = back02Hit.distance; }
                Debug.DrawRay(tBack02.position, tBack02.TransformDirection(Vector3.forward) * distance, Color.green);
            }
        }
        if (Physics.Raycast(tLeft01.position, tLeft01.forward, out left01Hit, Mathf.Infinity))
        {
            if (left01Hit.distance <= skinDepth)
            {
                left01 = true;
                Debug.DrawRay(tLeft01.position, tLeft01.TransformDirection(Vector3.forward) * left01Hit.distance, Color.red);
            }
            else
            {
                left01 = false;
                float distance = maxDistance;
                if (left01Hit.distance < maxDistance) { distance = left01Hit.distance; }

                Debug.DrawRay(tLeft01.position, tLeft01.TransformDirection(Vector3.forward) * distance, Color.green);
            }
        }
        if (Physics.Raycast(tLeft02.position, tLeft02.forward, out left02Hit, Mathf.Infinity))
        {
            if (left02Hit.distance <= skinDepth)
            {
                left02 = true;
                Debug.DrawRay(tLeft02.position, tLeft02.TransformDirection(Vector3.forward) * left02Hit.distance, Color.red);
            }
            else
            {
                left02 = false;
                float distance = maxDistance;
                if (left02Hit.distance < maxDistance) { distance = left02Hit.distance; }
                Debug.DrawRay(tLeft02.position, tLeft02.TransformDirection(Vector3.forward) * distance, Color.green);
            }
        }
        if (Physics.Raycast(tRight01.position, tRight01.forward, out right01Hit, Mathf.Infinity))
        {
            if (right01Hit.distance <= skinDepth)
            {
                right01 = true;
                Debug.DrawRay(tRight01.position, tRight01.TransformDirection(Vector3.forward) * right01Hit.distance, Color.red);
            }
            else
            {
                right01 = false;
                float distance = maxDistance;
                if (right01Hit.distance < maxDistance) { distance = right01Hit.distance; }
                Debug.DrawRay(tRight01.position, tRight01.TransformDirection(Vector3.forward) * distance, Color.green);
            }
        }
        if (Physics.Raycast(tRight02.position, tRight02.forward, out right02Hit, Mathf.Infinity))
        {
            if (right02Hit.distance <= skinDepth)
            {
                right02 = true;
                Debug.DrawRay(tRight02.position, tRight02.TransformDirection(Vector3.forward) * right02Hit.distance, Color.red);
            }

            else
            {
                right02 = false;
                float distance = maxDistance;                
                if (right02Hit.distance < maxDistance) {distance = right02Hit.distance;}
                Debug.DrawRay(tRight02.position, tRight02.TransformDirection(Vector3.forward) * distance, Color.green);
            }
        }

        yFloatList.Add(front01Hit.point.z);
        yFloatList.Add(front02Hit.point.z);
        yFloatList.Add(back01Hit.point.z);
        yFloatList.Add(back02Hit.point.z);
        xFloatList.Add(left01Hit.point.x);
        xFloatList.Add(left02Hit.point.x);
        xFloatList.Add(right01Hit.point.x);
        xFloatList.Add(right02Hit.point.x);
    }

    private void Update()
    {
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


        if (debug)
        {
            Front01Debug.transform.position = new Vector3(front01Hit.point.x, 0.5f, front01Hit.point.z);
            Front02Debug.transform.position = new Vector3(front02Hit.point.x, 0.5f, front02Hit.point.z);
            Back01Debug.transform.position = new Vector3(back01Hit.point.x, 0.5f, back01Hit.point.z);
            Back02Debug.transform.position = new Vector3(back02Hit.point.x, 0.5f, back02Hit.point.z);
            Left01Debug.transform.position = new Vector3(left01Hit.point.x, 0.5f, left01Hit.point.z);
            Left02Debug.transform.position = new Vector3(left02Hit.point.x, 0.5f, left02Hit.point.z);
            Right01Debug.transform.position = new Vector3(right01Hit.point.x, 0.5f, right01Hit.point.z);
            Right02Debug.transform.position = new Vector3(right02Hit.point.x, 0.5f, right02Hit.point.z);

            fColGo.transform.position = new Vector3(transform.position.x, transform.position.y, frontColPoint);
            bColGo.transform.position = new Vector3(transform.position.x, transform.position.y, backColPoint);
            lColGo.transform.position = new Vector3(leftColPoint, transform.position.y, transform.position.z);
            rColGo.transform.position = new Vector3(rightColPoint, transform.position.y, transform.position.z);
        }
    }
}
