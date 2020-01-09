using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffDebugStuff : MonoBehaviour
{
    public GameObject[] debugStuff;
    // Start is called before the first frame update
    void Awake()
    {
        foreach (GameObject go in debugStuff)
        {
            go.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
