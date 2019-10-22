using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu_Options : MonoBehaviour
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
}
