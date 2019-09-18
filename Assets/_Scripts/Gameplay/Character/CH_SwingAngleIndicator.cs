using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CH_SwingAngleIndicator : MonoBehaviour
{
    MeshRenderer mr;
    [SerializeField] Material[] mats; 


    // Start is called before the first frame update
    void Start()
    {
        mr = GetComponent<MeshRenderer>();
        AngleDeselected();
    }

    public void AngleSelected()
    {
        mr.material = mats[1];
    }

    public void AngleDeselected()
    {
        mr.material = mats[0];
    }
}
