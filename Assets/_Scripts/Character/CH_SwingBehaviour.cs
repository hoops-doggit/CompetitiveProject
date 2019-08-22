﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CH_SwingBehaviour : MonoBehaviour
{
    public float hitBallStrength;

    public bool buttonHeld;
    public bool currentlySwinging;
    public KeyCode swingKey;
    [SerializeField] private GameObject bathitZone;
    [SerializeField] private Material[] mats = new Material[3];
    [SerializeField] private float lengthOfSwingPersistance;
    public List<GameObject> objectsInSwingZone = new List<GameObject>();
    public List<GameObject> objectsThatHaveBeenHit = new List<GameObject>();
    public int swingChargeTimer;
    private int swingChargeMax = 7;
    private bool ignoreBall = false;

    private void Start()
    {
        CH_Input chi = GetComponentInParent<CH_Input>();
        swingKey = chi.swingKey;
        StopSwing();
        bathitZone = gameObject;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (swingChargeTimer <= swingChargeMax)
        {
            swingChargeTimer++;
        }

        if (Input.GetKeyDown(swingKey) && swingChargeTimer >= swingChargeMax)
        {
            Swing();
            StartCoroutine("SwingEnumerator");
        }

        if (currentlySwinging)
        {
            if (objectsInSwingZone.Count > 0)
            {
                for (int i = 0; i < objectsInSwingZone.Count; i++)
                {
                    if (!ignoreBall)
                    {
                        if (objectsInSwingZone[i].tag == "ball" && !objectsThatHaveBeenHit.Contains(objectsInSwingZone[i]))
                        {
                            objectsInSwingZone[i].GetComponent<B_Behaviour>().HitBall(GetComponentInParent<Transform>(), hitBallStrength);
                            objectsThatHaveBeenHit.Add(objectsInSwingZone[i]);
                            ignoreBall = true;
                        }
                    }

                    if (objectsInSwingZone[i].tag == "bullet")
                    {
                        objectsInSwingZone[i].GetComponent<Gun_Bullet>().HitByBat(GetComponentInParent<Transform>(), hitBallStrength);
                        objectsThatHaveBeenHit.Add(objectsInSwingZone[i]);
                    }

                    if (objectsInSwingZone[i].tag == "player" && !objectsThatHaveBeenHit.Contains(objectsInSwingZone[i]))
                    {
                        objectsInSwingZone[i].transform.parent.GetComponent<CH_Movement2>().MoveYouGotWhackedByABat(transform.position);
                        if (objectsInSwingZone[i].GetComponentInParent<CH_BallInteractions>().holdingBall)
                        {
                            ignoreBall = true;
                        }
                        objectsThatHaveBeenHit.Add(objectsInSwingZone[i]);

                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ball" && other.GetComponent<B_Behaviour>().free)
        {
            if (!objectsInSwingZone.Contains(other.gameObject))
            {
                objectsInSwingZone.Add(other.gameObject);
            }
        }

        if(other.tag == "bullet")
        {
            if (!objectsInSwingZone.Contains(other.gameObject))
            {
                objectsInSwingZone.Add(other.gameObject);
            }
        }

        if (other.tag == "player")
        {
            if (!objectsInSwingZone.Contains(other.gameObject))
            {
                objectsInSwingZone.Add(other.gameObject);
            }
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

    private void PrepareSwing()
    {
        swingChargeTimer++;
        MeshRenderer batMeshRend = bathitZone.GetComponent<MeshRenderer>();
        batMeshRend.enabled = true;
        if (swingChargeTimer < swingChargeMax)
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

        currentlySwinging = true;
        MeshRenderer batMeshRend = bathitZone.GetComponent<MeshRenderer>();
        batMeshRend.enabled = true;
        batMeshRend.material = mats[2];
    }

    public void StopSwing()
    {
        swingChargeTimer = 0;
        currentlySwinging = false;
        MeshRenderer batMeshRend = bathitZone.GetComponent<MeshRenderer>();
        batMeshRend.material = mats[0];        
        batMeshRend.enabled = false;
        objectsThatHaveBeenHit.Clear();
        ignoreBall = false;
    }

    private IEnumerator SwingEnumerator()
    {
        Debug.Log("started swing");        
        yield return new WaitForSeconds(lengthOfSwingPersistance);
        StopSwing();
        Debug.Log("stopped swing");
        StopCoroutine("SwingEnumerator");
    }
}
