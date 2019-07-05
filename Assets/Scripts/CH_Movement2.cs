using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CH_Movement2 : MonoBehaviour {

    public float lastx, lasty, lastAngle;
    public float speed;
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




    //handle horisontal and vertical input axes separately
    //take input as raw and use an acceleration model to speed up over time


	// Use this for initialization
	void Awake () {
        lastx = 0;
        lasty = 0;
        lastAngle = 0;
        chCol = GetComponent<CH_Collisions>();
	}

    // Update is called once per frame
    void FixedUpdate()
    {
        
        Move(Input.GetAxisRaw("horizontal"), Input.GetAxisRaw("vertical"), transform.position, chCol.front, chCol.back, chCol.left, chCol.right, chCol.collisionPoints);
        Shoot(Input.GetKey(KeyCode.Space));
    }

    private void Shoot(bool v)
    {
        GameObject bullety = Instantiate(bullet, gunEnd.position, Quaternion.identity, transform);
        bullety.transform.parent = null;
        bullety.GetComponent<Rigidbody>().AddForce(transform.forward * bulletSpeed);


    }

    public void Move(float x, float y, Vector3 currentPosition, bool front, bool back, bool left, bool right, List<float> collisionPoints)
    {
        inputVector = new Vector2(x, y);
        Vector2 rawInputVector = inputVector;
        Vector2 sides = new Vector2(Mathf.Sign(x), Mathf.Sign(y));
        magnitude = inputVector.magnitude;


        //these are clamping my character!!! NO!
        //if player is pushing against wall and a collision is being registered, stop registering input

        if (inputVector.y > 0 && transform.position.z >= collisionPoints[0])
        {
            inputVector.y = 0;
        }
        if (inputVector.y < 0 && transform.position.z <= collisionPoints[1])
        {
            inputVector.y = 0;
        }
        if (inputVector.x < 0 && transform.position.x <= collisionPoints[2])
        {
            inputVector.x = 0;
        }
        if (inputVector.x > 0 && transform.position.x >= collisionPoints[3])
        {
            inputVector.x = 0;
        }


        if (inputVector.magnitude > 1)
        {
            inputVector/= inputVector.magnitude;            
        }

        newPos = currentPosition;
        float dt = 1;
        newPos.z += inputVector.y * dt * speed;
        newPos.x += inputVector.x * dt * speed;        


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
        //if no input

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
        

        
        

 

        //head.eulerAngles = new Vector3(0,Vector2.Angle(new Vector2(rawInput.x , rawInput.y), Vector2.up), 0);
        
    }
	
	
}
