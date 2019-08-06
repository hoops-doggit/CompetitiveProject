using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM_GoalBox : MonoBehaviour
{
    public GoalBoxes gb;
    private Material mat;
    private Color preGlow;
    [ColorUsageAttribute(true, true, 0f, 8f, 0.125f, 3f)]
    public Color scoredGlow;
    private GameObject ball;

    private void Start()
    {
        mat = gameObject.GetComponent<Renderer>().material;
        preGlow = mat.GetColor("_EmissionColor");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "ball")
        {
            ball = other.gameObject;
            GoalScored();
        }
    }

    private void GoalScored()
    {
        GM_ScoreKeeper.instance.UpdateScore(gb);
        StartCoroutine("GoalScoredTimeRelatedStuff");
        gameObject.GetComponent<Collider>().enabled = false;
    }

    private IEnumerator GoalScoredTimeRelatedStuff()
    {
        mat.SetColor("_EmissionColor", scoredGlow);
        yield return new WaitForSeconds(3);
        mat.SetColor("_EmissionColor", preGlow);
        GM_MatchMaster.instance.ResetBall(ball);
        gameObject.GetComponent<Collider>().enabled = true;


    }


}
