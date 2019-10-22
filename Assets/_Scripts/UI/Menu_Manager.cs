using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoboRyanTron.SearchableEnum;

public class Menu_Manager : MonoBehaviour
{
    public enum MenuStyles { Horizontal, Vertical, Grid };
    public MenuStyles menuStyles;
    public int currentPosition;
    public List<Menu_Asset> menuOptions;
    [SearchableEnum]
    public KeyCode confirmButton, backButton, forwardButton, leftButton, rightButton;


    // Update is called once per frame
 

    public void NextOption()
    {
        ResetSelectionState();
        currentPosition++;
        if(currentPosition > menuOptions.Count -1)
        {
            currentPosition = 0;
        }
    }

    public void PreviousOption()
    {
        ResetSelectionState();
        currentPosition--;
        if (currentPosition < 0)
        {
            currentPosition = menuOptions.Count;
        }
    }

    public void LeftOption()
    {

    }

    public void RightOption()
    {

    }

    public void ResetSelectionState()
    {
        menuOptions[currentPosition].ResetState();
    }


    public void RunCommand(int i)
    {
        menuOptions[i].RunOption();
    }

    void Update()
    {
        menuOptions[currentPosition].Selected();
        switch (menuStyles)
        {
            case MenuStyles.Vertical:
                if (Input.GetKeyDown(backButton))
                {
                    PreviousOption();
                }
                if (Input.GetKeyDown(forwardButton))
                {
                    NextOption();
                }
                if (Input.GetKeyDown(confirmButton))
                {
                    RunCommand(currentPosition);
                }
                break;
            case MenuStyles.Horizontal:
                break;
            case MenuStyles.Grid:
                break;
        }
    }



}
