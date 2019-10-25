using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class Menu_Asset_HToggle : Menu_Asset
{
    private TextMeshProUGUI tmp;
    public string[] options;
    public int hCurOption;    
    public UnityEvent[] commands;

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

    public override void LeftInteract()
    {
        hCurOption--;
        if (curOption < 0)
        {
            hCurOption = options.Length - 1;
        }
    }

    public override void RightInteract()
    {
        hCurOption++;
        if (hCurOption > options.Length - 1)
        {
            hCurOption = 0;
        }
        base.RightInteract();
    }

    public override void RunCommand()
    {
        commands[curOption].Invoke();
    }
}
