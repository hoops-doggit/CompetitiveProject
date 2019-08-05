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
    }
    void FixedUpdate () {
        age++;
        if (age > maxAge)
        {
            Death();
        }
	}

    private void OnCollisionEnter(Collision collision)
    {
        Collider col = GetComponent<Collider>();
        col.enabled = false;
        if (collision.gameObject.GetComponent<Block_Destructible>() != null) {
            collision.gameObject.GetComponent<Block_Destructible>().Bumped();
        }
        Death();
    }

    private void Death()
    {
        Destroy(gameObject);
    }
}
