using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunType_MachineGun : Gun {

    public GameObject bullet;
    public Transform t;
    private int bulletInterval;
    private int intervalTime;
    private string owner;
    private Transform ownerT;

    public GunType_MachineGun(GameObject bullet, Transform t, int bulletInterval, string owner, Transform ownerT)
    {
        this.bullet = bullet;
        this.t = t;
        this.bulletInterval = bulletInterval;
        this.owner = owner;
        this.ownerT = ownerT;
    }    

    public override void TriggerHold()
    {
        intervalTime++;
        if (intervalTime >= bulletInterval)
        {
            Shoot(bullet, t, owner, ownerT);
            intervalTime = 0;
        }
        
        base.TriggerHold();
    }

    public override void TriggerRelease()
    {
        intervalTime = 0;
        base.TriggerRelease();
    }
}
