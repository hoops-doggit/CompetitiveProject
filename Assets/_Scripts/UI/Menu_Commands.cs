using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu]
public class Menu_Commands : ScriptableObject
{
    public void Start2PlayerGame()
    {
        SceneManager.LoadScene("GunnerSoccer4P");
        Debug.Log("Load 2 player game");
    }

    public void Start4PlayerGame()
    {
        Debug.Log("Load 4 player game");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
