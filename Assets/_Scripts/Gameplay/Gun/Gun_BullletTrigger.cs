using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_BullletTrigger : MonoBehaviour
{
    [SerializeField] private Gun_Bullet gb;

    private void Start()
    {
        string owner = GetComponentInParent<Gun_Bullet>().owner;
        switch (owner)
        {
            case "p1":
                gameObject.layer = LayerMask.NameToLayer("p1bulletTrigger");
                break;
            case "p2":
                gameObject.layer = LayerMask.NameToLayer("p2bulletTrigger");
                break;
            case "p3":
                gameObject.layer = LayerMask.NameToLayer("p3bulletTrigger");
                break;
            case "p4":
                gameObject.layer = LayerMask.NameToLayer("p4bulletTrigger");
                break;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        gb.HitBall(other.gameObject);
    }

}
