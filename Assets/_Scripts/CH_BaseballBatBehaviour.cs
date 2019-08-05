﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CH_BaseballBatBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject bathitZone;
    [SerializeField] private Material[] mats = new Material[2];
    [SerializeField] private float lengthOfWindUp;
    public float hitStrength;
    public bool buttonHeld;

    public string swingButton;


    private void Start()
    {
        CH_Input chi = GetComponent<CH_Input>();
        swingButton = chi.swingButton;
        StopSwing();

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (Input.GetButtonUp(swingButton))
        {
            Swing();
            buttonHeld = true;
        }
        else
        {
            buttonHeld = false;
        }

        if (Input.GetButton(swingButton))
        {
            PrepareSwing();
        }        
    }

    private void PrepareSwing()
    {
        MeshRenderer batMeshRend = bathitZone.GetComponent<MeshRenderer>();
        batMeshRend.enabled = true;
        batMeshRend.material = mats[0];
        bathitZone.GetComponent<Collider>().enabled = false;
    }

    private void Swing()
    {
        bathitZone.GetComponent<Collider>().enabled = true;
        MeshRenderer batMeshRend = bathitZone.GetComponent<MeshRenderer>();
        batMeshRend.material = mats[1];
        StopSwing();
    }

    public void StopSwing()
    {
        bathitZone.GetComponent<Collider>().enabled = false;
        MeshRenderer batMeshRend = bathitZone.GetComponent<MeshRenderer>();
        batMeshRend.material = mats[0];        
        batMeshRend.enabled = false;        
    }

    private IEnumerator SwingEnumerator()
    {
        Swing();
        yield return new WaitForSeconds(lengthOfWindUp);
        StopSwing();
        StopCoroutine("SwingEnumerator");
    }
}
