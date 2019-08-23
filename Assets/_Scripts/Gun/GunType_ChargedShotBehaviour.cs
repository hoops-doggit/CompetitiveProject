using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GunType_ChargedShotBehaviour : Gun {

    private GameObject bullet;
    private GameObject chargeIndicatorBullet;
    private GameObject cloneBullet;
    private Transform bulletSpawnPos;
    private float timeTillFire;
    private float scaleFactor = 0.001f;
    private string owner;


    //Constructor
    public GunType_ChargedShotBehaviour(GameObject bullet, GameObject chargeIndicatorBullet,  Transform bulletSpawnPos, float timeTillFire, string owner)
    {
        this.bullet = bullet;
        this.chargeIndicatorBullet = chargeIndicatorBullet;
        this.bulletSpawnPos = bulletSpawnPos;
        this.timeTillFire = timeTillFire;
        this.owner = owner;
    }


    public override void Gupdate()
    {
        if(timeHeld < timeTillFire)
        {
            if (cloneBullet == null)
            {
                cloneBullet = GameObject.Instantiate(chargeIndicatorBullet);
                cloneBullet.transform.position = bulletSpawnPos.position;
                cloneBullet.transform.SetParent(bulletSpawnPos);
                cloneBullet.transform.localScale = Vector3.zero;
            }
            else if (cloneBullet != null && cloneBullet.transform.localScale.x < bullet.transform.localScale.x)
            {
                cloneBullet.transform.localScale += bullet.transform.localScale / timeTillFire;
            }
        }
        base.Gupdate();
    }
    

    public override void TriggerRelease()
    {       

        if (timeHeld > timeTillFire)
        {
            Shoot(bullet, bulletSpawnPos, owner);
        }
        GameObject.Destroy(cloneBullet);

        base.TriggerRelease();
    }
}
