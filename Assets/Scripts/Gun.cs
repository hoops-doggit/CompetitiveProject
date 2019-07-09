using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Gun : MonoBehaviour {

    public GunType gunType;
    [SerializeField]
    private string fireButtonName;
    public Transform chargeShotBulletSpawnPos;
    [SerializeField]
    private Transform bulletHolder;
    private CH_Input chi;

    [Header("Charge Shot Variables")]
    public float timeTillFire;
    public float timeButtonHeld;
    public GameObject chargeShotBullet;
    public MeshRenderer bulb01;
    public MeshRenderer bulb02;
    public MeshRenderer bulb03;
    public Material off;
    public Material on;

    [Header("Artillary Shot Variables")]
    public float timeTillFireArtillary;


    private void Awake()
    {
        chi = GetComponent<CH_Input>();
        
        bulletHolder = GameObject.Find("_BulletHolder").transform;
        bulb01.material = off;
        bulb02.material = off;
        bulb03.material = off;
    }

    private void Start()
    {
        fireButtonName = chi.shootButton;
    }

    void Update()
    {
        Shoot(gunType);
    }

    public void Shoot(GunType gt)
    {
        switch (gt)
        {
            case GunType.chargeshot:
                ChargeShot();
                break;
            case GunType.artillary:
                break;
        }
    }

    private void ChargeShot()
    {
        if (Input.GetButton(fireButtonName))
        {
            timeButtonHeld++;
        }
        if(timeButtonHeld > timeTillFire / 10)
        {
            bulb01.material = on;
        }
        if(timeButtonHeld > timeTillFire / 2)
        {
            bulb02.material = on;
        }
        if(timeButtonHeld >= timeTillFire)
        {

            bulb03.material = on;
        }
        if(Input.GetButtonUp(fireButtonName) && timeButtonHeld >= timeTillFire)
        {
            GameObject firedChargeShot = Instantiate(chargeShotBullet, chargeShotBulletSpawnPos.position, chargeShotBulletSpawnPos.rotation, bulletHolder);
            timeButtonHeld = 0;
            bulb01.material = off;
            bulb02.material = off;
            bulb03.material = off;
            firedChargeShot.transform.parent = bulletHolder;
        }
        else if(!Input.GetButton(fireButtonName) && timeButtonHeld < timeTillFire)
        {
            //this will likely have a cooldown effect in the future
            timeButtonHeld = 0;
            bulb01.material = off;
            bulb02.material = off;
            bulb03.material = off;
        }
    }
}
