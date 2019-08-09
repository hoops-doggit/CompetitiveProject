using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CH_PlayerInteractions : MonoBehaviour
{
    private CH_BallInteractions ballints;
    private CH_Movement2 movement;
    private bool evaluatingStunned;
    public Transform player2;


    private void Start()
    {
        ballints = GetComponent<CH_BallInteractions>();
        movement = GetComponent<CH_Movement2>();
    }

    private void FixedUpdate()
    {
        //for debug only
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GotShot();
        }

        if (evaluatingStunned)
        {

        }

    }

    public void GotShot()
    {
        evaluatingStunned = true;
        ballints.DropBall();
    }

    private void GotStunned(float x, float y)
    {
        //takes source of stun and moves character away in that direction
        Vector2 direction = new Vector2(player2.position.x, player2.position.z).normalized;
        movement.Move(direction.x, direction.y, true);
    }
}
