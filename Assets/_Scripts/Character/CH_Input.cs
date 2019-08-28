using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CH_Input : MonoBehaviour {
    public PlayerNumber playerNumber;
    public bool joystick;
    public string xAxis;
    public string yAxis;
    public string swingButton;

    public KeyCode shootKey;
    public KeyCode throwKey;
    public KeyCode swingKey;
    public string owner;

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
                shootKey = KeyCode.Joystick1Button0;
                throwKey = KeyCode.Joystick1Button1;
                swingKey = KeyCode.Joystick1Button2;
                owner = "p1";

                break;
            case PlayerNumber.player2:
                xAxis = "horizontal2";
                yAxis = "vertical2";
                shootKey = KeyCode.Joystick2Button0;
                throwKey = KeyCode.Joystick2Button1;
                //swingKey = KeyCode.Joystick2Button2;
                swingKey = KeyCode.Space;
                owner = "p2";
                break;
            case PlayerNumber.player3:
                xAxis = "horizontal3";
                yAxis = "vertical3";
                shootKey = KeyCode.Joystick3Button0;
                throwKey = KeyCode.Joystick3Button1;
                swingKey = KeyCode.Joystick3Button2;
                owner = "p3";
                break;
            case PlayerNumber.player4:
                xAxis = "horizontal4";
                yAxis = "vertical4";
                shootKey = KeyCode.Joystick4Button0;
                throwKey = KeyCode.Joystick4Button1;
                swingKey = KeyCode.Joystick4Button2;
                owner = "p4";
                break;
        }
    }

}
