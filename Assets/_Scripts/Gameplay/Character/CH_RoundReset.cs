using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CH_RoundReset : MonoBehaviour
{
    private Vector3 startPos;
    private CH_BallInteractions chb;

    // Start is called before the first frame update
    void Start()
    {
        chb = GetComponent<CH_BallInteractions>();
        startPos = transform.position;
    }

    public void ResetPlayer()
    {
        transform.position = startPos;
        chb.ResetRelationshipToBall();
    }
}
