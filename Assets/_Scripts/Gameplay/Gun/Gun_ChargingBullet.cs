using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Gun_ChargingBullet : MonoBehaviour
{
    public Material[] charging;
    public List<Material> charged = new List<Material>();
    private Renderer r;
    private string playerNumber;
    private PlayerNumber pn;

    private void Start()
    {
        r = GetComponent<Renderer>();
        pn = GetComponentInParent<CH_Input>().playerNumber;
        switch (pn)
        {
            case PlayerNumber.player1:
                r.material = charging[0];
                break;
            case PlayerNumber.player2:
                r.material = charging[1];
                break;
            case PlayerNumber.player3:
                r.material = charging[2];
                break;
            case PlayerNumber.player4:
                r.material = charging[3];
                break;
        }

    }


    public void SetChargedMat(int i)
    {
        r.material = charged[i];
    }
}
