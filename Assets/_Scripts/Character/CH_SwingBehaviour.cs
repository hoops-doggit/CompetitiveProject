using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CH_SwingBehaviour : MonoBehaviour
{
    public float hitStrength;
    public bool buttonHeld;
    public bool currentlySwinging;
    public string swingButton;
    [SerializeField] private GameObject bathitZone;
    [SerializeField] private Material[] mats = new Material[3];
    [SerializeField] private float lengthOfSwingPersistance;
    public List<GameObject> objectsInSwingZone = new List<GameObject>();
    private int swingChargeTimer;
    private int swingChargeMax = 7;

    private void Start()
    {
        CH_Input chi = GetComponentInParent<CH_Input>();
        swingButton = chi.swingButton;
        StopSwing();
        bathitZone = gameObject;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (Input.GetButton(swingButton))
        {
            PrepareSwing();
        }
        else if (!Input.GetButton(swingButton) && swingChargeTimer >= swingChargeMax)
        {
            StartCoroutine("SwingEnumerator");
        }
        else
        {
            StopSwing();
        }

        if (currentlySwinging)
        {
            if (objectsInSwingZone.Count > 0)
            {
                for (int i = 0; i < objectsInSwingZone.Count; i++)
                {
                    if(objectsInSwingZone[i].tag == "ball")
                    {
                        objectsInSwingZone[i].GetComponent<B_Behaviour>().HitBall(GetComponentInParent<Transform>(), hitStrength);
                        objectsInSwingZone.Remove(objectsInSwingZone[i]);
                        //could error the for loop because it's altering the count as it's looping through
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
    }

    private void OnTriggerExit(Collider other)
    {
        objectsInSwingZone.Remove(other.gameObject);
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
        batMeshRend.material = mats[2];
    }

    public void StopSwing()
    {
        swingChargeTimer = 0;
        currentlySwinging = false;
        MeshRenderer batMeshRend = bathitZone.GetComponent<MeshRenderer>();
        batMeshRend.material = mats[0];        
        batMeshRend.enabled = false;        
    }

    private IEnumerator SwingEnumerator()
    {
        Debug.Log("started swing");
        Swing();
        yield return new WaitForSeconds(lengthOfSwingPersistance);
        StopSwing();
        Debug.Log("stopped swing");
        StopCoroutine("SwingEnumerator");
    }
}
