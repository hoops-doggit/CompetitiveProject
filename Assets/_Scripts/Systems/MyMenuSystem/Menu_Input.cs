using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MenuStates { waitingForMenu, interactingWithMenu, notListeningForInput, runningExecution, transitioningScreen}
//[RequireComponent(typeof(Menu_Manager))]
public class Menu_Input : MonoBehaviour
{
    [SerializeField] private string menuName;
    public MenuStates mStates;

    //this script should catch all input
    public Vector2 joy1In;
    public Vector2 joy2In;
    public Vector2 joy3In;
    public Vector2 joy4In;
    public Vector2 arrowIn;

    public Vector2 menuInput;

    public float menuBuffer = 0;
    public float inputDelay = 0.21f;
    private Menu_Manager mm;

    private void Start()
    {
        mm = GetComponent<Menu_Manager>();
    }


    // Update is called once per frame
    void Update()
    {



        switch (mStates)
        {
            case MenuStates.interactingWithMenu:

                #region gamepad vectors
                joy1In = new Vector2(Input.GetAxis("horizontal1"), Input.GetAxis("vertical1"));
                joy2In = new Vector2(Input.GetAxis("horizontal2"), Input.GetAxis("vertical2"));
                joy3In = new Vector2(Input.GetAxis("horizontal3"), Input.GetAxis("vertical3"));
                joy4In = new Vector2(Input.GetAxis("horizontal4"), Input.GetAxis("vertical4"));
                #endregion

                #region gamepad accept buttons
                if(Input.GetButtonDown("fire1") || Input.GetButtonDown("fire2") || Input.GetButtonDown("fire3") || Input.GetButtonDown("fire4") || Input.GetKeyDown(KeyCode.Return))
                {
                    mm.RunCommand();
                }
                #endregion


                #region arrows
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    arrowIn.y = 1;
                }
                else if (Input.GetKey(KeyCode.DownArrow))
                {
                    arrowIn.y = -1;
                }
                else { arrowIn.y = 0; }
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    arrowIn.x = 1;
                }
                else if (Input.GetKey(KeyCode.LeftArrow))
                {
                    arrowIn.x = -1;
                }
                else { arrowIn.x = 0; }
                #endregion


                menuInput = (joy1In + joy2In + joy3In + joy4In + arrowIn).normalized;
                if (menuBuffer > 0)
                {
                    menuBuffer -= Time.deltaTime;
                }


                if (menuBuffer <= 0)
                {
                    if (menuInput.y > 0.5)
                    {
                        mm.PreviousOption();
                        menuBuffer = inputDelay;
                    }
                    else if (menuInput.y < -0.5)
                    {
                        mm.NextOption();
                        menuBuffer = inputDelay;
                    }
                    else if (menuInput.x > 0.5)
                    {
                        mm.RightOption();
                        menuBuffer = inputDelay;
                    }
                    else if (menuInput.x < -0.5)
                    {
                        mm.LeftOption();
                        menuBuffer = inputDelay;
                    }
                }
                break;
            case MenuStates.notListeningForInput:
                break;
        }
        
        


    }


}
