using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State { Normal, Stunned, FiredGun, Dashing, SwingAttack}
public enum StateSpeed { Accelerating, Deccelerating, Dashing, Aiming}

public class CH_Movement2 : MonoBehaviour {
    public State preState, curState;
    public StateSpeed speedState;
    private float lastx, lasty, lastAngle, stunMovementAmount, shotMovementAmount;
    public float speed, minSpeed, midSpeed, maxSpeed, accSpeed, midAccSpeed, decSpeed, minRotationSpeed, minInputVector, ballCarryAcc , ballCarryMaxSpeed, dashSpeed, dashDecSpeed, swingDashDecSpeed, dashCooldownTime, dashCooldown, bulletStunMovement, batStunMovement, shotBulletMovement;
    public bool playerMovementDisabled, carryingBall, directionInput, rightTrigger, leftTrigger, dashCooldownRunning;
    public Vector2 movementDirection, lerpedInputVector;
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

    private float xRemainder, yRemainder, magnitude;
    public Vector2 rawInputVector, inputDirection, stunDirection, shotDirection, dashDirection, previousInputVector;
    private CH_Collisions chCol;
    private CH_BallInteractions chball;
    private CH_Input chi;
    [SerializeField] private List<float> clampValues = new List<float>(4);
    [SerializeField] private List<CH_Trails> trails = new List<CH_Trails>(2);
    private string xAxis, yAxis, throwButton, hold, brake;
    private KeyCode dashKey;
    [SerializeField] Gun_Lazer gunLazer;


    // Use this for initialization
    void Start () {
        chball = GetComponent<CH_BallInteractions>();
        lastx = 0;
        lasty = 0;
        lastAngle = 0;        
        chi = GetComponent<CH_Input>();    
        xAxis = chi.xAxis;
        yAxis = chi.yAxis;
        hold = chi.hold;
        brake = chi.brake;
        dashKey = chi.dashKey;       
	}

    void FixedUpdate()
    {
        chCol = GetComponent<CH_Collisions>();
        chCol.CalculateRays();
        chi = GetComponent<CH_Input>();

        //this checks if player is inputing any movement
        if (chi.xInput != 0 || chi.yInput != 0)
        {
            directionInput = true;
        }
        else
        {
            directionInput = false;
        }

        #region states

        if (curState == State.Normal)
        {
            if (leftTrigger) //Aiming
            {
                //directionInput = false;
                Speed(StateSpeed.Aiming);
                Move(chi.xInput, chi.yInput, 0);
                gunLazer.FirinMaLazer();
            }
            else
            {
                if (directionInput) { Speed(StateSpeed.Accelerating); }
                else { Speed(StateSpeed.Deccelerating); }
                Move(chi.xInput, chi.yInput, State.Normal);
            }
        }

        else if (curState == State.Stunned)
        {
            Move(stunDirection.x, stunDirection.y, State.Stunned); //direction of impact
            if (stunMovementAmount > 0)
            {
                stunMovementAmount /= 1.25f;
                if (stunMovementAmount < 0.001f)
                {
                    stunMovementAmount = 0;
                    SetState(State.Normal);
                }
            }
            speed = 0;
        }
        else if (curState == State.FiredGun)
        {
            //if you shoot while dashing it should execute both dash and shoot movement
            if(preState == State.Dashing)
            {
                if (leftTrigger) { Speed(StateSpeed.Aiming); gunLazer.FirinMaLazer(); }
                else { Speed(StateSpeed.Dashing); }
                Speed(StateSpeed.Dashing);
                Move(chi.xInput, chi.yInput, State.Dashing);

                Move(shotDirection.x, shotDirection.y, State.FiredGun);

                if (shotMovementAmount > 0)
                {
                    shotMovementAmount /= 1.25f;
                    if (shotMovementAmount < 0.05f)
                    {
                        shotMovementAmount = 0;
                    }
                }
            }
            else
            {
                Move(shotDirection.x, shotDirection.y, State.FiredGun);
                if (shotMovementAmount > 0)
                {
                    shotMovementAmount /= 1.25f;
                    if (shotMovementAmount < 0.05f)
                    {
                        shotMovementAmount = 0;
                        SetState(State.Normal);
                    }
                }
            }
        }
        else if (curState == State.Dashing)
        {
            if (leftTrigger) { Speed(StateSpeed.Aiming); gunLazer.FirinMaLazer(); }
            else { Speed(StateSpeed.Dashing); }            
            Move(chi.xInput, chi.yInput, State.Dashing);                                  
        }

        else if(curState == State.SwingAttack)
        {
            if (preState == State.Dashing)
            {
                Speed(StateSpeed.Dashing);
                Move(chi.xInput, chi.yInput, State.SwingAttack);
                if (leftTrigger)
                {
                    gunLazer.FirinMaLazer();
                }
            }
            else
            {
                //deccelerate in last direction 
                //ignore direction input
                directionInput = false;
                Speed(StateSpeed.Deccelerating);
                Move(chi.xInput, chi.yInput, State.SwingAttack);
            }
        }
        #endregion

        #region right trigger stuff
        if (Input.GetAxisRaw(hold) > 0)
        {
            if (dashCooldown == 0 && !rightTrigger)
            {// & !rightTrigger
                Dash();
            }
            rightTrigger = true;
        }        

        //Right trigger
        if (Input.GetAxisRaw(hold) < 0.1f)
        {
            rightTrigger = false;
        }
        #endregion

        #region left trigger stuff
        if (Input.GetAxisRaw(hold) < 0)
        {
            leftTrigger = true;
        }
        else { leftTrigger = false; }
        #endregion

    }   

    public void Move(float x, float y, State mode)
    {
        bool front = chCol.front; bool back = chCol.back; bool left = chCol.left; bool right = chCol.right;
        List<float> collisionPoints = chCol.collisionPoints;

        rawInputVector = new Vector2(x, y);
        lerpedInputVector = Vector2.MoveTowards(previousInputVector, rawInputVector, Time.deltaTime * minInputVector);
        if (directionInput)
        {
            previousInputVector = lerpedInputVector;
        }
        newPos = transform.position;
        magnitude = lerpedInputVector.magnitude;
        if (magnitude > 1)
        {
            lerpedInputVector /= magnitude;            
        }

        if (mode == State.Normal)
        {
            if (directionInput)
            {
                newPos.z += lerpedInputVector.y * speed;
                newPos.x += lerpedInputVector.x * speed;
                inputDirection = lerpedInputVector;
                HeadDirection2(rawInputVector);
            }
            else if (!directionInput)
            {
                newPos.z += inputDirection.y * speed;
                newPos.x += inputDirection.x * speed;
                HeadDirection2(rawInputVector);
            }            
        }

        else if (mode == State.Stunned) 
        {
            newPos.z += stunDirection.y * stunMovementAmount;
            newPos.x += stunDirection.x * stunMovementAmount;
        }

        else if(mode == State.Dashing) 
        {
            //rather than lerp vector, check angle input is at and return vector with slight adjustment;
            dashDirection = Vector2.Lerp(dashDirection, lerpedInputVector, 0.03f).normalized;
            //newPos.z += dashDirection.y * dashMovementAmount;
            //newPos.x += dashDirection.x * dashMovementAmount;
            newPos.z += dashDirection.y * speed ;
            newPos.x += dashDirection.x * speed ;
            HeadDirection2(lerpedInputVector);
        }

        else if(mode == State.SwingAttack)
        {
            if(preState == State.Dashing)
            {

                newPos.z += dashDirection.y * speed;
                newPos.x += dashDirection.x * speed;
                HeadDirection2(rawInputVector);
                //newPos.z += dashDirection.y * dashMovementAmount;
                //newPos.x += dashDirection.x * dashMovementAmount;
            }
            else
            {
                newPos.z += previousInputVector.y * speed;
                newPos.x += previousInputVector.x * speed;
            }            
        }

        else if(mode == State.FiredGun) 
        {
            newPos.z += shotDirection.y * shotMovementAmount;
            newPos.x += shotDirection.x * shotMovementAmount;
        }

        newPos.z = Mathf.Clamp(newPos.z, chCol.collisionPoints[1], chCol.collisionPoints[0]);
        newPos.x = Mathf.Clamp(newPos.x, chCol.collisionPoints[2], chCol.collisionPoints[3]);
        clampValues = collisionPoints;
        gameObject.transform.position = newPos;

        PositionAutoCorrect(chCol.frontColPoint, chCol.backColPoint, chCol.leftColPoint, chCol.rightColPoint, rawInputVector);               
    }

    public void AccDec()
    {
        if (directionInput && !carryingBall)
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
        else if (directionInput && carryingBall)
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
            else if (Input.GetAxis(hold) < 0)
            {
                if (speed > 0)
                {
                    speed -= decSpeed * 2;
                }
                else
                {
                    speed = 0;
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

    public void Speed(StateSpeed state)
    {
        speedState = state;

        if(state == StateSpeed.Accelerating)
        {
            if (!carryingBall)
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
            else if (carryingBall)
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
        }

        else if(state == StateSpeed.Deccelerating)
        {
            if (curState == State.Normal)
            {
                if (speed > 0)
                {
                    speed -= decSpeed;
                }
                else
                {
                    speed = 0;
                }
            }
            else if(curState == State.SwingAttack)
            {
                if(preState == State.Dashing)
                {
                    if (speed > 0)
                    {
                        speed -= decSpeed/2;
                    }
                    else
                    {
                        speed = 0;
                    }
                }
                else
                {
                    if (speed > 0)
                    {
                        speed -= decSpeed * 8;
                    }
                    else
                    {
                        speed = 0;
                    }
                }
                
            }
        }

        else if(state == StateSpeed.Dashing)
        {            
            //speed gets set higher than max speed for the dash
            if (speed > maxSpeed)
            {
                speed -= dashDecSpeed;
            }
            else
            {
                SetState(State.Normal);
            }
        }

        else if(state == StateSpeed.Aiming)
        {
            //deccelerate only
            if (speed > 0)
            {
                speed -= decSpeed * 2;
            }
            else
            {
                speed = 0;
            }            
        }
    }

    public void Dash()
    {
        if (carryingBall)
        {
            chball.DropBall(head.forward, "dash");
            carryingBall = false;
        }       

        dashDirection = DegreeToVector2(head.eulerAngles.y);
        dashAngleDebug = dashDirection;
        speed = dashSpeed;
        dashCooldown = dashCooldownTime;
        SetState(State.Dashing);
        if (!dashCooldownRunning)
        {
            StartCoroutine("DashCoolDown");
        }
    }

    public IEnumerator DashCoolDown()
    {
        dashCooldownRunning = true;
        foreach (CH_Trails t in trails)
        {
            t.Dashing();
        }       
        
        while (dashCooldown > 0)
        {
            dashCooldown -= 0.01f;
            if(speed <= maxSpeed)
            {
                foreach (CH_Trails t in trails)
                {
                    t.CoolingDown();
                }
            }
            yield return new WaitForSeconds(0.01f);
        }
        
        foreach (CH_Trails t in trails)
        {
            t.Ready();
        }        
        dashCooldown = 0;
        dashCooldownRunning = false;
    }

    private void CancelDash()
    {
        //StartCoroutine("DashCoolDown");
        //foreach (CH_Trails t in trails)
        //{
        //    t.Ready();
        //}        
    }

    public void MoveYouGotShot(Vector3 velocity)
    {
        stunDirection = new Vector2(velocity.x, velocity.z).normalized;
        stunMovementAmount = bulletStunMovement;
        SetState(State.Stunned);
    }

    public void MoveYouJustShot(Vector3 bulletDirection)
    {
        shotDirection = new Vector2(bulletDirection.x, bulletDirection.z).normalized * -1;
        shotMovementAmount = shotBulletMovement;
        SetState(State.FiredGun);
    }

    public void MoveYouGotWhackedByABat(Vector3 positionOfHitter)
    {
        //stunned direction is used for calculating the direction the player who got hit should move
        stunDirection.x = (positionOfHitter.x - transform.position.x) * -1;
        stunDirection.y = (positionOfHitter.z - transform.position.z) * -1;
        stunDirection = stunDirection.normalized;
        GetComponent<CH_BallInteractions>().DropBall(positionOfHitter, "bat");
        stunMovementAmount = batStunMovement;
        SetState(State.Stunned);
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

        if (lastx == x && lasty == y) {}
        else
        {
            if (x < 0)
            {
                angle = 360 + Vector2.SignedAngle(new Vector2(x, y), Vector2.up);
                #region old angle stuff
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
                #endregion
                head.eulerAngles = new Vector3(0, angle, 0);
                lastAngle = angle;
            }
            else
            {
                angle = Vector2.Angle(new Vector2(x, y), Vector2.up);
                #region old angle stuff
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
                #endregion
                head.eulerAngles = new Vector3(0, angle, 0);
                lastAngle = angle;
            }
        }
    }

    void HeadDirection2(Vector2 rawInput)
    {
        if (rawInput.magnitude > 0.05)
        {
            float x = rawInput.x;
            float y = rawInput.y;
            Vector3 relPos = Quaternion.AngleAxis(Mathf.Atan2(x, -y * -1f) * Mathf.Rad2Deg, transform.up) * Vector3.forward;
            Quaternion rotation = Quaternion.LookRotation(relPos, Vector3.up);
            Quaternion tr = Quaternion.Slerp(head.rotation, rotation, Time.deltaTime * minRotationSpeed);
            head.rotation = tr;
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

    public void SetState(State _to)
    {
        preState = curState;
        curState = _to;

        //if (preState == State.Dashing)
        //{
        //    CancelDash();
        //}

        // do anything else?
        switch (_to)
        {
            case State.Normal:
                break;
            case State.Stunned:
                break;
            case State.FiredGun:
                break;
            case State.Dashing:
                break;
            case State.SwingAttack:
                break;
        }
    }

}
