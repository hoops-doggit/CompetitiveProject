using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_New : MonoBehaviour
{

    private Rigidbody rb;
    [SerializeField]private float startSpeed;


    private void OnTriggerEnter(Collider other)
    {
        Vector3 newDirection = transform.position - other.transform.position;
        newDirection.y = 0;
        Destroy(other.gameObject);

        rb.AddForce(newDirection.normalized * startSpeed, ForceMode.Acceleration);
    }



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
