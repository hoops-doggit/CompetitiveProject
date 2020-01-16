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

    public void Shoot(GameObject bullet, Transform bulletSpawnPos, string owner, Transform ownerT)
    {
        #region commented out text is for targeting block transforms

        //int layerMask = 1 << 11;
        //layerMask = ~layerMask;
        //RaycastHit hit = new RaycastHit();
        //Vector3 target = new Vector3();
        //if (Physics.Raycast(bulletSpawnPos.position, bulletSpawnPos.forward, out hit, Mathf.Infinity, layerMask))
        //{
        //    target = hit.collider.gameObject.GetComponent<Collider>().bounds.center;
        //}
        #endregion

        GameObject b = GameObject.Instantiate(bullet);
        b.transform.position = bulletSpawnPos.position;
        b.transform.rotation = bulletSpawnPos.rotation;
        b.GetComponent<Gun_Bullet>().owner = owner;
        if (ownerT != null)
        {
            b.GetComponent<Gun_Bullet>().ownerT = ownerT;
        }
        ownerT.GetComponentInParent<CH_Movement2>().MoveYouJustShot(b.transform.forward);


        switch (owner)
        {
            case "p1":
                b.layer = LayerMask.NameToLayer("p1bulletHull");
                break;
            case "p2":
                b.layer = LayerMask.NameToLayer("p2bulletHull");
                break;
            case "p3":
                b.layer = LayerMask.NameToLayer("p3bulletHull");
                break;
            case "p4":
                b.layer = LayerMask.NameToLayer("p4bulletHull");
                break;
        }
        
        //b.transform.LookAt(target);
    }

    public virtual void Gupdate() {}

}
