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
    private float timer;
    [SerializeField] private List<Color> colours = new List<Color>(2);
    public UnityEvent buttonEvent;

    private void Awake()
    {
        tmp = GetComponent<TextMeshProUGUI>();
        startText = tmp.text;
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
        buttonEvent.Invoke();
    }
}
