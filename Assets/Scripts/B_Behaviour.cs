using UnityEngine;

public class B_Behaviour : MonoBehaviour
{
    private Collider col;
    private Rigidbody rb;
    private bool ballHeld;

    private void Start()
    {
        ballHeld = false;
        col = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        FreezeAllRigidbodyConstraints();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponentInParent<CH_BallInteractions>().PickUpBall(gameObject, this);
            BallPickedUp();
        }
    }

    public void BallPickedUp()
    {
        FreezeAllRigidbodyConstraints();
        col.enabled = false;
    }

    public void BallThrown()
    {
        UnfreezeAllRigidbodyConstraints();
        col.enabled = true;
    }

    private void FreezeAllRigidbodyConstraints()
    {
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    private void UnfreezeAllRigidbodyConstraints()
    {
        rb.constraints = RigidbodyConstraints.None;
    }
}
