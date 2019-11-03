using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public abstract class Menu_Asset : MonoBehaviour
{
    //runs when selected by user
    public abstract void Selected();
    public abstract void ResetState();
    public abstract void RunEvent();

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

}
