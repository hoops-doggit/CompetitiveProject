using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CH_Movement2 : MonoBehaviour {

    public float speed;
    public float acc;

    private Rigidbody rb;
    private Vector3 currentPos;
    private Vector3 newPos;

    private float xRemainder;
    private float yRemainder;
    private Vector2 inputVector;
    private CH_Collisions chCol;


    //handle horisontal and vertical input axes separately
    //take input as raw and use an acceleration model to speed up over time


	// Use this for initialization
	void Awake () {
        rb = GetComponent<Rigidbody>();
        chCol = GetComponent<CH_Collisions>();
	}

    public void Move(float x, float y, Vector3 gameObjectPosition)
    {
        inputVector = new Vector2(x, y);
        Vector2 sides = new Vector2(Mathf.Sign(x), Mathf.Sign(y));
        if (inputVector.magnitude > 1 || inputVector.magnitude <-1)
        {
            inputVector = inputVector.normalized * sides;            
        }

        //do ray casts
        newPos = gameObjectPosition;
        float dt = Time.deltaTime;
        newPos.x += inputVector.x * dt * speed;
        newPos.z += inputVector.y * dt * speed;

        newPos.x = Mathf.Clamp(newPos.x, chCol.leftColPoint, chCol.rightColPoint);
        newPos.z = Mathf.Clamp(newPos.z, chCol.backColPoint, chCol.frontColPoint);

        gameObject.transform.position = newPos;

    }
	
	// Update is called once per frame
	void Update () {

        Move(Input.GetAxisRaw("horizontal"), Input.GetAxisRaw("vertical"), transform.position);
    }
}
