using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleGoalBlocker : MonoBehaviour
{
    void Start()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }
}
