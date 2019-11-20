using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_BullletTrigger : MonoBehaviour
{
    [SerializeField] private Gun_Bullet gb;


    private void OnTriggerEnter(Collider other)
    {
        GameObject go = other.gameObject;

        gb.HitBall(go);
    }

}
