using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CH_Movement2 : MonoBehaviour {

    private float lastx, lasty, lastAngle;
    public float speed, stunMovement;
    private float stunMovementAmount;
    public bool stunned;
    public Vector2 stunnedDirection;
    public float acc;
    public float skinDepth;
    public float autoCorrectDistance = 0.5f;
    public float autoCorrectAmount= 0.01f;
    public float headTurnTolerance;
    public Transform head;
    public Vector2 tempHeadRotation = new Vector2 (0,0);
    public float headAngle;
    


    public Transform gunEnd;
    public GameObject bullet;
    public float bulletSpeed;


    private Vector3 currentPos;
    private Vector3 newPos;

    private float xRemainder;
    private float yRemainder;
    private Vector2 inputVector;
    private CH_Collisions chCol;
    public float magnitude;
    [SerializeField]
    private List<float> clampValues = new List<float>(4);

    private string xAxis;
    private string yAxis;
    private string throwButton;
    private int joystickInvert;


    // Use this for initialization
    void Start () {

        lastx = 0;
        lasty = 0;
        lastAngle = 0;
        
        CH_Input chi = GetComponent<CH_Input>();    
        xAxis = chi.xAxis;
        yAxis = chi.yAxis;
        if (chi.joystick)
        {
            joystickInvert = -1;
        }
        else
        {
            joystickInvert = 1;
        }        
	}

    // Update is called once per frame
    void FixedUpdate()
    {
        chCol = GetComponent<CH_Collisions>();
        chCol.CalculateRays();        

        if (stunned)
        {
            Move(stunnedDirection.x, stunnedDirection.y, false); //direction of impact
            if (stunMovementAmount > 0)
            {
                stunMovementAmount /= 2f;
                if (stunMovementAmount < 0.0001f)
                {
                    stunMovementAmount = 0;
                    stunned = false;
                }
            }
        }
        if (!stunned)
        {
            Move(Input.GetAxisRaw(xAxis), Input.GetAxisRaw(yAxis), true);
        }
    }

    public void MoveYouGotStunned(Vector3 velocity)
    {
        //Vector3 forward = t.rotation * Vector3.back;
        stunnedDirection = new Vector2(velocity.x, velocity.z).normalized;
        stunned = true;
        stunMovementAmount = stunMovement;
    }

    public void Move(float x, float y, bool mode)
    {
        bool front = chCol.front; bool back = chCol.back; bool left = chCol.left; bool right = chCol.right;
        List<float> collisionPoints = chCol.collisionPoints;

        inputVector = new Vector2(x, y);
        Vector2 rawInputVector = inputVector;
        Vector2 sides = new Vector2(Mathf.Sign(x), Mathf.Sign(y));
        magnitude = inputVector.magnitude;

        #region Facing Direction Assist
        //if player is pushing against wall and a collision is being registered, stop registering input
        //This code foces the player to look adjacent to the wall they're walking towards

        //if (inputVector.y > 0 && transform.position.z >= collisionPoints[0])
        //{
        //    inputVector.y = 0;
        //}
        //if (inputVector.y < 0 && transform.position.z <= collisionPoints[1])
        //{
        //    inputVector.y = 0;
        //}
        //if (inputVector.x < 0 && transform.position.x <= collisionPoints[2])
        //{
        //    inputVector.x = 0;
        //}
        //if (inputVector.x > 0 && transform.position.x >= collisionPoints[3])
        //{
        //    inputVector.x = 0;
        //}
        #endregion


        if (inputVector.magnitude > 1)
        {
            inputVector/= inputVector.magnitude;            
        }

        newPos = transform.position;

        if (mode) //this is used for normal movement
        {
            newPos.z += inputVector.y * speed;
            newPos.x += inputVector.x * speed;
        }
        else if (!mode) //this is used for moving the player against their will
        {
            newPos.z += inputVector.y * stunMovementAmount;
            newPos.x += inputVector.x * stunMovementAmount;
        }
        
        newPos.z = Mathf.Clamp(newPos.z, chCol.collisionPoints[1], chCol.collisionPoints[0]);
        newPos.x = Mathf.Clamp(newPos.x, chCol.collisionPoints[2], chCol.collisionPoints[3]);
        clampValues = collisionPoints;
        

        gameObject.transform.position = newPos;
        PositionAutoCorrect(chCol.frontColPoint, chCol.backColPoint, chCol.leftColPoint, chCol.rightColPoint, rawInputVector);
        HeadDirection(inputVector);
    }

    


    void PositionAutoCorrect(float front, float back, float left, float right, Vector2 rawinputVector)
    {
        //if player is too close to a wall, push them away a little till they're not too close
        float f = front - transform.position.z ;
        //float b = transform.position
        float l = left + transform.position.x ;
        float r = right - transform.position.x ;

        //front
        if (f <= autoCorrectDistance && rawinputVector.y == 0)
        {
            transform.position -= new Vector3(0, 0, autoCorrectAmount) * Time.deltaTime;
        }
        //back
        if (transform.position.z <= (back + autoCorrectDistance) && rawinputVector.y == 0)
        {
            transform.position += new Vector3(0, 0, autoCorrectAmount) * Time.deltaTime;
        }
        //left
        if (transform.position.x <= (left + autoCorrectDistance) && rawinputVector.x == 0)
        {
            transform.position += new Vector3(autoCorrectAmount, 0, 0) * Time.deltaTime;
        }
        if (r <= autoCorrectDistance && rawinputVector.x == 0)
        {
            transform.position -= new Vector3(autoCorrectAmount, 0, 0) * Time.deltaTime;
        }
    }

    void HeadDirection(Vector2 rawInput)
    {
        float angleCutoff = 20;
        float x = rawInput.x;
        float y = rawInput.y;
        float angle = 0;

        if (lastx == x && lasty == y)
        {
            
        }
        else
        {
            if (x < 0)
            {
                angle = 360 + Vector2.SignedAngle(new Vector2(x, y), Vector2.up);
                if (angle>lastAngle-360)
                {

                }
                else if (angle > lastAngle + angleCutoff)
                {
                    angle = lastAngle + angleCutoff;
                }
                else if( angle < lastAngle - angleCutoff)
                {
                    angle = lastAngle - angleCutoff;
                }
                
                head.eulerAngles = new Vector3(0, angle, 0);
                lastAngle = angle;
            }
            else
            {

                angle = Vector2.Angle(new Vector2(x, y), Vector2.up);
                if (angle > lastAngle - 360)
                {

                }
                else if (angle > lastAngle + angleCutoff)
                {
                    angle = lastAngle + angleCutoff;
                }
                else if (angle < lastAngle - angleCutoff)
                {
                    angle = lastAngle - angleCutoff;
                }
                head.eulerAngles = new Vector3(0, angle, 0);
                lastAngle = angle;
            }
        }
    }
	
	
}
