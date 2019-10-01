using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CH_Trails : MonoBehaviour
{

    public Gradient pre;
    public float preTime;
    public Gradient during;
    public float duringTime;
    public Gradient post;
    public float postTime;
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
        tr.time = duringTime;
    }
    public void CoolingDown()
    {
        tr = GetComponent<TrailRenderer>();
        tr.colorGradient = post;
        tr.time = postTime;
    }
    public void Ready()
    {
        tr = GetComponent<TrailRenderer>();
        tr.colorGradient = pre;
        tr.time = preTime;
    }
}
