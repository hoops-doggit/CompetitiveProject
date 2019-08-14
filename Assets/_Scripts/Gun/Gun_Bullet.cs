using UnityEngine;

public class Gun_Bullet : MonoBehaviour {

    float age = 0;
    public float maxAge = 800;
    public float initialSpeed = 200;
    private Rigidbody rb;
    public Vector3 direction;
    private int i = 0;
    private bool directionSwitch = true;


    // Update is called once per frame
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * initialSpeed, ForceMode.Acceleration);
    }
    void FixedUpdate () {
        age++;
        if (age > maxAge)
        {
            Death();
        }

        i++;
        if (directionSwitch)
        {
            if (i > 1)
            {
                direction = rb.velocity;
                directionSwitch = false;
            }
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

    public void HitByBat(Transform t, float hitStrength)
    {
        rb.AddForce(t.forward * (rb.velocity.magnitude + hitStrength), ForceMode.Acceleration);
    }

    

}
