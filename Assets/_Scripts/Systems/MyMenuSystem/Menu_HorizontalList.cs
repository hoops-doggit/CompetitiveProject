using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class Menu_HorizontalList : Menu_Asset
{
    private TextMeshProUGUI tmp;
    public string startText;    
    private int curOption;    
    private int timer;
    public string[] options; //used to render options as text
    public UnityEvent[] commands;

    private void Start()
    {
        tmp = GetComponent<TextMeshProUGUI>();
        tmp.text = startText;
        timer = 6;
    }

    public override void Selected()
    {
        timer++;
        if (timer > 5)
        {
            if (tmp.text != "[ " + startText + options[curOption] + " ]")
            {
                tmp.text = "[ " + startText + options[curOption] + " ]";
            }
            else
            {
                tmp.text = "";
            }
            timer = 0;
        }
    }

    public override void ResetState()
    {
        tmp.text = startText + options[curOption];
        RunEvent();
    }

    public override void LeftInteract()
    {
        curOption--;
        if (curOption < 0)
        {
            curOption = options.Length - 1;
        }
    }

    public override void RightInteract()
    {
        curOption++;
        if (curOption > options.Length - 1)
        {
            curOption = 0;
        }
        base.RightInteract();
    }

    public override void RunEvent()
    {
        commands[curOption].Invoke();
    }
}
