using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu]
public class Menu_Commands : ScriptableObject
{
    //this is just a container for holding code that will be run during menu events


    public enum Players { Two, Four };
    public Players players;
    public int playerNo;
    public PlayerInitialiser pi;

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

    private void TwoPlayers()
    {
        UpdatePlayerNumber(2);
    }

    public void FourPlayers()
    {
        UpdatePlayerNumber(4);
    }

    public void UpdatePlayerNumber(int i)
    {
        pi.players = i;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
