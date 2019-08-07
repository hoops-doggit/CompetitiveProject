using UnityEngine;

public class Gun_ChargingBullet : MonoBehaviour
{
    public Material charging;
    public Material charged;
    private Renderer r;

    private void Start()
    {
        r = GetComponent<Renderer>();
    }


    public void SetChargedMat()
    {
        r.material = charged;
    }
}
