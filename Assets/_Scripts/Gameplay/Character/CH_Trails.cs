using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CH_Trails : MonoBehaviour
{
    [SerializeField] CH_Input input;
    int owner;
    public Gradient[] pre;
    public float preTime;
    public Gradient[] during;
    public float duringTime;
    public Gradient[] post;
    public float postTime;
    private TrailRenderer tr;

    private void Start()
    {
        switch (input.playerNumber)
        {
            case PlayerNumber.player1:
                owner = 0;
                break;
            case PlayerNumber.player2:
                owner = 1;
                break;
            case PlayerNumber.player3:
                owner = 2;
                break;
            case PlayerNumber.player4:
                owner = 3;
                break;
        }

        tr = GetComponent<TrailRenderer>();
        tr.colorGradient = pre[owner];
    }

    // Start is called before the first frame update
    public void Dashing()
    {
        tr = GetComponent<TrailRenderer>();
        tr.colorGradient = during[owner];
        tr.time = duringTime;
    }
    public void CoolingDown()
    {
        tr = GetComponent<TrailRenderer>();
        tr.colorGradient = post[owner];
        tr.time = postTime;
    }
    public void Ready()
    {
        tr = GetComponent<TrailRenderer>();
        tr.colorGradient = pre[owner];
        tr.time = preTime;
    }
}
