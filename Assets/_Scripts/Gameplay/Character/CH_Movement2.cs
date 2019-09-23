using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CH_Movement2 : MonoBehaviour {    
    private float lastx, lasty, lastAngle, movementAmount;
    public float speed, minSpeed, midSpeed, maxSpeed, accSpeed, midAccSpeed, decSpeed, ballCarryAcc , ballCarryMaxSpeed, dashMovement, bulletStunMovement, batStunMovement, shotBulletMovement;
    public bool stunned, shotBullet, playerMovementDisabled, carryingBall;
    public Vector2 movementDirection;
    public float skinDepth;
    public float autoCorrectDistance = 0.5f;
    public float autoCorrectAmount= 0.01f;
    public float autoCorrectDeadZone;
    public float headTurnTolerance;
    public Transform head;
    public Vector2 tempHeadRotation = new Vector2 (0,0);
    public float headAngle;
    public Vector2 dashAngleDebug;
    
    public Transform gunEnd;
    public GameObject bullet;
    public float bulletSpeed;

    private Vector3 currentPos;
    private Vector3 newPos;

    private float xRemainder;
    private float yRemainder;
    private Vector2 inputVector, inputDirection;
    private CH_Collisions chCol;
    public float magnitude;
    [SerializeField] private List<float> clampValues = new List<float>(4);

    private string xAxis, yAxis, throwButton, hold;
    private KeyCode dashKey;

    private int joystickInvert;
    private bool playerInput, dashing;


    // Use this for initialization
    void Start () {

        lastx = 0;
        lasty = 0;
        lastAngle = 0;
        
        CH_Input chi = GetComponent<CH_Input>();    
        xAxis = chi.xAxis;
        yAxis = chi.yAxis;
        hold = chi.hold;
        dashKey = chi.dashKey;

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

        else if (dashing)
        {
            Move(movementDirection.x, movementDirection.y, 1); //direction of impact
            if (movementAmount > 0)
            {
                movementAmount /= 1.1f;
                if (movementAmount < 0.01f)
                {
                    movementAmount = 0;
                    dashing = false;
                }
            }
        }

        //this checks if player is inputing any movement
        if (Input.GetAxisRaw(xAxis) != 0 || Input.GetAxisRaw(yAxis) !=0)
        {
            playerInput = true;
        }
        else
        {
            playerInput = false;
        }

        if (!playerMovementDisabled)
        {
            if (!stunned && !shotBullet)
            {
                if (Input.GetAxisRaw(hold) > 0)
                {
                    AccDec();
                    playerInput = false;
                    Move2(inputDirection.x, inputDirection.y, 1);
                    HeadDirection(new Vector2(Input.GetAxisRaw(xAxis), Input.GetAxisRaw(yAxis)));
                }
                else
                {
                    AccDec();
                    Move2(Input.GetAxisRaw(xAxis), Input.GetAxisRaw(yAxis), 0);
                    HeadDirection(new Vector2(Input.GetAxisRaw(xAxis), Input.GetAxisRaw(yAxis)));
                }
            }
        }

        if (Input.GetKeyDown(dashKey))
        {
            MoveYouJustDashed();
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

    public void MoveYouJustDashed()
    {
        float y = 0;
        if (transform.rotation.eulerAngles.y > 0)
        {
            y = 1;
        }
        else if(transform.rotation.eulerAngles.y < 0)
        {
            y = -1;
        }
        movementDirection = DegreeToVector2(head.eulerAngles.y);
        dashAngleDebug = movementDirection;

        dashing = true;
        movementAmount = dashMovement;
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

    public void Move2(float x, float y, int mode)
    {
        bool front = chCol.front; bool back = chCol.back; bool left = chCol.left; bool right = chCol.right;
        List<float> collisionPoints = chCol.collisionPoints;

        inputVector = new Vector2(x, y);

        Vector2 rawInputVector = inputVector;
        magnitude = inputVector.magnitude;

        if (inputVector.magnitude > 1)
        {
            inputVector/= inputVector.magnitude;            
        }

        newPos = transform.position;

        if (playerInput)
        {
            //input direction is used to store player input direction
            //I use it below for recalling the last inputed direction when the player isn't inputing a direction
            inputDirection = inputVector.normalized;
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
        }
        else if(!playerInput)
        {
            if (mode == 0) //normal movement
            {
                newPos.z += inputDirection.y * speed;
                newPos.x += inputDirection.x * speed;
            }
            else if (mode == 1) //moves the player against their will
            {
                newPos.z += inputDirection.y * movementAmount;
                newPos.x += inputDirection.x * movementAmount;
            }

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

    public void AccDec()
    {
        if (playerInput && !carryingBall)
        {
            if (speed < maxSpeed)
            {
                if (speed < minSpeed)
                {
                    speed = minSpeed;
                }
                else if (speed < midSpeed)
                {
                    speed *= accSpeed;
                }
                else if (speed > midSpeed)
                {
                    speed *= midAccSpeed;
                }

            }
            else
            {
                speed = maxSpeed;
            }
        }
        else if (playerInput && carryingBall)
        {
            if (speed < ballCarryMaxSpeed)
            {
                if (speed < minSpeed)
                {
                    speed = minSpeed;
                }
                else if (speed < ballCarryMaxSpeed)
                {
                    speed *= ballCarryAcc;
                }
                else if (speed > midSpeed)
                {
                    speed *= midAccSpeed;
                }

            }
            else
            {
                speed = ballCarryMaxSpeed;
            }
        }
        else
        {
            if(speed > 0)
            {
                speed -= decSpeed;
            }
            if(speed < 0)
            {
                speed = 0;
            }
            
        }
    }

    void PositionAutoCorrect(float front, float back, float left, float right, Vector2 rawinputVector)
    {
        //if player is too close to a wall, push them away a little till they're not too close
        float f = front - transform.position.z;
        float b = back + transform.position.z;
        float l = left + transform.position.x;
        float r = right - transform.position.x;

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

    public void PlayerHoldingBall()
    {
        carryingBall = true;
    }

    public void PlayerReleasedBall()
    {
        carryingBall = false;
    }

    public static Vector2 RadianToVecor2(float radian)
    {
        return new Vector2(-Mathf.Cos(radian), Mathf.Sin(radian));
    }

    public static Vector2 DegreeToVector2(float degree)
    {
        float angle = degree + 90;
        return RadianToVecor2(angle * Mathf.Deg2Rad);
    }
	
	
}
