using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CH_Movement2 : MonoBehaviour {

    private float lastx, lasty, lastAngle, movementAmount;
    public float speed, runningWithBallSpeed, bulletStunMovement, batStunMovement, shotBulletMovement;
    public bool stunned, shotBullet;
    public Vector2 movementDirection;
    public float acc;
    public float skinDepth;
    public float autoCorrectDistance = 0.5f;
    public float autoCorrectAmount= 0.01f;
    public float autoCorrectDeadZone;
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

    private string xAxis, yAxis, throwButton, hold;

    private int joystickInvert;


    // Use this for initialization
    void Start () {

        lastx = 0;
        lasty = 0;
        lastAngle = 0;
        
        CH_Input chi = GetComponent<CH_Input>();    
        xAxis = chi.xAxis;
        yAxis = chi.yAxis;
        hold = chi.hold;

        if (chi.joystick)
        {
            joystickInvert = -1;
        }
        else
        {
            joystickInvert = 1;
        }        
	}

    void FixedUpdate()
    {
        chCol = GetComponent<CH_Collisions>();
        chCol.CalculateRays();        

        if (stunned)
        {
            Move(movementDirection.x, movementDirection.y, 1); //direction of impact
            if (movementAmount > 0)
            {
                movementAmount /= 1.25f;
                if (movementAmount < 0.001f)
                {
                    movementAmount = 0;
                    stunned = false;
                }
            }
        }

        else if (shotBullet)
        {
            Move(movementDirection.x, movementDirection.y, 1); //direction of impact
            if (movementAmount > 0)
            {
                movementAmount /= 1.25f;
                if (movementAmount < 0.01f)
                {
                    movementAmount = 0;
                    shotBullet = false;
                }
            }
        }
        if (!stunned && !shotBullet)
        {
            if (Input.GetAxisRaw(hold) > 0)
            {
                HeadDirection(new Vector2(Input.GetAxisRaw(xAxis), Input.GetAxisRaw(yAxis)));
            }
            else
            {
                Move(Input.GetAxisRaw(xAxis), Input.GetAxisRaw(yAxis), 0);
                HeadDirection(new Vector2(Input.GetAxisRaw(xAxis), Input.GetAxisRaw(yAxis)));
            }
        }
    }

    public void MoveYouGotShot(Vector3 velocity)
    {
        //Vector3 forward = t.rotation * Vector3.back;
        movementDirection = new Vector2(velocity.x, velocity.z).normalized;
        movementAmount = bulletStunMovement;
        stunned = true;
    }

    public void MoveYouJustShot(Vector3 bulletDirection)
    {
        movementDirection = new Vector2(bulletDirection.x, bulletDirection.z).normalized * -1;
        shotBullet = true;
        movementAmount = shotBulletMovement;
    }


    public void MoveYouGotWhackedByABat(Vector3 positionOfHitter)
    {
        //stunned direction is used for calculating the direction the player who got hit should move
        movementDirection.x =  (positionOfHitter.x - transform.position.x) *-1;
        movementDirection.y = (positionOfHitter.z - transform.position.z) * -1;
        movementDirection = movementDirection.normalized;
        GetComponent<CH_BallInteractions>().DropBall(positionOfHitter, "bat");        
        movementAmount = batStunMovement;
        stunned = true;
    }


    public void Move(float x, float y, int mode)
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

        if (mode == 0) //normal movement
        {
            newPos.z += inputVector.y * speed;
            newPos.x += inputVector.x * speed;
        }
        else if (mode == 1) //moves the player against their will
        {
            newPos.z += inputVector.y * movementAmount;
            newPos.x += inputVector.x * movementAmount;
        }
        
        newPos.z = Mathf.Clamp(newPos.z, chCol.collisionPoints[1], chCol.collisionPoints[0]);
        newPos.x = Mathf.Clamp(newPos.x, chCol.collisionPoints[2], chCol.collisionPoints[3]);
        clampValues = collisionPoints;
        

        gameObject.transform.position = newPos;

        if (stunned || shotBullet) { }
        else
        {
            HeadDirection(inputVector);
        }

        PositionAutoCorrect(chCol.frontColPoint, chCol.backColPoint, chCol.leftColPoint, chCol.rightColPoint, rawInputVector);               
    }

    


    void PositionAutoCorrect(float front, float back, float left, float right, Vector2 rawinputVector)
    {
        //if player is too close to a wall, push them away a little till they're not too close
        float f = front - transform.position.z ;
        float b = back + transform.position.z;
        float l = left + transform.position.x ;
        float r = right - transform.position.x ;

        //front
        if (f <= autoCorrectDistance && Mathf.Abs(rawinputVector.y) < autoCorrectDeadZone)
        {
            transform.position -= new Vector3(0, 0, autoCorrectAmount) * Time.deltaTime;
        }
        //back
        if (transform.position.z <= (back + autoCorrectDistance) && Mathf.Abs(rawinputVector.y) < autoCorrectDeadZone)
        {
            transform.position += new Vector3(0, 0, autoCorrectAmount) * Time.deltaTime;
        }
        //left
        if (transform.position.x <= (left + autoCorrectDistance) && Mathf.Abs(rawinputVector.x) < autoCorrectDeadZone)
        {
            transform.position += new Vector3(autoCorrectAmount, 0, 0) * Time.deltaTime;
        }
        //right
        if (r <= autoCorrectDistance && Mathf.Abs(rawinputVector.x) < autoCorrectDeadZone)
        {
            transform.position -= new Vector3(autoCorrectAmount, 0, 0) * Time.deltaTime;
        }
    }

    void HeadDirection(Vector2 rawInput)
    {
        //float angleCutoff = 20;
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
                //if (angle>lastAngle-360)
                //{

                //}
                //else if (angle > lastAngle + angleCutoff)
                //{
                //    angle = lastAngle + angleCutoff;
                //}
                //else if( angle < lastAngle - angleCutoff)
                //{
                //    angle = lastAngle - angleCutoff;
                //}
                
                head.eulerAngles = new Vector3(0, angle, 0);
                lastAngle = angle;
            }
            else
            {

                angle = Vector2.Angle(new Vector2(x, y), Vector2.up);
                //if (angle > lastAngle - 360)
                //{

                //}
                //else if (angle > lastAngle + angleCutoff)
                //{
                //    angle = lastAngle + angleCutoff;
                //}
                //else if (angle < lastAngle - angleCutoff)
                //{
                //    angle = lastAngle - angleCutoff;
                //}
                head.eulerAngles = new Vector3(0, angle, 0);
                lastAngle = angle;
            }
        }
    }
	
	
}
