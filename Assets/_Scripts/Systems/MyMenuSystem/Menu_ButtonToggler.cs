using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class Menu_ButtonToggler : Menu_Asset
{
    public string startText;
    private TextMeshProUGUI tmp;
    private float timer;
    [SerializeField] private List<Color> colours = new List<Color>(2);
    public UnityEvent[] buttonEvents = new UnityEvent[2];
    public bool toggled = false;

    private void Start()
    {
        tmp = GetComponent<TextMeshProUGUI>();
        tmp.text = startText;
    }

    public override void Selected()
    {
        timer+=Time.deltaTime;
        if (timer > 0.1f)
        {           
            if (tmp.text != "[ " + startText + " ]")
            {
                tmp.color = colours[1];
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
        tmp.color = colours[0];
    }

    public override void RunEvent()
    {
        if (!toggled)
        {
            buttonEvents[1].Invoke();
            toggled = true;
        }
        else
        {
            buttonEvents[0].Invoke();
            toggled = false;
        }        
    }
}
