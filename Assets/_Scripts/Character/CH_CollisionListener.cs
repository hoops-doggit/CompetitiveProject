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
        gameObject.layer = LayerMask.NameToLayer(GetComponentInParent<CH_Input>().owner);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "bullet")
        {
            GameManager.inst.TimeFreeze();
            playerInteractions.GotShot(collision.gameObject.GetComponent<Gun_Bullet>().direction);
            Debug.Log("moveyougot stuneed");
            playerMovement.MoveYouGotStunned(collision.gameObject.GetComponent<Gun_Bullet>().direction);
        }

        if(collision.gameObject.tag == "swingZone")
        {
            GameManager.inst.TimeFreeze();
        }
    }
}
