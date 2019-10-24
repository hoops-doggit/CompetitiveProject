using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class Menu_Asset : MonoBehaviour
{
    public enum Style { HorizontalOptions }
    public Style style;
    [Tooltip("start text is what the option should look like when the game starts")]
    public string startText;
    private TextMeshProUGUI tmp;
    public string[] options;
    public int curOption;
    
    private int timer;
    public UnityEvent optionCommand;
 


    private void Start()
    {
        tmp = GetComponent<TextMeshProUGUI>();
        if (startText == null)
        {
            startText = tmp.text;
        }
        else
        {
            tmp.text = startText;
        }
    }

    public void Selected()
    {
        timer++;
        if (timer > 5)
        {
            if(options.Length > 0)
            {
                if (tmp.text != "[ " + startText + options[curOption] + " ]")
                {
                    tmp.text = "[ " + startText + options[curOption] + " ]";
                }
                else
                {
                    tmp.text = "";
                }
            }
            else
            {
                if (tmp.text != "[ " + startText + " ]")
                {
                    tmp.text = "[ " + startText + " ]";
                }
                else
                {
                    tmp.text = "";
                }
            }   

            timer = 0;
        }
    }

    public void LeftInteract()
    {
        switch (style)
        {
            case Style.HorizontalOptions:
                PreviousOption();
                break;
        }
    }

    public void RightInteract()
    {
        switch (style)
        {
            case Style.HorizontalOptions:
                NextOption();
                break;
        }
    }

    public void NextOption()
    {
        curOption++;
        if (curOption > options.Length - 1)
        {
            curOption = 0;
        }

    }

    public void PreviousOption()
    {
        curOption--;
        if (curOption < 0)
        {
            curOption = options.Length - 1;
        }
    }

    public void ResetState()
    {
        if (options.Length > 0)
        {
            tmp.text = startText + options[curOption];
        }
        else
        {
            tmp.text = startText;
        }
    }

    public void RunCommand()
    {
        optionCommand.Invoke();
    }
}
