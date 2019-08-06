using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CH_Movement : MonoBehaviour {

    private CH_Collisions col;
    public float speed;
    private Vector3 move;
    private Vector2 m;
    private Vector3 moveClamped;


	// Use this for initialization
	void Start () {
        col = GetComponent<CH_Collisions>();
	}


    public void Movement(float x, float y)
    {
        //Debug.Log("x= " + x + "  y= "+ y);
        if(col.front || col.front02)
        {
            if (y > 0)
            {
                y = 0;
            }
        }
        if(col.back01 || col.back02)
        {
            if(y < 0)
            {
                y = 0;
            }
            
        }
        if(col.left01 || col.left02)
        {
            if (x < 0)
            {
                x = 0;
            }
            
        }
        if(col.right01 || col.right02)
        {
            if (x > 0)
            {
                x = 0;
            }
        }

        m.x = x;
        m.y = y;

        float mLength = m.magnitude;
        if (mLength > 1)
        {
            move.x = m.x / mLength;
            move.z = m.y / mLength;
            ExecuteMovement(move);
        }
        else if (mLength < -1)
        {
            move.x = m.x / mLength;
            move.z = m.y / mLength;
            ExecuteMovement(move);
        }
        else
        {
            move.x = m.x;
            move.z = m.y;
            ExecuteMovement(move);
        }
    }

    private void ExecuteMovement(Vector3 move)
    {
        gameObject.transform.position += (move*speed*Time.deltaTime);
        //gameObject.transform.position = ClampMovement(gameObject.transform.position);
    }

    private Vector3 ClampMovement(Vector3 moveVal)
    {
        Mathf.Clamp(moveVal.y, col.backColPoint, col.frontColPoint);
        Mathf.Clamp(moveVal.x, col.leftColPoint, col.rightColPoint);
        return moveVal;
    }

    private void ResetMoveVector3()
    {
        move = Vector3.zero;
    }
	

	void Update () {
        ResetMoveVector3();
        Movement(Input.GetAxis("horizontal"), Input.GetAxis("vertical"));
    }
}
