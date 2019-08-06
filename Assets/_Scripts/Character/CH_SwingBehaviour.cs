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
    [SerializeField] private Material[] mats = new Material[2];
    [SerializeField] private float lengthOfSwingPersistance;
    public List<GameObject> objectsInSwingZone = new List<GameObject>();

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
        if (Input.GetButtonUp(swingButton))
        {
            StartCoroutine("SwingEnumerator");
        }

        if (Input.GetButton(swingButton))
        {
            PrepareSwing();
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
        objectsInSwingZone.Add(other.gameObject);        
    }

    private void OnTriggerExit(Collider other)
    {
        objectsInSwingZone.Remove(other.gameObject);
    }

    private void PrepareSwing()
    {
        MeshRenderer batMeshRend = bathitZone.GetComponent<MeshRenderer>();
        batMeshRend.enabled = true;
        batMeshRend.material = mats[0];
    }

    private void Swing()
    {
        currentlySwinging = true;
        MeshRenderer batMeshRend = bathitZone.GetComponent<MeshRenderer>();
        batMeshRend.material = mats[1];
    }

    public void StopSwing()
    {
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
