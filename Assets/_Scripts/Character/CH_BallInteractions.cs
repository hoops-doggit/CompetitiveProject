using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CH_BallInteractions : MonoBehaviour
{
    public Transform ballHeldPos;
    public Transform ballThrowPosition;
    public float ballThrowSpeed;
    private B_Behaviour ballScript;
    private GameObject ball;
    private bool holdingBall;
    private string throwAxis;
    [SerializeField]private Rigidbody my_rb;
    

    private void Start()
    {
        CH_Input chi = GetComponent<CH_Input>();
        throwAxis = chi.throwButton;
    }

    private void Update()
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
        if (ballScript.heldBy != my_rb)
        {
            this.ballScript = ballScript;
            this.ball = ball;
            holdingBall = true;
            ball.transform.parent = ballHeldPos.transform;
            ball.transform.localPosition = new Vector3(0, 0, 0);
            ballScript.BallPickedUp(my_rb);
        }
    }

    public void ThrowBall()
    {
        holdingBall = false;
        ball.transform.parent = ballThrowPosition;
        ball.transform.localPosition = new Vector3(0, 0, 0);
        ball.transform.parent = null;
        ballScript.BallThrown();
        ball.GetComponent<Rigidbody>().AddForce(ballHeldPos.transform.forward * ballThrowSpeed, ForceMode.Acceleration);
    }    

    public void DropBall()
    {
        if (holdingBall)
        {
            ballScript.BallDropped();
            holdingBall = false;
        }
        
    }

    
}
