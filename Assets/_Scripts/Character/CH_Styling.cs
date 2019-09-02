using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CH_Styling : MonoBehaviour
{
    [SerializeField] private List<CH_StyleType> styles = new List<CH_StyleType>();


    public Renderer[] body;
    public Renderer[] gun;

    private bool hitFlash;
    private Color bodyColour;


    // Start is called before the first frame update
    void Start()
    {
        CH_Input i = GetComponent<CH_Input>();
        if(i.playerNumber == PlayerNumber.player1)
        {
            PropogateStyle(0);
        }
        
        else if(i.playerNumber == PlayerNumber.player2)
        {
            PropogateStyle(1);
        }

        else if (i.playerNumber == PlayerNumber.player3)
        {
            PropogateStyle(2);
        }

        else if (i.playerNumber == PlayerNumber.player4)
        {
            PropogateStyle(3);
        }

        bodyColour = body[0].material.color;
    }

    private void Update()
    {
        if (hitFlash)
        {
            foreach (Renderer r in body)
            {
                r.material.color = Color.white;
            }
        }
    }





    private void PropogateStyle(int styleNo)
    {
        foreach (Renderer r in body)
        {
            r.material = styles[styleNo].body;
        }

        foreach (Renderer r in gun)
        {
            r.material = styles[styleNo].gun;
        }
    }

    public void BodyHitFlash()
    {
        foreach (Renderer r in body)
        {
            r.material.color = Color.white;
        }

        StartCoroutine(ReturnHitFlash());
    }

    private IEnumerator ReturnHitFlash()
    {
        yield return new WaitForSeconds(0.01f);

        foreach (Renderer r in body)
        {
            r.material.color = bodyColour;
        }
    }




}
