using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CH_CollisionListener : MonoBehaviour
{
    private CH_PlayerInteractions playerInteractions;

    private void Start()
    {
        playerInteractions = GetComponentInParent<CH_PlayerInteractions>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "bullet")
        {
            playerInteractions.GotShot();
        }
    }
}
