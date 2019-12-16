using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RigidbodyPause : MonoBehaviour
{
    private Rigidbody rb;
    public Vector3 velocity;
    public Vector3 angularVelocity;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void PauseRigidbody()
    {
        velocity = rb.velocity;
        rb.Sleep();
    }

    public void UnPauseRigidbody()
    {
        rb.WakeUp();
        rb.velocity = velocity;        
    }



}
