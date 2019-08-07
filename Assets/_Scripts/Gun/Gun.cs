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
        int layerMask = 1 << 11;
        layerMask = ~layerMask;
        RaycastHit hit = new RaycastHit();
        Vector3 target = new Vector3();
        if (Physics.Raycast(bulletSpawnPos.position, bulletSpawnPos.forward, out hit, Mathf.Infinity, layerMask))
        {
            target = hit.collider.gameObject.GetComponent<Collider>().bounds.center;
        }
        GameObject b = GameObject.Instantiate(bullet);
        b.transform.position = bulletSpawnPos.position;
        b.transform.LookAt(target);
    }

    public virtual void Gupdate() {}

}
