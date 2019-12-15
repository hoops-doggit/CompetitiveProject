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
    public int curOption;    
    private float timer;
    public string[] options; //used to render options as text
    public UnityEvent[] commands;
    public List<Color> colours = new List<Color>(2);
    public bool highlighted;

    private void Start()
    {
        tmp = GetComponent<TextMeshProUGUI>();
        tmp.text = startText;
        timer = 6;
    }

    public override void Selected()
    {
        timer += Time.deltaTime ;
        if (timer > 0.1) //0.2 = seconds before switching highlighted mode
        {
            if (!highlighted)
            {
                tmp.color = colours[1];
                tmp.text = "< " + startText + options[curOption] + " >";
                highlighted = true;
            }
            else
            {
                tmp.color = colours[0];
                tmp.text = " " ;
                highlighted = false;
            }
            timer = 0;
        }
    }

    public override void ResetState()
    {
        tmp.text = startText + options[curOption];
        tmp.color = colours[0];
        RunEvent();
        highlighted = false;

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
        Debug.Log("run event " + curOption);
    }
}
