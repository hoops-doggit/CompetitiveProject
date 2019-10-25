using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class MA_Button : Menu_Asset
{
    public enum Style { HorizontalOptions, Button }
    public Style style;
    [Tooltip("start text is what the option should look like when the game starts")]
    public string startText;
    private TextMeshProUGUI tmp;
    public string[] options;
    public int curOption;
    
    public UnityEvent optionCommand;
 


    private void Start()
    {
        tmp = GetComponent<TextMeshProUGUI>();
        tmp.text = startText;

    }

    public override void Selected()
    {
        timer++;
        if (timer > 5)
        {           
            if (tmp.text != "[ " + startText + " ]")
            {
                tmp.text = "[ " + startText + " ]";
            }
            else
            {
                tmp.text = "";
            }
            timer = 0;
        }
        base.Selected();
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

        if(style == Style.HorizontalOptions)
        {
            optionCommand.Invoke();
        }
    }

    public override void RunCommand()
    {
        optionCommand.Invoke();
    }
}
