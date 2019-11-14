using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TutorialManager : MonoBehaviour
{

    public enum Tut { movement, shoot};
    public Tut _tut;
    public CH_Input inputs;
    //instructions
    //listen for the required feedback
    //Maybe ask for it a couple times? 
    //Que next step/phase

    public UnityEventQueueSystem hahaha;

    public List<UnityEvent> listOfEvents;

    public bool checkMovement,  checkShoot, checkAim;
    private bool movementX, movementY, shot;

    // Start is called before the first frame update
    void Start()
    {
        //put check movement info onscreen
        checkMovement = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (checkMovement)
        {
            if (!movementX)
            {
                CheckMovement(inputs);
            }
            else if (!movementY)
            {
                CheckMovement(inputs);
            }
            else
            {
                //Start animation to hide instructions
                checkMovement = false;
                checkShoot = true;
            }
        }
        else if (checkShoot)
        {
            //display shot instructions 
            CheckShoot(inputs);

        }
        else if (checkAim)
        {

        }
    }

    private void CheckMovement(CH_Input inp)
    {
        if(Input.GetAxis(inp.xAxis) > 1)
        {
            movementX = true;
        }
        if (Input.GetAxis(inp.yAxis) > 1)
        {
            movementY = true;
        }
    }

    private void CheckShoot(CH_Input inp)
    {
        if (Input.GetKey(inp.shootKey))
        {
            checkShoot = false;
        }
    }





}
