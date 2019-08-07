using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunType_MachineGun : Gun {

    public GameObject bullet;
    public Transform t;
    private int bulletInterval;
    private int intervalTime;

    public GunType_MachineGun(GameObject bullet, Transform t, int bulletInterval)
    {
        this.bullet = bullet;
        this.t = t;
        this.bulletInterval = bulletInterval;
    }    

    public override void TriggerHold()
    {
        intervalTime++;
        if (intervalTime >= bulletInterval)
        {
            Shoot(bullet, t);
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
