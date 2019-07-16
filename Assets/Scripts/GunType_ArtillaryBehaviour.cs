using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunType_ArtillaryBehaviour : Gun {

    private GameObject bullet;
    private Transform bulletSpawnPos;
    private Vector3 target;
    private Transform armPos;
    private float angle;
    private int coolDownTimer;
    private int coolDownLength;

    public GunType_ArtillaryBehaviour(GameObject bullet, Transform bulletSpawnPos, Transform armPos, float angle, int coolDownLength)
    {
        this.bullet = bullet;
        this.bulletSpawnPos = bulletSpawnPos;
        this.angle = angle;
        this.armPos = armPos;
        this.coolDownLength = coolDownLength;
    }

    public override void TriggerStart()
    {
        if (coolDownTimer == 0)
        {
            armPos.localEulerAngles = new Vector3(angle, 0, 0);
            Shoot(bullet, bulletSpawnPos);
            coolDownTimer = coolDownLength;
        }        
        base.TriggerStart();
    }

    public override void Gupdate()
    {
        if(coolDownTimer > 0)
        {
            coolDownTimer--;
        }
        base.Gupdate();
    }
}
