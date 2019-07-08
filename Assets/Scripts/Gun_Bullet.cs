using UnityEngine;

public class Gun_Bullet : MonoBehaviour {

    float age = 0;
    public float maxAge = 800;
    public float initialSpeed = 200;


    // Update is called once per frame
    private void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * initialSpeed, ForceMode.Acceleration);
        //transform.parent = GameObject.Find("BulletHolder").transform;
    }
    void FixedUpdate () {
        age++;
        if (age > maxAge)
        {
            Destroy(gameObject);
        }
	}
}
