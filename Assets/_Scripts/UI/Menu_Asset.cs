using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class Menu_Asset : MonoBehaviour
{
    public enum Style { HorizontalOptions, Button }
    public Style style;
    [Tooltip("start text is what the option should look like when the game starts")]
    public string startText;
    private TextMeshProUGUI tmp;
    public int curOption;    
    [HideInInspector] public int timer;
 


    private void Start()
    {
        tmp = GetComponent<TextMeshProUGUI>();
        tmp.text = startText;
    }

    //Use this for updating text
    public virtual void Selected()
    {

    }

    public virtual void LeftInteract()
    {

    }

    public virtual void RightInteract()
    {

    }

    public virtual void UpInterract()
    {

    }

    public virtual void DownInteract()
    {

    }

    public virtual void RunCommand()
    {

    }

    public virtual void ResetState()
    {
        
    }
}
