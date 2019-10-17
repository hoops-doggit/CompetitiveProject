using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CH_PlayerInteractions : MonoBehaviour
{
    private CH_BallInteractions ballInteractions;
    private CH_Movement2 movement;
    public Transform player2;


    private void Start()
    {
        ballInteractions = GetComponent<CH_BallInteractions>();
        movement = GetComponent<CH_Movement2>();
    }

    public void GotShot(Vector3 bulletDirection)
    {
        ballInteractions.DropBall(bulletDirection, "bullet");
    }

    //private void GotStunned(float x, float y)
    //{
    //    //takes source of stun and moves character away in that direction
    //    Vector2 direction = new Vector2(player2.position.x, player2.position.z).normalized;
    //    movement.Move2(direction.x, direction.y, State.Stunned);
    //}
}
