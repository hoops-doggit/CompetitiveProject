using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CH_SwingAttack : MonoBehaviour
{   
    public float hitBallStrength, hitPause;
    public bool currentlySwinging;
    public KeyCode swingKey;
    [SerializeField] private GameObject bathitZone;
    [SerializeField] private Material[] mats = new Material[3];
    [SerializeField] private float lengthOfSwingPersistance;
    public List<GameObject> objectsInSwingZone = new List<GameObject>();
    public List<GameObject> objectsThatHaveBeenHit = new List<GameObject>();
    public int swingCooldown;
    private int swingChargeMax = 8;
    private bool ignoreBall = false;
    [SerializeField] private string owner;
    [SerializeField] private Transform ownerT;
    private CH_BallInteractions chb;
    private CH_Styling chs;
    private CH_Movement2 chm;
    private Rigidbody rb;

    Vector3 savedVelocity;
    Vector3 savedAngularVelocity;

    private void Start()
    {
        rb = GetComponentInParent<Rigidbody>();
        chs = GetComponentInParent<CH_Styling>();
        chm = GetComponentInParent<CH_Movement2>();

        if (GetComponentInParent<CH_Input>() != null)
        {
            CH_Input chi = GetComponentInParent<CH_Input>();
            swingKey = chi.swingKey;
            owner = chi.owner;
        }
        if (GetComponentInParent<GunControl>() != null)
        {
            ownerT = GetComponentInParent<GunControl>().ownerT;
        }

        if (GetComponentInParent<CH_BallInteractions>() != null)
        {
            chb = GetComponentInParent<CH_BallInteractions>();
        }

        StopSwing();
        bathitZone = gameObject;
    }

    private void FixedUpdate()
    {
        if (swingCooldown < swingChargeMax)
        {
            swingCooldown++;
        }

        if (Input.GetKeyDown(swingKey) && swingCooldown >= swingChargeMax)
        {
            Swing();
        }        
    }

    private void Update()
    {
        if (objectsInSwingZone.Count != 0)
        {
            for (int i = 0; i < objectsInSwingZone.Count; i++)
            {
                if (objectsInSwingZone[i] == null)
                {
                    objectsInSwingZone.RemoveAt(i);
                }
            }
        }

        if (objectsThatHaveBeenHit.Count != 0)
        {
            for (int i = 0; i < objectsThatHaveBeenHit.Count; i++)
            {
                if (objectsThatHaveBeenHit[i] == null)
                {
                    objectsThatHaveBeenHit.RemoveAt(i);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        CheckIfObjectIsInSwingZoneList(other);
    }

    private void OnTriggerStay(Collider other)
    {
        CheckIfObjectIsInSwingZoneList(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (objectsInSwingZone.Contains(other.gameObject))
        {
            objectsInSwingZone.Remove(other.gameObject);
        }
        if (objectsThatHaveBeenHit.Contains(other.gameObject))
        {
            objectsThatHaveBeenHit.Remove(other.gameObject);
        }
    }

    private void CheckIfObjectIsInSwingZoneList(Collider other)
    {
        if (!objectsInSwingZone.Contains(other.gameObject))
        {
            if (other.tag == "ball" && other.GetComponent<B_Behaviour>().free)
            {
                objectsInSwingZone.Add(other.gameObject);
            }

            if (other.tag == "bullet" && other.GetComponent<Gun_Bullet>().owner != owner)
            {
                objectsInSwingZone.Add(other.gameObject);
            }

            if (other.tag == "player")
            {
                objectsInSwingZone.Add(other.gameObject);
            }
        }
    }

    void OnPauseGame()
    {
        savedVelocity = rigidbody.velocity;
        savedAngularVelocity = rigidbody.angularVelocity;
        rigidbody.isKinematic = true;
    }

    void OnResumeGame()
    {
        rigidbody.isKinematic = false;
        rigidbody.AddForce(savedVelocity, ForceMode.VelocityChange);
        rigidbody.AddTorque(savedAngularVelocity, ForceMode.VelocityChange);
    }

    public IEnumerator SwingAttack()
    {
        //stop player from moving

        yield return new WaitForSeconds(hitPause);
        //wait till swing has chaged (hit pause)
        //Do things

        #region Do things
        if (chb.holdingBall)
        {
            chb.ThrowBall();
        }

        if (objectsInSwingZone.Count > 0)
        {
            for (int i = 0; i < objectsInSwingZone.Count; i++)
            {
                if (!ignoreBall)
                {
                    if (objectsInSwingZone[i].tag == "ball" && !objectsThatHaveBeenHit.Contains(objectsInSwingZone[i]))
                    {
                        GameManager.inst.TimeFreeze();
                        objectsInSwingZone[i].GetComponent<B_Behaviour>().HitBall(GetComponentInParent<Transform>(), hitBallStrength);
                        ignoreBall = true;
                    }
                }

                if (objectsInSwingZone[i].tag == "bullet" && !objectsThatHaveBeenHit.Contains(objectsInSwingZone[i]))
                {
                    GameManager.inst.TimeFreeze();
                    objectsInSwingZone[i].GetComponent<Gun_Bullet>().HitByBat(ownerT, hitBallStrength, owner);
                }

                if (objectsInSwingZone[i].tag == "player" && !objectsThatHaveBeenHit.Contains(objectsInSwingZone[i]))
                {
                    GameManager.inst.TimeFreeze();
                    objectsInSwingZone[i].transform.parent.GetComponent<CH_Movement2>().MoveYouGotWhackedByABat(transform.position);
                    objectsInSwingZone[i].transform.parent.GetComponent<CH_Styling>().BodyHitFlash();

                    if (objectsInSwingZone[i].GetComponentInParent<CH_BallInteractions>().holdingBall)
                    {
                        ignoreBall = true;
                    }
                }
            }
        }
        objectsInSwingZone.Clear();
        objectsThatHaveBeenHit.Clear();
        Debug.Log("StoppingSwing");
        StopSwing();
        yield break;
        #endregion        
    }  

    private void PrepareSwing()
    {
        swingCooldown++;
        MeshRenderer batMeshRend = bathitZone.GetComponent<MeshRenderer>();
        batMeshRend.enabled = true;
        if (swingCooldown < swingChargeMax)
        {
            batMeshRend.material = mats[0];
        }
        else
        {
            batMeshRend.material = mats[1];
        }
    }

    private void Swing()
    {
        if (!chb.holdingBall)
        {
            chm.playerMovementDisabled = true;
            MeshRenderer batMeshRend = bathitZone.GetComponent<MeshRenderer>();
            batMeshRend.enabled = true;
            batMeshRend.material = mats[2];
            StartCoroutine(SwingAttack());
        }
        else
        {
            chb.ThrowBall();
        }
    }

    public void StopSwing()
    {
        swingCooldown = 0;
        MeshRenderer batMeshRend = bathitZone.GetComponent<MeshRenderer>();
        batMeshRend.material = mats[0];
        batMeshRend.enabled = false;
        objectsThatHaveBeenHit.Clear();
        Debug.Log("stopping swing");
        ignoreBall = false;
        chm.playerMovementDisabled = false;
    }   
}
