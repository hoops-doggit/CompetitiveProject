using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu]
public class System_initialisePlayers : ScriptableObject
{
    public enum Players { Two, Four };
    public Players players;

    public void StartGame(Players players)
    {

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

    private void InitialisePlayerPrefs()
    {
        
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
