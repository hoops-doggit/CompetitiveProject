using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CH_Trails : MonoBehaviour
{
    public Gradient pre;
    public Gradient during;
    public Gradient post;
    private TrailRenderer tr;

    private void Start()
    {
        tr = GetComponent<TrailRenderer>();
        tr.colorGradient = pre;
    }

    // Start is called before the first frame update
    public void Dashing()
    {
        tr = GetComponent<TrailRenderer>();
        tr.colorGradient = during;
    }
    public void CoolingDown()
    {
        tr = GetComponent<TrailRenderer>();
        tr.colorGradient = post;
    }
    public void Ready()
    {
        tr = GetComponent<TrailRenderer>();
        tr.colorGradient = pre;
    }
}
