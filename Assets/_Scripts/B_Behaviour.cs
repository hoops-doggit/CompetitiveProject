using UnityEngine;

public class B_Behaviour : MonoBehaviour
{
    private Collider col;
    private Rigidbody rb;
    private bool ballHeld;
    [SerializeField] private CH_BaseballBatBehaviour bbbb;

    private void Start()
    {
        ballHeld = false;
        col = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        FreezeAllRigidbodyConstraints();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponentInParent<CH_BallInteractions>().PickUpBall(gameObject, this);
            BallPickedUp();
        }

        
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "swingZone")
        {
            bbbb.StopSwing();
            HitBall(collision.gameObject.transform.parent.transform, bbbb.hitStrength);
        }
    }

    public void HitBall(Transform playerT, float hitStrength)
    {
        Debug.Log("got hit");
        UnfreezeAllRigidbodyConstraints();
        rb.AddForce(playerT.forward * hitStrength, ForceMode.Acceleration);
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
