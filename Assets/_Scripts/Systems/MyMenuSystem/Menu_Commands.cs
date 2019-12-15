using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Players { Two, Four };

[CreateAssetMenu]
public class Menu_Commands : ScriptableObject
{
    //this is just a container for holding code that will be run during menu events

    
    public Players players;
    public int playerNo;
    public PlayerInitialiser pi;
    public bool controls = false;
    public GameObject controlss;


    public void StartGame()
    {
        //get which number the players are set to
        switch (players)
        {
            case Players.Two:
                Debug.Log("Load 2 player game");
                SceneManager.LoadScene("GunnerSoccer2P");
                
                break;
            case Players.Four:
                Debug.Log("Load 4 player game");
                SceneManager.LoadScene("GunnerSoccer4P");
                break;
        }        
    }

    public void TwoPlayers()
    {
        players = Players.Two;
        Debug.Log("players 2");
    }

    public void FourPlayers()
    {
        players = Players.Four;
        Debug.Log("players 4");
    }

    public void DisplayControls()
    {
        if (!controls)
        {
            controlss.SetActive(true);
            controls = true;
        }
        else
        {
            controlss.SetActive(false);
            controls = false;
        }

    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
