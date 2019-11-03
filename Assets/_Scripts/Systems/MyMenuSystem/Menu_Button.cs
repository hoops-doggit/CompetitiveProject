using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class Menu_Button : Menu_Asset
{
    public string startText;
    private TextMeshProUGUI tmp;
    private int timer;
    public UnityEvent buttonEvent;

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
    }

    public override void ResetState()
    {
        tmp.text = startText;
    }

    public override void RunEvent()
    {
        buttonEvent.Invoke();
    }
}
