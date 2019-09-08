using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Enabler : MonoBehaviour
{
    private void Awake()
    {
        if (gameObject.activeSelf == false)
        {
            gameObject.SetActive(true);
        }
    }
}
