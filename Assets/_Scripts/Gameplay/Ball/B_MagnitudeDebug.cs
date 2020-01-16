using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class B_MagnitudeDebug : MonoBehaviour
{
    public bool displayDebugMagnitude;
    public GameObject tmpObject;
    public TextMeshPro tmpComp;
    public float textSize;

    // Start is called before the first frame update
    void Start()
    {
        if(tmpObject == null && displayDebugMagnitude)
        {
            tmpObject = new GameObject();
            tmpObject.AddComponent<TextMeshPro>();
            tmpComp = tmpObject.GetComponent<TextMeshPro>();
            tmpComp.fontSize = textSize;
            tmpComp.enableWordWrapping = false;
            tmpComp.alignment = TextAlignmentOptions.TopGeoAligned;

        }
    }

    // Update is called once per frame
    void Update()
    {
        if(tmpObject != null)
        {
            tmpObject.transform.position = new Vector3(transform.localPosition.x, 1, transform.localPosition.z);
            tmpComp.text = GetComponentInParent<Rigidbody>().velocity.magnitude.ToString("F2");
        }
        
    }
}
