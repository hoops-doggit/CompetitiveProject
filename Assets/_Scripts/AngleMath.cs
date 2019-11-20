using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AngleMath : MonoBehaviour
{
    public float AngleBetweenTwoVectors(Vector2 vector1, Vector2 vector2)
    {
        float angle = Mathf.Atan2(vector2.y, vector2.x) - Mathf.Atan2(vector1.y, vector1.x);
        return angle;
    }
    
    public Vector2 Vector2BetweenTwoVectors(Vector2 vector1, Vector2 vector2)
    {
        Vector2 result = new Vector2(vector1.x + vector2.x, vector1.y + vector2.y);
        return result;
    }
}
