﻿using System.Collections;
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
    private int selectedDeflectAngle;
    [SerializeField] private Material[] mats = new Material[3];
    
    public List<GameObject> objectsInSwingZone = new List<GameObject>();
    public List<GameObject> objectsThatHaveBeenHit = new List<GameObject>();
    public int swingCooldown;
    private int swingChargeMax = 8;
    private bool ignoreBall = false;
    [SerializeField] private string owner;
    [SerializeField] private Transform ownerT;
    public float extremeAngleCutoff, middleAngle, inputAngleDebug, headAngleDebug, lmid, lcutoff, rmid,  rcutoff;
    private CH_BallInteractions chb;
    private CH_Styling chs;
    private CH_Movement2 chm;
    private Rigidbody rb;
    private CH_Input chi;

    public GameObject lMid;
    public GameObject lCutoff;
    public GameObject rMid;
    public GameObject rCutoff;
    public GameObject facingDirection;
    public GameObject inputDirection;
    public GameObject[] guides;
    public bool debug;

    Vector3 savedVelocity;
    Vector3 savedAngularVelocity;

    private void Start()
    {
        if (!debug)
        {
            lMid.SetActive(false);
            lCutoff.SetActive(false);
            rMid.SetActive(false);
            rCutoff.SetActive(false);
            facingDirection.SetActive(false);
            inputDirection.SetActive(false);

            foreach(GameObject go in guides)
            {
                go.SetActive(false);
            }

        }
        rb = GetComponentInParent<Rigidbody>();
        chs = GetComponentInParent<CH_Styling>();
        chm = GetComponentInParent<CH_Movement2>();
        chi = GetComponentInParent<CH_Input>();

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

        TurnOffDeflectAngles();
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
            DotProductCalculateDeflectAngle(Input.GetAxisRaw(chi.xAxis), Input.GetAxisRaw(chi.yAxis),GetComponentInParent<Transform>().eulerAngles.y);
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
        //adds object 
        if (currentlySwinging)
        {
            CheckIfObjectIsInSwingZoneList(other);
        }        
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
                other.GetComponent<RigidbodyPause>().PauseRigidbody();
                //other.GetComponent<B_Behaviour>().PauseBallSwing();
            }

            else if (other.tag == "bullet")
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
            TurnOnDeflectAngles();
            currentlySwinging = true;
            chm.SetState(State.SwingAttack);
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
                        objectsInSwingZone[i].GetComponent<RigidbodyPause>().UnPauseRigidbody();
                        objectsInSwingZone[i].GetComponent<B_Behaviour>().HitBall(deflectAngle[selectedDeflectAngle].transform, hitBallStrength);
                        ignoreBall = true;
                    }
                }

                if (objectsInSwingZone[i].tag == "bullet" && !objectsThatHaveBeenHit.Contains(objectsInSwingZone[i]))
                {
                    if (objectsInSwingZone[i].GetComponent<Gun_Bullet>().owner != owner)
                    {
                        objectsInSwingZone[i].GetComponent<RigidbodyPause>().UnPauseRigidbody();
                        GameManager.inst.TimeFreeze();
                        objectsInSwingZone[i].GetComponent<Gun_Bullet>().HitByBat(ownerT, hitBallStrength, owner);
                    }
                    else
                    {
                        objectsInSwingZone[i].GetComponent<RigidbodyPause>().UnPauseRigidbody();
                        GameManager.inst.TimeFreeze();
                        objectsInSwingZone[i].GetComponent<Gun_Bullet>().GotHitByOwner(deflectAngle[selectedDeflectAngle].transform, hitBallStrength, owner);
                    }
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

    public void StopSwing()
    {
        currentlySwinging = false;
        swingCooldown = 0;
        MeshRenderer batMeshRend = bathitZone.GetComponent<MeshRenderer>();
        batMeshRend.material = mats[0];
        batMeshRend.enabled = false;
        objectsThatHaveBeenHit.Clear();
        ignoreBall = false;
        chm.SetState(chm.preState);
        TurnOffDeflectAngles();
        if(chm.preState == State.Dashing)
        {
            chm.SetState(State.Dashing);
        }
        else
        {
            chm.SetState(State.Normal);
        }
        
    }

    private void TurnOffDeflectAngles()
    {
        for (int i = 0; i < deflectAngle.Length; i++)
        {
            deflectAngle[i].SetActive(false);
        }
    }

    private void TurnOnDeflectAngles()
    {
        for (int i = 0; i < deflectAngle.Length; i++)
        {
            deflectAngle[i].SetActive(true);
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
        selectedDeflectAngle = i;
    }

    private void CalculateDeflectAngle1(float xAxis, float yAxis, float lookAngle)
    {
        float inputAngle = CalculateInputAngle(xAxis, yAxis);
        inputAngleDebug = inputAngle;
        headAngleDebug = lookAngle;

        float lMidAngle = Clamp0360(lookAngle - middleAngle);
        float rMidAngle = Clamp0360(lookAngle + middleAngle);
        float lLocalCutoff = Clamp0360(lookAngle - extremeAngleCutoff);
        float rLocalCutoff = Clamp0360(lookAngle + extremeAngleCutoff);
        lmid = lMidAngle;
        lcutoff = lLocalCutoff;
        rmid = rMidAngle;

        rcutoff = rLocalCutoff;

        //Fire Left
        if (lLocalCutoff > (360 - extremeAngleCutoff)) //this line is correct
        {
            if (inputAngle < lMidAngle && inputAngle > 0)
            {
                HighlightSelectedDeflectAngle(0);
            }
            else if(inputAngle > lLocalCutoff && inputAngle < 360)
            {
                HighlightSelectedDeflectAngle(0);
            }
        }
        else if (inputAngle < lMidAngle && inputAngle > lLocalCutoff)
        {

            HighlightSelectedDeflectAngle(0);
        }

        //Fire Right
        else if (rLocalCutoff < extremeAngleCutoff )
        {
            if (inputAngle > rMidAngle && inputAngle < 360)
            {
                HighlightSelectedDeflectAngle(2);
            }
            else if (inputAngle > 0 && inputAngle < extremeAngleCutoff)
            {
                HighlightSelectedDeflectAngle(2);
            }
        }
        else if (inputAngle > rMidAngle && inputAngle < rLocalCutoff)
        {
            HighlightSelectedDeflectAngle(2);
        }
        
        //Fire Centre
        else
        {
            HighlightSelectedDeflectAngle(1);
        }  
        


    }

    private void CalculateDeflectAngle2(float xAxis, float yAxis, float lookAngle)
    {
        float inputAngle = CalculateInputAngle(xAxis, yAxis);
        inputAngleDebug = inputAngle;
        headAngleDebug = lookAngle;

        float lClose = Clamp0360(lookAngle - middleAngle);
        float rClose = Clamp0360(lookAngle + middleAngle);
        float lFar = Clamp0360(lookAngle - extremeAngleCutoff);
        float rFar = Clamp0360(lookAngle + extremeAngleCutoff);
        lmid = lClose;
        lcutoff = lFar;
        rmid = rClose;
        rcutoff = rFar;

        //Fire Left

        //if difference between lmidAngle and llocalCutoff is > (extremeAngleCutoff-middleAngle)

        if (DegreeDistance(lClose, lFar) > (extremeAngleCutoff - middleAngle))
        {
            if (lFar > (360 - extremeAngleCutoff) && inputAngle > lFar) //this line is correct
            {
                HighlightSelectedDeflectAngle(0);                
            }

            else if (inputAngle < lClose && inputAngle > 0)
            {
                HighlightSelectedDeflectAngle(0);
            }
        }
        else if (inputAngle < lClose && inputAngle > lFar)
        {
            HighlightSelectedDeflectAngle(0);
        }

        //Fire Right

        if(DegreeDistance(rClose, rFar) > (extremeAngleCutoff - middleAngle))
        {
            if (rFar < extremeAngleCutoff && inputAngle < extremeAngleCutoff)
            {
                HighlightSelectedDeflectAngle(2);
            }

            else if(inputAngle > rClose && inputAngle < 361)
            {
                HighlightSelectedDeflectAngle(2);
            }
        }
        else if (inputAngle > rClose && inputAngle < rFar)
        {
            HighlightSelectedDeflectAngle(2);
        }

        //Fire Centre
        else
        {
            HighlightSelectedDeflectAngle(1);
        }  
        


    }

    private void DotProductCalculateDeflectAngle(float xAxis, float yAxis, float lookAngle)
    {
        Vector2 lookDir = Quaternion.Euler(0, 0, lookAngle) * Vector2.up;
        Vector2 inputV2 = new Vector2(xAxis, yAxis);
        float inputAngle = CalculateInputAngle(xAxis, yAxis);
        float distance = Vector2.Angle(lookDir.normalized, inputV2.normalized);

        float lMidAngle = JamesesClamp0360(lookAngle - middleAngle);
        float rMidAngle = JamesesClamp0360(lookAngle + middleAngle);
        float lCutoff = JamesesClamp0360(lookAngle - extremeAngleCutoff);
        float rCutoff = JamesesClamp0360(lookAngle + extremeAngleCutoff);
        inputAngleDebug = inputAngle;
        headAngleDebug = lookAngle;
        lmid = lMidAngle;
        lcutoff = lCutoff;
        rmid = rMidAngle;
        rcutoff = rCutoff;

        #region debug
        rMid.transform.rotation = Quaternion.Euler(0, rMidAngle, 0);
        lMid.transform.rotation = Quaternion.Euler(0, lMidAngle, 0);
        this.lCutoff.transform.rotation = Quaternion.Euler(0, lCutoff, 0);
        this.rCutoff.transform.rotation = Quaternion.Euler(0, rCutoff, 0);
        inputDirection.transform.rotation = Quaternion.Euler(0, inputAngle, 0);
        facingDirection.transform.rotation = Quaternion.Euler(0, lookAngle, 0);
        #endregion

        //if ideal circumstances
        if(lMidAngle > lCutoff && rMidAngle < rCutoff)
        {
            //Debug.Log("ideal");
            if(inputAngle < lMidAngle && inputAngle > lCutoff)
            {
                HighlightSelectedDeflectAngle(0);
            }
            else if(inputAngle > rMidAngle && inputAngle < rCutoff)
            {
                HighlightSelectedDeflectAngle(2);
            }
            else
            {
                HighlightSelectedDeflectAngle(1);
            }
        }

        //if left quadrant is straddling 0/360
        else if (lMidAngle < lCutoff)
        {
            //Debug.Log("left");
            //if IA is between lmid angle and rmidAngle eg, in the centre
            if (inputAngle > lMidAngle && inputAngle < rMidAngle)
            {
                HighlightSelectedDeflectAngle(1);
            }
            //if IA is in the right quadrant
            else if(inputAngle > rMidAngle && inputAngle < rCutoff)
            {
                HighlightSelectedDeflectAngle(2);
            }
            //if IA is in the back quadrant
            else if(inputAngle > rCutoff && inputAngle < lCutoff)
            {
                HighlightSelectedDeflectAngle(1);
            }
            else
            {
                HighlightSelectedDeflectAngle(0);
            }
        }

        //if right quadrant is straddling 0/360
        else if (rMidAngle > rCutoff)
        {
            //Debug.Log("right");
            //if IA is in the front quadrant
            if (inputAngle > lMidAngle && inputAngle < rMidAngle)
            {
                //Debug.Log("propperFront");
                HighlightSelectedDeflectAngle(1);
            }
            //if IA is in the left quadrant
            else if (inputAngle < lMidAngle && inputAngle > lCutoff)
            {
                //Debug.Log("propperLeft");
                HighlightSelectedDeflectAngle(0);
            }
            //if IA is in the back quadrant
            else if (inputAngle < lCutoff && inputAngle > rCutoff)
            {
                //Debug.Log("back");
                HighlightSelectedDeflectAngle(1);
            }
            //Its in the right quadrant
            else
            {
                //Debug.Log("else right");
                HighlightSelectedDeflectAngle(2);
            }
        }        
    }

    float AngleDir2D(Vector2 A, Vector2 B)
    {
        //returns negative if b is left of a or postitve if b is right of a
        return -A.x * B.y + A.y * B.x;
    }

    float AngleDir3D(Vector3 fwd, Vector3 targetDir, Vector3 up)
    {
        Vector3 perp = Vector3.Cross(fwd, targetDir);
        float dir = Vector3.Dot(perp, up);

        if (dir > 0f)
        {
            return 1f;
        }
        else if (dir < 0f)
        {
            return -1f;
        }
        else
        {
            return 0f;
        }
    }

    private float CalculateInputAngle(float x, float y)
    {
        float angle = 0;

        if (x < 0)
        {
            return angle = 360 + Vector2.SignedAngle(new Vector2(x, y), Vector2.up);
        }
        else
        {
            return angle = Vector2.Angle(new Vector2(x, y), Vector2.up);
        }
    }

    private float DegreeDistance(float angleA, float angleB)
    {
        float phi = Mathf.Abs(angleB - angleA) % 360;
        float distance = phi > 180 ? 360 - phi : phi;
        return distance;
    }

    private float Clamp0360(float eulerAngles)
    {
        float result = eulerAngles - Mathf.CeilToInt(eulerAngles / 360f) * 360f;
        if (result < 0)
        {
            result += 360f;
        }
        return result;
    }
    private float JamesesClamp0360(float eulerAngles)
    {
        if (eulerAngles < 0.0f)
        {
            return eulerAngles + (Mathf.Ceil(Mathf.Abs(eulerAngles) / 360.0f)) * 360.0f;
        }
        else if (eulerAngles > 360.0f)
        {
            return eulerAngles - Mathf.Floor(eulerAngles / 360.0f) * 360.0f;
        }
        else
        {
            return eulerAngles;
        }
    }
}
