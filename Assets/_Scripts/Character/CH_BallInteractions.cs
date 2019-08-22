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
    public bool holdingBall;
    private KeyCode throwKey;
    [SerializeField]private Rigidbody my_rb;
    

    private void Start()
    {
        CH_Input chi = GetComponent<CH_Input>();
        throwKey = chi.throwKey;
    }

    private void Update()
    {
        if (holdingBall)
        {
            if (Input.GetKey(throwKey))
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

    public void DropBall(Vector3 stunVelocity, string reason)
    {
        if (holdingBall && reason == "bullet")
        {
            ballScript.BallDroppedBullet(stunVelocity);
            holdingBall = false;
        }
        if (holdingBall && reason == "bat")
        {
            ballScript.BallDroppedBat(stunVelocity);
            holdingBall = false;
        }

    }

    
}
