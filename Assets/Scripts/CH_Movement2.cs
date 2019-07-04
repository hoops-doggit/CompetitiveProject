using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CH_Movement2 : MonoBehaviour {

    public float speed;
    public float acc;
    public float autoCorrectDistance = 0.5f;
    public float autoCorrectAmount= 0.01f;

    private Rigidbody rb;
    private Vector3 currentPos;
    private Vector3 newPos;

    private float xRemainder;
    private float yRemainder;
    private Vector2 inputVector;
    private CH_Collisions chCol;
    public float magnitude;


    //handle horisontal and vertical input axes separately
    //take input as raw and use an acceleration model to speed up over time


	// Use this for initialization
	void Awake () {
        rb = GetComponent<Rigidbody>();
        chCol = GetComponent<CH_Collisions>();
	}

    // Update is called once per frame
    void FixedUpdate()
    {
        Move(Input.GetAxisRaw("horizontal"), Input.GetAxisRaw("vertical"), transform.position, chCol.front, chCol.back, chCol.left, chCol.right);
        
    }

    public void Move(float x, float y, Vector3 currentPosition, bool front, bool back, bool left, bool right)
    {
        inputVector = new Vector2(x, y);
        Vector2 rawInputVector = inputVector;
        Vector2 sides = new Vector2(Mathf.Sign(x), Mathf.Sign(y));
        magnitude = inputVector.magnitude;

        if (inputVector.y > 0 && front)
        {
            inputVector.y = 0;
        }
        if (inputVector.y < 0 && back)
        {
            inputVector.y = 0;
        }
        if (inputVector.x > 0 && right)
        {
            inputVector.x = 0;
        }
        if (inputVector.x < 0 && left)
        {
            inputVector.x = 0;
        }


        if (inputVector.magnitude > 1)
        {
            inputVector/= inputVector.magnitude;            
        }

        newPos = currentPosition;
        float dt = Time.deltaTime;
        newPos.x += inputVector.x * dt * speed;
        newPos.z += inputVector.y * dt * speed;

        newPos.x = Mathf.Clamp(newPos.x, chCol.leftColPoint, chCol.rightColPoint);
        newPos.z = Mathf.Clamp(newPos.z, chCol.backColPoint, chCol.frontColPoint);

        gameObject.transform.position = newPos;
        PositionAutoCorrect(chCol.frontColPoint, chCol.backColPoint, chCol.leftColPoint, chCol.rightColPoint, rawInputVector);
    }

    void PositionAutoCorrect(float front, float back, float left, float right, Vector2 rawinputVector)
    {
        //if player is too close to a wall, push them away a little till they're not too close
        float f = front - transform.position.z ;
        //float b = transform.position
        float l = left + transform.position.x ;
        float r = right - transform.position.x ;

        if (f < autoCorrectDistance && rawinputVector.y == 0)
        {
            transform.position -= new Vector3(0, 0, autoCorrectAmount) * Time.deltaTime;
        }
        if (transform.position.z <= (back + autoCorrectDistance) && rawinputVector.y == 0)
        {
            transform.position += new Vector3(0, 0, autoCorrectAmount) * Time.deltaTime;
        }
        if (transform.position.x <= (left + autoCorrectDistance) && rawinputVector.x == 0)
        {
            transform.position += new Vector3(autoCorrectAmount, 0, 0) * Time.deltaTime;
        }
        if (r < autoCorrectDistance && rawinputVector.x == 0)
        {
            transform.position -= new Vector3(autoCorrectAmount, 0, 0) * Time.deltaTime;
        }
    }
	
	
}
