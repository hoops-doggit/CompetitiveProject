using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GunType_ChargedShotBehaviour : Gun {

    private GameObject bullet;
    private Transform bulletSpawnPos;
    private float timeTillFire = 20;
    //public float timeButtonHeld;

    public GunType_ChargedShotBehaviour(GameObject bullet, Transform bulletSpawnPos, float timeTillFire)
    {
        this.bullet = bullet;
        this.bulletSpawnPos = bulletSpawnPos;
        this.timeTillFire = timeTillFire;
    }

    public override void TriggerRelease()
    {
        if(timeHeld > timeTillFire)
        {
            Shoot(bullet, bulletSpawnPos);
        }
        base.TriggerRelease();
    }
}
