using UnityEngine;

public class GunControl : MonoBehaviour {
    public bool firegun;
    private Gun gun;
    public bool gamePaused;
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
    public Transform cooldownSpawnPos, cooldownChargePos;
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
        timeTillCharge = 0;
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
        gun = new GunType_CooldownChargedShotBehaviour(bullet, cooldownChargedIndicator, cooldownChargePos, bulletSpawnPos, timeTillCharge, owner, ownerT);
    }


    // Update is called once per frame
    void FixedUpdate ()
    {
        if (!gamePaused)
        {
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

            if (Input.GetKeyDown(shootButton))
            {
                if (chb.holdingBall)
                {
                    chb.ThrowBall();
                }

                else if (!chb.holdingBall)
                {
                    gun.TriggerPull();
                }
            }
            else
            {
                gun.TriggerRelease();
            }

            //This is used for debugging etc
            if (firegun)
            {
                gun.TriggerPull();
            }
        }
    }

    public void ToggleInputPause()
    {
        if (gamePaused) { gamePaused = false; }
        else { gamePaused = true; }
    }
}
