using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class Menu_Asset : MonoBehaviour
{
    public bool selected;
    public TextMeshProUGUI text;
    private string textText;
    private int timer;
    public UnityEvent optionCode;


    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        textText = text.text;
    }

    public void Selected()
    {
        timer++;
        if (timer > 5)
        {
            if (text.text != "")
            {
                text.text = "";
            }
            else
            {
                text.text = "- "+textText+" -";
            }
            timer = 0;
        }
    }

    public void ResetState()
    {
        text.text = textText;
    }

    public void RunOption()
    {
        optionCode.Invoke();
    }
}
