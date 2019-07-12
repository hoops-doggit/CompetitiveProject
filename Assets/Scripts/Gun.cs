using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Gun {

    public int timeHeld { get; private set; }
    
    public void TriggerPull()
    {
        if(timeHeld == 0)
        {
            TriggerStart();
        }
        else
        {
            TriggerHold();
        }
    }
    public virtual void TriggerStart()
    {
        timeHeld++;
    }
    public virtual void TriggerHold()
    {
        timeHeld++;
    }
    public virtual void TriggerRelease()
    {
        timeHeld = 0;
    }

    public void Shoot(GameObject bullet, Transform bulletSpawnPos)
    {
        GameObject b = GameObject.Instantiate(bullet);
        b.transform.position = bulletSpawnPos.position;
        b.transform.forward = bulletSpawnPos.forward;
    }

    public virtual void Gupdate() {}

}
