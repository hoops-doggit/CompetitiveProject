using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_Lazer : MonoBehaviour
{
    public List<Vector3> points = new List<Vector3>();
    [SerializeField] Transform startPoint;
    [SerializeField] LayerMask layerMask;
    [SerializeField] Material[] mats;
    [SerializeField] CH_Input pi;
    private LineRenderer lineR;
    public bool fireLazer;


    void Start()
    {
        lineR = GetComponent<LineRenderer>();
        gameObject.transform.SetParent(null);
        transform.position = Vector3.zero;
        switch (pi.playerNumber)
        {
            case PlayerNumber.player1:
                lineR.material = mats[0];
                break;
            case PlayerNumber.player2:
                lineR.material = mats[1];
                break;
            case PlayerNumber.player3:
                lineR.material = mats[2];
                break;
            case PlayerNumber.player4:
                lineR.material = mats[3];
                break;
        }
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
