﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CH_Input : MonoBehaviour {

    public InputMethod inputMethod;
    public PlayerNumber playerNumber;
    public bool joystick;
    public string xAxis;
    public string yAxis;
    public string swingButton;
    public string hold, brake;

    public KeyCode shootKey;
    public KeyCode throwKey;
    public KeyCode swingKey;
    public KeyCode dashKey;
    public KeyCode menuKey;
    public KeyCode aimKey;
    public string owner;
    private float deadzone = 0.1f;
    public float xInput, yInput;


    void Awake ()
    {
        SetupInput(playerNumber, inputMethod);        
	}

    private void Start()
    {
        if(GM_MatchMaster.instance != null)
        {
            GM_MatchMaster.instance.AddPlayer(this.gameObject);
        }        
    }

    private void FixedUpdate()
    {
        CalculateDeadZone(Input.GetAxisRaw(xAxis), Input.GetAxisRaw(yAxis));
    }

    private void Update()
    {
        if(playerNumber != PlayerNumber.player1)
        {
            if (Input.GetKeyDown(menuKey))
            {
                GM_MatchMaster.instance.TogglePause();
            }            
        }
        else
        {
            if(Input.GetKeyDown(menuKey) || Input.GetKeyDown(KeyCode.Escape))
            {
                Debug.Log("trying to pause");
                GM_MatchMaster.instance.TogglePause();
            }
        }
        
    }





    private void CalculateDeadZone(float horizontal, float vertical)
    {
        Vector2 stickInput = new Vector2(horizontal, vertical);
        if (stickInput.magnitude < deadzone)
        {
            stickInput = Vector2.zero;
        }
        else
        {
            stickInput = stickInput.normalized * ((stickInput.magnitude - deadzone) / (1 - deadzone));
        }
        xInput = stickInput.x;
        yInput = stickInput.y;
    }

    private void SetupInput(PlayerNumber pn, InputMethod inm)
    {
        switch (inm)
        {
            case InputMethod.joystick:
                switch (pn)
                {
                    case PlayerNumber.player1:
                        xAxis = "horizontal1";
                        yAxis = "vertical1";
                        hold = "hold1";
                        brake = "hold1";
                        shootKey = KeyCode.Joystick1Button0;
                        throwKey = KeyCode.Joystick1Button1;
                        swingKey = KeyCode.Joystick1Button2;
                        dashKey = KeyCode.Joystick1Button3;
                        menuKey = KeyCode.Joystick1Button7;
                        owner = "p1";
                        break;

                    case PlayerNumber.player2:
                        xAxis = "horizontal2";
                        yAxis = "vertical2";
                        hold = "hold2";
                        brake = "hold2";
                        shootKey = KeyCode.Joystick2Button0;
                        throwKey = KeyCode.Joystick2Button1;
                        swingKey = KeyCode.Joystick2Button2;
                        dashKey = KeyCode.Joystick2Button3;
                        menuKey = KeyCode.Joystick2Button7;
                        owner = "p2";
                        //swingKey = KeyCode.Space;
                        break;

                    case PlayerNumber.player3:
                        xAxis = "horizontal3";
                        yAxis = "vertical3";
                        hold = "hold3";
                        brake = "brake3";
                        shootKey = KeyCode.Joystick3Button0;
                        throwKey = KeyCode.Joystick3Button1;
                        swingKey = KeyCode.Joystick3Button2;
                        dashKey = KeyCode.Joystick3Button3;
                        menuKey = KeyCode.Joystick3Button7;
                        owner = "p3";
                        break;

                    case PlayerNumber.player4:
                        xAxis = "horizontal4";
                        yAxis = "vertical4";
                        hold = "hold4";
                        brake = "brake4";
                        shootKey = KeyCode.Joystick4Button0;
                        throwKey = KeyCode.Joystick4Button1;
                        swingKey = KeyCode.Joystick4Button2;
                        dashKey = KeyCode.Joystick4Button3;
                        menuKey = KeyCode.Joystick4Button7;
                        owner = "p4";
                        break;
                }
                break;
            case InputMethod.keyboard:
                switch (pn)
                {
                    case PlayerNumber.player1:
                        xAxis = "horizontal5";
                        yAxis = "vertical5";
                        hold = "hold5";
                        brake = "hold5";
                        shootKey = KeyCode.Slash;
                        throwKey = KeyCode.L;
                        swingKey = KeyCode.Period;
                        dashKey = KeyCode.Comma;
                        menuKey = KeyCode.Escape;
                        aimKey = KeyCode.M;
                        owner = "p1";
                        break;

                    case PlayerNumber.player2:
                        xAxis = "horizontal6";
                        yAxis = "vertical6";
                        hold = "hold1";
                        brake = "hold1";
                        shootKey = KeyCode.Slash;
                        throwKey = KeyCode.L;
                        swingKey = KeyCode.Period;
                        dashKey = KeyCode.Comma;
                        menuKey = KeyCode.Escape;
                        aimKey = KeyCode.M;
                        owner = "p2";
                        break;

                    case PlayerNumber.player3:
                        xAxis = "horizontal6";
                        yAxis = "vertical6";
                        hold = "hold1";
                        brake = "hold1";
                        shootKey = KeyCode.Slash;
                        throwKey = KeyCode.L;
                        swingKey = KeyCode.Period;
                        dashKey = KeyCode.Comma;
                        menuKey = KeyCode.Escape;
                        aimKey = KeyCode.M;
                        owner = "p3";
                        break;

                    case PlayerNumber.player4:
                        xAxis = "horizontal6";
                        yAxis = "vertical6";
                        hold = "hold1";
                        brake = "hold1";
                        shootKey = KeyCode.Slash;
                        throwKey = KeyCode.L;
                        swingKey = KeyCode.Period;
                        dashKey = KeyCode.Comma;
                        menuKey = KeyCode.Escape;
                        aimKey = KeyCode.M;
                        owner = "p4";
                        break;
                }
                break;
        }

    }

}
