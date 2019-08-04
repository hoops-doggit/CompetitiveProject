using UnityEngine;

public class GunControl : MonoBehaviour {
    private Gun gun;
    [Header("Charged Shot Parameters")]
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private GameObject chargeIndicator;
    [SerializeField]
    private Transform bulletSpawnPos;
    [SerializeField]
    private float timeTillFire;

    [Header("Artillary Shot Parameters")]
    public GameObject aBullet;
    public Transform aBulletSpawnPos;
    public Transform armPos;
    public float angle;
    public int coolDownLength;

    [Header("Machine Gun Parameters")]
    public GameObject machineGunBullet;
    public Transform machineGunSpawnPos;
    public int intervalTime;


    // Use this for initialization
    void Start () {
        ChangeToChargedShot();
	}

    public void ChangeToChargedShot()
    {
        gun = new GunType_ChargedShotBehaviour(bullet, chargeIndicator, bulletSpawnPos, timeTillFire);
    }

    public void ChangeToArtillary()
    {
        gun = new GunType_ArtillaryBehaviour(aBullet, aBulletSpawnPos, armPos, angle, coolDownLength);
;   }

    public void ChangeToMachineGun()
    {
        gun = new GunType_MachineGun(machineGunBullet, machineGunSpawnPos, intervalTime);
    }


    // Update is called once per frame
    void FixedUpdate () {

        gun.Gupdate();

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeToChargedShot();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeToArtillary();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ChangeToMachineGun();
        }


		if (Input.GetButton("fire1"))
        {
            gun.TriggerPull();
        }
        else
        {
            gun.TriggerRelease();
        }
	}
}
