using UnityEngine;

public class B_Behaviour : MonoBehaviour
{
    private Collider col;
    private Rigidbody rb;
    private bool ballHeld;
    public bool free;
    [SerializeField] private CH_BaseballBatBehaviour bbbb;

    private void Start()
    {
        ballHeld = false;
        col = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        FreezeAllRigidbodyConstraints();
        free = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && free)
        {
            collision.gameObject.GetComponentInParent<CH_BallInteractions>().PickUpBall(gameObject, this);
        }        
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //free = true;
        }
    }

    public void HitBall(Transform playerT, float hitStrength)
    {
        if (!ballHeld)
        {
            Debug.Log("got hit");
            UnfreezeAllRigidbodyConstraints();
            rb.AddForce(playerT.forward * hitStrength, ForceMode.Acceleration);
        }        
    }

    public void BallPickedUp()
    {
        ballHeld = true;
        FreezeAllRigidbodyConstraints();
        free = false;
        col.enabled = false;
    }

    public void BallThrown()
    {
        ballHeld = false;
        UnfreezeAllRigidbodyConstraints();
        free = true;
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
