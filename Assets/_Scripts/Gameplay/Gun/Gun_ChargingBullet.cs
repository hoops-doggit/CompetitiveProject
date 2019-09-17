using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Gun_ChargingBullet : MonoBehaviour
{
    public Material charging;
    public List<Material> charged = new List<Material>();
    private Renderer r;

    private void Start()
    {
        r = GetComponent<Renderer>();
    }


    public void SetChargedMat(int i)
    {
        r.material = charged[i];
    }
}
