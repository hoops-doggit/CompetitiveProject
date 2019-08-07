using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CH_Input : MonoBehaviour {
    public PlayerNumber playerNumber;
    public bool joystick;
    public string xAxis;
    public string yAxis;
    public string shootButton;
    public string throwButton;
    public string swingButton;

    // Use this for initialization
    void Awake () {
        SetupInput(playerNumber);
	}

    private void SetupInput(PlayerNumber pn)
    {
        switch (pn)
        {
            case PlayerNumber.player1:
                xAxis = "horizontal1";
                yAxis = "vertical1";
                shootButton = "fire1";
                throwButton = "throw1";
                swingButton = "swing1";
                break;
            case PlayerNumber.player2:
                xAxis = "horizontal2";
                yAxis = "vertical2";
                shootButton = "fire2";
                throwButton = "throw2";
                swingButton = "swing2";
                break;
            case PlayerNumber.player3:
                xAxis = "horizontal3";
                yAxis = "vertical3";
                break;
            case PlayerNumber.player4:
                xAxis = "horizontal4";
                yAxis = "vertical4";
                break;
        }
    }

}
