using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Gun_ChargingBullet : MonoBehaviour
{
    public string pn;
    public Material[] charging;
    public List<Material> charged = new List<Material>();
    private Renderer r;
    private string playerNumber;
    

    private void Start()
    {
        r = GetComponent<Renderer>();

        if(pn == "p1")
        {
            r.material = charging[0];
        }
        else if (pn == "p2")
        {
            r.material = charging[1];
        }
        else if (pn == "p3")
        {
            r.material = charging[2];
        }
        else if (pn == "p4")
        {
            r.material = charging[3];
        }


    }


    public void SetChargedMat(int i)
    {
        r.material = charged[i];
    }
}
