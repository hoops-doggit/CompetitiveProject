using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM_ScoreKeeper : MonoBehaviour
{
    public static GM_ScoreKeeper instance = null;
    public int team1score = 0;
    public int team2score = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if(instance != null)
        {
            Destroy(gameObject);
        }
    }

    public void UpdateScore(GoalBoxes gb)
    {
        if (gb.Equals(GoalBoxes.team1))
        {
            Team1Scored();
        }
        else if (gb.Equals(GoalBoxes.team2))
        {
            Team2Scored();
        }
    }

    private void Team1Scored()
    {
        team1score++;
        UpdateScoreUI();
    }

    private void Team2Scored()
    {
        team2score++;
        UpdateScoreUI();
    }
    
    private void UpdateScoreUI()
    {
        GetComponent<GM_UI>().UpdateScoreText();
    }
}
