﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_MatchStartingText : MonoBehaviour
{
    [SerializeField] private Text startGameText;
    private bool start;
    [SerializeField] private GM_MatchMaster mm;

    

    public void StartGame()
    {
        if (!start)
        {
            StartCoroutine(UpdateText());
            start = true;
        }
    }

    private IEnumerator UpdateText()
    {
        mm.ResetRound();
        yield return new WaitForSeconds(2.5f);
        startGameText.text = "3";
        yield return new WaitForSeconds(0.8f);
        startGameText.text = "2";
        yield return new WaitForSeconds(0.8f);
        startGameText.text = "1";
        yield return new WaitForSeconds(0.8f);
        startGameText.text = "GO!";
        mm.StartRound();
        yield return new WaitForSeconds(0.2f);
        startGameText.text = null;
        
    }

    private IEnumerator NewRound()
    {
        yield return new WaitForSeconds(0.4f);
    }



    public void EndOfMatch(int team)
    {
        if (team == 1)
        {
            startGameText.text = "oh hey team 1 you won!";
            StartCoroutine(RestartGame());
        }
        if (team == 2)
        {
            startGameText.text = "wowowowow team 2 you won!";
            StartCoroutine(RestartGame());
        }
    }

    public void Goal()
    {
        startGameText.text = "Goal!!!";
        StartCoroutine(EraseText());
    }

    public IEnumerator EraseText()
    {
        yield return new WaitForSeconds(2.5f);
        startGameText.text = "new round in \n 3";
        yield return new WaitForSeconds(0.8f);
        startGameText.text = "new round in 2";
        yield return new WaitForSeconds(0.8f);
        startGameText.text = "new round in 1";
        yield return new WaitForSeconds(0.8f);
        startGameText.text = "!!GO!!";
        mm.StartRound();
        yield return new WaitForSeconds(0.2f);        
        startGameText.text = null;
    }

    private IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(10);

        SceneManager.LoadScene(1);
    }




}
