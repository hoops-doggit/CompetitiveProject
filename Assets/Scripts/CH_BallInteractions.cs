﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CH_BallInteractions : MonoBehaviour
{
    public Transform ballHeldPos;
    public Transform ballGroundPos;
    public float ballThrowSpeed;
    private B_Behaviour ballScript;
    private GameObject ball;
    private bool holdingBall;

    private string throwAxis;
    

    private void Start()
    {
        CH_Input chi = GetComponent<CH_Input>();
        throwAxis = chi.throwAxis;
    }

    private void FixedUpdate()
    {
        if (holdingBall)
        {
            if (Input.GetButton(throwAxis))
            {
                ThrowBall();
            }
        }
    }

    public void PickUpBall(GameObject ball, B_Behaviour ballScript)
    {
        this.ballScript = ballScript;
        this.ball = ball;
        ballScript.BallPickedUp();
        holdingBall = true;
        ball.transform.parent = ballHeldPos.transform;
        ball.transform.localPosition = new Vector3 (0,0,0);
    }

    public void ThrowBall()
    {
        holdingBall = false;
        ball.transform.parent = ballGroundPos;
        ball.transform.localPosition = new Vector3(0, 0, 0);
        ball.transform.parent = null;
        ballScript.BallThrown();
        ball.GetComponent<Rigidbody>().AddForce(ballHeldPos.transform.forward * ballThrowSpeed, ForceMode.Acceleration);
    }
}