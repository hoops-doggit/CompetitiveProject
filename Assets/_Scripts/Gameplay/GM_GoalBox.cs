using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM_GoalBox : MonoBehaviour
{
    public GoalBoxes gb;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "ball")
        {
            GoalScored();
        }
    }

    private void GoalScored()
    {
        GM_ScoreKeeper.instance.UpdateScore(gb);
    }
}
