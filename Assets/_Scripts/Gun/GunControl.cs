using UnityEngine;

public class GunControl : MonoBehaviour {
    public bool firegun;
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
    public string owner;
    public Transform ownerT;
    private CH_BallInteractions chb;



    // Use this for initialization
    void Start () {

        CH_Input chi = GetComponent<CH_Input>();
        chb = GetComponentInParent<CH_BallInteractions>();

        shootButton = chi.shootKey;
        owner = chi.owner;

        ChangeToCooldownChargedShot();
    }

    public void ChangeToChargedShot()
    {
        gun = new GunType_ChargedShotBehaviour(bullet, chargeIndicator, bulletSpawnPos, timeTillFire, owner, ownerT);
    }

    public void ChangeToArtillary()
    {
        gun = new GunType_ArtillaryBehaviour(aBullet, aBulletSpawnPos, armPos, angle, coolDownLength, owner, ownerT);
;   }

    public void ChangeToMachineGun()
    {
        gun = new GunType_MachineGun(machineGunBullet, machineGunSpawnPos, intervalTime, owner, ownerT);
    }

    public void ChangeToCooldownChargedShot()
    {
        gun = new GunType_CooldownChargedShotBehaviour(bullet, cooldownChargedIndicator,  bulletSpawnPos, timeTillCharge, owner, ownerT);
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


        if (firegun)
        {
            gun.TriggerPull();
        }

        if (Input.GetKey(shootButton))
        {
            if (chb.holdingBall)
            {
                chb.ThrowBall();
            }

            gun.TriggerPull();
        }
        else
        {
            gun.TriggerRelease();
        }
	}
}
