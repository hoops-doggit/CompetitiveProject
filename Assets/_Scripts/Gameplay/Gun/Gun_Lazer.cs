using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_Lazer : MonoBehaviour
{
    public List<Vector3> points = new List<Vector3>();
    [SerializeField] Transform startPoint;
    [SerializeField] LayerMask layerMask;
    private LineRenderer lineR;
    private bool fireLazer;

    void Start()
    {
        lineR = GetComponent<LineRenderer>();
    }

    public void FirinMaLazer()
    {
        fireLazer = true;
    }

    public void StopFiringLazer()
    {
        lineR.enabled = false;
    }

    private void FixedUpdate()
    {
        if (fireLazer)
        {
            RaycastHit hit;
            if (Physics.Raycast(startPoint.position, startPoint.forward, out hit, Mathf.Infinity, layerMask))
            {
                Debug.DrawRay(startPoint.position, startPoint.forward * hit.distance, Color.green);
                lineR.enabled = true;
                lineR.SetPosition(0, startPoint.position);
                lineR.SetPosition(1, hit.point);
            }
        }
        else
        {
            StopFiringLazer();
        }
        fireLazer = false;
    }


}
