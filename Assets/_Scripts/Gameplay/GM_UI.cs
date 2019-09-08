using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GM_UI : MonoBehaviour
{
    [SerializeField] private Text scoreText;
    [SerializeField] private GameObject canvases;

    private void Awake()
    {
        if (canvases.activeSelf == false)
        {
            canvases.SetActive(true);
        }
    }



    public void UpdateScoreText()
    {
        scoreText.text = GM_ScoreKeeper.instance.team1score.ToString() + ":" + GM_ScoreKeeper.instance.team2score.ToString();
    }

    
}
