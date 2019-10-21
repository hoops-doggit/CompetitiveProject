using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class UI_CameraShake : MonoBehaviour
{
    public CameraShaker cs;
    

    public void DoCameraShake()
    {
        cs.ShakeOnce(2,4,2,3);
    }



    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            DoCameraShake();
        }
    }
}
