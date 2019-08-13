using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CH_CollisionListener : MonoBehaviour
{
    private CH_PlayerInteractions playerInteractions;
    private CH_Movement2 playerMovement;

    private void Start()
    {
        playerInteractions = GetComponentInParent<CH_PlayerInteractions>();
        playerMovement = GetComponentInParent<CH_Movement2>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "bullet")
        {
            playerInteractions.GotShot();
            Debug.Log("moveyougot stuneed");
            playerMovement.MoveYouGotStunned(collision.rigidbody.velocity);
        }
    }
}
