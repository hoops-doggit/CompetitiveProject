using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuAsset : MonoBehaviour
{
    public bool selected;
    public int position;
    public TextMeshPro text;
    private string textText;
    private int timer;
    //menu assets have 
    //visual representation eg text/image
    //bool - selected or not
    //int - 

    private void Start()
    {
        text = GetComponent<TextMeshPro>();
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
                text.text = textText;
            }
            timer = 0;
        }
    }
}
