using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShipFollowBehaviour : MonoBehaviour
{
    public GameObject leadGameObject;
    public List<GameObject> ships = new List<GameObject>();


 

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(leadGameObject.transform);
    }
}
