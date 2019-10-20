using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GM_UI : MonoBehaviour
{
    [SerializeField] private Text scoreText;
    [SerializeField] private GameObject canvases;
    public Text fpsText;

    float deltaTime = 0.0f;

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

    }

    private void FixedUpdate()
    {
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        fpsText.text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps).ToString();
    }



    private void Awake()
    {
        if (canvases.activeSelf == false)
        {
            canvases.SetActive(true);
        }

        Application.targetFrameRate = 60;
    }



    public void UpdateScoreText()
    {
        scoreText.text = GM_ScoreKeeper.instance.team1score.ToString() + ":" + GM_ScoreKeeper.instance.team2score.ToString();
    }

    
}
