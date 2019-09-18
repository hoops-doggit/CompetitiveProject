using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CH_SwingAttack : MonoBehaviour
{
    public KeyCode swingKey;
    public float hitBallStrength, hitPause;
    [SerializeField] private float lengthOfSwingPersistance;
    public bool currentlySwinging;    
    
    [SerializeField] private GameObject bathitZone;
    [SerializeField] GameObject[] deflectAngle;
    [SerializeField] private Material[] mats = new Material[3];
    
    public List<GameObject> objectsInSwingZone = new List<GameObject>();
    public List<GameObject> objectsThatHaveBeenHit = new List<GameObject>();
    public int swingCooldown;
    private int swingChargeMax = 8;
    private bool ignoreBall = false;
    [SerializeField] private string owner;
    [SerializeField] private Transform ownerT;
    public float extremeLeftAngle, leftMiddleAngle, rightMiddleAngle;
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

        if (currentlySwinging)
        {
            CalculateDeflectAngle()
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (currentlySwinging)
        {
            CheckIfObjectIsInSwingZoneList(other);            
        }
        

    }

    private void OnTriggerStay(Collider other)
    {
        if (currentlySwinging)
        {
            CheckIfObjectIsInSwingZoneList(other);
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (objectsInSwingZone.Contains(other.gameObject) && !currentlySwinging)
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
                other.GetComponent<RigidbodyPause>().PauseRigidbody();
                //other.GetComponent<B_Behaviour>().PauseBallSwing();
            }

            else if (other.tag == "bullet" && other.GetComponent<Gun_Bullet>().owner != owner)
            {
                objectsInSwingZone.Add(other.gameObject);
                other.GetComponent<RigidbodyPause>().PauseRigidbody();
            }

            else if (other.tag == "player")
            {
                objectsInSwingZone.Add(other.gameObject);
            }
        }
    }

    void OnPauseGame()
    {
        savedVelocity = GetComponent<Rigidbody>().velocity;
        savedAngularVelocity = GetComponent<Rigidbody>().angularVelocity;
        GetComponent<Rigidbody>().isKinematic = true;
    }

    void OnResumeGame()
    {
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().AddForce(savedVelocity, ForceMode.VelocityChange);
        GetComponent<Rigidbody>().AddTorque(savedAngularVelocity, ForceMode.VelocityChange);
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
                        //objectsInSwingZone[i].GetComponent<B_Behaviour>().WakeBallSwing();
                        objectsInSwingZone[i].GetComponent<RigidbodyPause>().WakeRigidbody();
                        objectsInSwingZone[i].GetComponent<B_Behaviour>().HitBall(GetComponentInParent<Transform>(), hitBallStrength);                        
                        ignoreBall = true;
                    }
                }

                if (objectsInSwingZone[i].tag == "bullet" && !objectsThatHaveBeenHit.Contains(objectsInSwingZone[i]))
                {
                    objectsInSwingZone[i].GetComponent<RigidbodyPause>().WakeRigidbody();
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
            ToggleDeflectAngles();
            currentlySwinging = true;
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
        currentlySwinging = false;
        swingCooldown = 0;
        MeshRenderer batMeshRend = bathitZone.GetComponent<MeshRenderer>();
        batMeshRend.material = mats[0];
        batMeshRend.enabled = false;
        objectsThatHaveBeenHit.Clear();
        ignoreBall = false;
        chm.playerMovementDisabled = false;
        ToggleDeflectAngles();


    }   

    private void ToggleDeflectAngles()
    {
        for (int i = 0; i < deflectAngle.Length; i++)
        {
            if (deflectAngle[0].activeSelf)
            {
                deflectAngle[i].SetActive(false);
            }
            else
            {
                deflectAngle[i].SetActive(true);
            }
        }
    }

    private void DeflectAnglesOn()
    {
        for (int i = 0; i < deflectAngle.Length; i++)
        {
            deflectAngle[i].SetActive(true);
        }  
    }

    private void HighlightSelectedDeflectAngle(int i)
    {
        foreach(GameObject go in deflectAngle)
        {
            go.GetComponent<CH_SwingAngleIndicator>().AngleDeselected();
        }
        deflectAngle[i].GetComponent<CH_SwingAngleIndicator>().AngleSelected();
    }

    private void CalculateDeflectAngle(float xAxis, float yAxis, float lookAngle)
    {
        Vector2 temp = new Vector2(xAxis, yAxis);
        float inputAngle = Mathf.Atan(xAxis/yAxis);

        if(inputAngle < rightMiddleAngle && inputAngle > leftMiddleAngle)
        {
            //Do middleAttack
            HighlightSelectedDeflectAngle(1);
        }

        else if (inputAngle < leftMiddleAngle)
        {
            HighlightSelectedDeflectAngle(0);
        }

        else if(inputAngle > rightMiddleAngle)
        {
            HighlightSelectedDeflectAngle(2);
        }
    }
}
