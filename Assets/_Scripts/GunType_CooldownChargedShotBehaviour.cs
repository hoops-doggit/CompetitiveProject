using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GunType_CooldownChargedShotBehaviour : Gun {

    private GameObject bullet;
    private GameObject chargeIndicatorBullet;
    private GameObject cloneBullet;
    private Transform bulletSpawnPos;
    private float cooldownTime;
    private float currentTime;
    private float scaleFactor = 0.001f;


    //Constructor
    public GunType_CooldownChargedShotBehaviour(GameObject bullet, GameObject chargeIndicatorBullet,  Transform bulletSpawnPos, int cooldownTime)
    {
        this.bullet = bullet;
        this.chargeIndicatorBullet = chargeIndicatorBullet;
        this.bulletSpawnPos = bulletSpawnPos;
        this.cooldownTime = cooldownTime;
    }


    public override void Gupdate()
    {
        if(currentTime > 0)
        {
            currentTime--;
        }
        

        if(currentTime > 0)
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
                cloneBullet.transform.localScale += bullet.transform.localScale / cooldownTime;
            }
        }
        base.Gupdate();
    }

    public override void TriggerStart()
    {
        base.TriggerStart();
        if(currentTime <= 0)
        {
            Shoot(bullet, bulletSpawnPos);
            GameObject.Destroy(cloneBullet);
            currentTime = cooldownTime;
        }
    }
}
