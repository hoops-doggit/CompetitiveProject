using UnityEngine;

public class GunControl : MonoBehaviour {
    private Gun gun;
    [Header("Charged Shot Parameters")]
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject chargeIndicator;
    [SerializeField] private Transform bulletSpawnPos;
    [SerializeField] private float timeTillFire;

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

    [Header("Cooldown Gun Parameters")]
    public GameObject cooldownChargedBullet;
    public GameObject cooldownChargedIndicator;
    public Transform cooldownSpawnPos;
    public int timeTillCharge;


    public KeyCode shootButton;



    // Use this for initialization
    void Start () {
        ChangeToCooldownChargedShot();
        CH_Input chi = GetComponent<CH_Input>();
        shootButton = chi.shootKey;
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

    public void ChangeToCooldownChargedShot()
    {
        gun = new GunType_CooldownChargedShotBehaviour(bullet, cooldownChargedIndicator,  bulletSpawnPos, timeTillCharge);
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
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ChangeToCooldownChargedShot();
        }


        if (Input.GetKey(shootButton))
        {
            gun.TriggerPull();
        }
        else
        {
            gun.TriggerRelease();
        }
	}
}
