﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CH_Movement2 : MonoBehaviour {    
    private float lastx, lasty, lastAngle, stunMovementAmount, shotMovementAmount, dashMovementAmount;
    public float speed, minSpeed, midSpeed, maxSpeed, accSpeed, midAccSpeed, decSpeed, ballCarryAcc , ballCarryMaxSpeed, dashMovement, dashCooldown, dashCooldownTime, bulletStunMovement, batStunMovement, shotBulletMovement;
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
    public Vector2 inputVector, inputDirection, stunDirection, shotDirection, dashDirection;
    private CH_Collisions chCol;
    public float magnitude;
    [SerializeField] private List<float> clampValues = new List<float>(4);
    [SerializeField] private List<CH_Trails> trails = new List<CH_Trails>(2);

    private string xAxis, yAxis, throwButton, hold;
    private KeyCode dashKey;

    private int joystickInvert;
    public bool playerInput, dashing, holdHeld;


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

            Move2(stunDirection.x, stunDirection.y, 1); //direction of impact
            if (stunMovementAmount > 0)
            {
                stunMovementAmount /= 1.25f;
                if (stunMovementAmount < 0.001f)
                {
                    stunMovementAmount = 0;
                    stunned = false;
                }
            }
            speed = 0;
        }
        else
        {
            if (shotBullet)
            {
                
                Move2(shotDirection.x, shotDirection.y, 3); //direction of impact
                if (shotMovementAmount > 0)
                {
                    shotMovementAmount /= 1.25f;
                    if (shotMovementAmount < 0.05f)
                    {
                        shotMovementAmount = 0;
                        shotBullet = false;
                    }
                }
            }

            if (dashing)
            {                
                Move2(Input.GetAxisRaw(xAxis), Input.GetAxisRaw(yAxis), 2); //direction of impact
                HeadDirection(new Vector2(Input.GetAxisRaw(xAxis), Input.GetAxisRaw(yAxis)));
                if (dashMovementAmount > 0)
                {
                    if (carryingBall)
                    {
                        
                        dashMovementAmount /= 1.2f;
                    }
                    dashMovementAmount /= 1.05f;
                    if (dashMovementAmount < 0.1f)
                    {
                        dashing = false;
                        dashMovementAmount = 0;
                        StartCoroutine("DashCoolDown");

                    }
                }
                speed = maxSpeed/2;
            }
        }
        

        //this checks if player is inputing any movement
        if (Input.GetAxisRaw(xAxis) != 0 || Input.GetAxisRaw(yAxis) != 0)
        {
            playerInput = true;
        }
        else
        {
            playerInput = false;
        }

        if (!playerMovementDisabled)
        {
            if (!stunned && !shotBullet && !dashing)
            {
                if (Input.GetAxisRaw(hold) > 0)
                {
                    //playerInput = false;
                    //AccDec();                    
                    //Move2(inputDirection.x, inputDirection.y, 0);
                    //HeadDirection(new Vector2(Input.GetAxisRaw(xAxis), Input.GetAxisRaw(yAxis)));
                }
                else
                {
                    
                }
                AccDec();
                Move2(Input.GetAxisRaw(xAxis), Input.GetAxisRaw(yAxis), 0);
                HeadDirection(new Vector2(Input.GetAxisRaw(xAxis), Input.GetAxisRaw(yAxis)));
            }
        }

        if (Input.GetAxisRaw(hold) > 0 &! holdHeld)
        {
            
            if (dashCooldown == 0)
            {
                Dash();
                foreach (CH_Trails t in trails)
                {
                    t.Dashing();
                }
            }
            holdHeld = true;
        }

        if(Input.GetAxisRaw(hold) < 0.05)
        {
            holdHeld = false;
        }
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

        if (mode == 0) //normal movement
        {
            if (playerInput)
            {
                newPos.z += inputVector.y * speed;
                newPos.x += inputVector.x * speed;
                inputDirection = inputVector;
            }
            else if (!playerInput)
            {
                newPos.z += inputDirection.y * speed;
                newPos.x += inputDirection.x * speed;
            }
        }

        else if (mode == 1) //Stun
        {
            newPos.z += stunDirection.y * stunMovementAmount;
            newPos.x += stunDirection.x * stunMovementAmount;
        }

        else if(mode == 2) //Dash
        {
            //rather than lerp vector, check angle input is at and return vector with slight adjustment;
            dashDirection = Vector2.Lerp(dashDirection, inputVector, 0.02f);
            newPos.z += dashDirection.y * dashMovementAmount;
            newPos.x += dashDirection.x * dashMovementAmount;
        }

        else if(mode == 3) //Shot a bullet
        {
            //rather than lerp vector, check angle input is at and return vector with slight adjustment;
            newPos.z += shotDirection.y * shotMovementAmount;
            newPos.x += shotDirection.x * shotMovementAmount;
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
        //if player is carrying ball, speed and acceleration are different
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
                    speed *= accSpeed;
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
        else //else decelerate
        {
            if(Input.GetAxis(hold) > 0)
            {
                if (speed > 0)
                {
                    speed -= decSpeed/2;
                }
            }
            else
            {
                if (speed > 0)
                {
                    speed -= decSpeed;
                }                
            }
            if (speed < 0)
            {
                speed = 0;
            }


        }
    }

    public void Dash()
    {
        dashDirection = DegreeToVector2(head.eulerAngles.y);
        dashAngleDebug = dashDirection;

        dashing = true;
        dashMovementAmount = dashMovement;
        dashCooldown = dashCooldownTime;
    }

    public void MoveYouGotShot(Vector3 velocity)
    {
        stunDirection = new Vector2(velocity.x, velocity.z).normalized;
        stunMovementAmount = bulletStunMovement;
        stunned = true;
    }

    public void MoveYouJustShot(Vector3 bulletDirection)
    {
        shotDirection = new Vector2(bulletDirection.x, bulletDirection.z).normalized * -1;
        shotBullet = true;
        shotMovementAmount = shotBulletMovement;
    }

    public void MoveYouGotWhackedByABat(Vector3 positionOfHitter)
    {
        //stunned direction is used for calculating the direction the player who got hit should move
        stunDirection.x = (positionOfHitter.x - transform.position.x) * -1;
        stunDirection.y = (positionOfHitter.z - transform.position.z) * -1;
        stunDirection = stunDirection.normalized;
        GetComponent<CH_BallInteractions>().DropBall(positionOfHitter, "bat");
        stunMovementAmount = batStunMovement;
        stunned = true;
    }

    

    public IEnumerator DashCoolDown()
    {
        foreach (CH_Trails t in trails)
        {
            t.CoolingDown();
        }
        while (dashCooldown > 0)
        {
            dashCooldown -= 0.25f;
            yield return new WaitForSeconds(0.25f);
        }
        foreach (CH_Trails t in trails)
        {
            t.Ready();
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
