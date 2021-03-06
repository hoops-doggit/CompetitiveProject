﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GunType_CooldownChargedShotBehaviour : Gun {

    private GameObject bullet;
    private GameObject chargeIndicatorBullet;
    private GameObject cloneBullet;
    private Transform bulletSpawnPos;
    private Transform chargeIndicatorPos;
    private float cooldownTime;
    private float currentTime;
    private float scaleFactor = 0.001f;
    private string owner;
    private Transform ownerT;


    //Constructor
    public GunType_CooldownChargedShotBehaviour(GameObject bullet, GameObject chargeIndicatorBullet,  Transform chargeIndicatorPos,Transform bulletSpawnPos, int cooldownTime, string owner, Transform ownerT)
    {
        this.bullet = bullet;
        this.chargeIndicatorBullet = chargeIndicatorBullet;
        this.bulletSpawnPos = bulletSpawnPos;
        this.cooldownTime = cooldownTime;
        this.owner = owner;
        this.ownerT = ownerT;
        this.chargeIndicatorPos = chargeIndicatorPos;
        currentTime = 1;
    }

    public override void Gupdate()
    {

        if(currentTime > 0)
        {
            currentTime--;

            if (cloneBullet == null)
            {
                cloneBullet = GameObject.Instantiate(chargeIndicatorBullet, bulletSpawnPos, true);
                cloneBullet.GetComponent<Gun_ChargingBullet>().pn = owner;
                cloneBullet.transform.position = chargeIndicatorPos.position;
                cloneBullet.transform.localScale = Vector3.zero;
                cloneBullet.transform.SetParent(bulletSpawnPos);
                
            }
            else if (cloneBullet != null && cloneBullet.transform.localScale.x < bullet.transform.localScale.x)
            {
                cloneBullet.transform.localScale += bullet.transform.localScale / cooldownTime;
            }
        }
        else
        {
            int i = 0;
            if (owner == "p1" || owner == "p3")
            {
                i= 0;
            }
            else if( owner == "p2" || owner == "p4")
            {
                i= 1;
            }
            cloneBullet.GetComponent<Gun_ChargingBullet>().SetChargedMat(i);
        }
        base.Gupdate();
    }

    public override void TriggerStart()
    {
        base.TriggerStart();
        if(currentTime <= 0)
        {
            Shoot(bullet, bulletSpawnPos, owner, ownerT);
            GameObject.Destroy(cloneBullet);
            cloneBullet = null;
            currentTime = cooldownTime;
        }
    }
}
