using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankEnemy : Enemy
{
    public Transform target;

    public override void GotHit()
    {
        Dance();
        base.GotHit();
    }

    public override void ImDead()
    {
        Debug.Log("I'm a tank and I guess I'm dead now");
        base.ImDead();
    }
}
