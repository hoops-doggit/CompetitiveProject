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
    private CH_Movement2 chm;
    

    private void Start()
    {
        CH_Input chi = GetComponent<CH_Input>();
        throwKey = chi.throwKey;
        chm = GetComponent<CH_Movement2>();
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
        if (!holdingBall)
        {
            if (ballScript.heldBy != my_rb)
            {
                this.ballScript = ballScript;
                this.ball = ball;
                holdingBall = true;
                chm.PlayerHoldingBall();
                ball.transform.parent = ballHeldPos.transform;
                ball.transform.localPosition = new Vector3(0, 0, 0);
                ballScript.BallPickedUp(my_rb);
            }
        }
    }

    public void ThrowBall()
    {
        ball.transform.parent = ballThrowPosition;
        ball.transform.localPosition = new Vector3(0, 0, 0);
        ball.transform.parent = null;
        ballScript.BallThrown();
        ball.GetComponent<Rigidbody>().AddForce(ballHeldPos.transform.forward * ballThrowSpeed, ForceMode.Acceleration);
        ResetRelationshipToBall();
    }    

    public void DropBall(Vector3 stunPosition, string reason)
    {
        if (holdingBall && reason == "bullet")
        {
            ballScript.BallDroppedBullet(stunPosition);
            ResetRelationshipToBall();
        }
        if (holdingBall && reason == "bat")
        {
            ballScript.BallDroppedBat(stunPosition);
            ResetRelationshipToBall();
        }
    }

    public void ResetRelationshipToBall()
    {
        ballScript = null;
        ball = null;
        holdingBall = false;
        chm.PlayerReleasedBall();
    }

    
}
