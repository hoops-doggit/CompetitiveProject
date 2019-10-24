using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu]
public class PlayerInitialiser : ScriptableObject
{
    public GameObject playerPrefab;
    public int players;

    public void TwoPlayers()
    {
        Instantiate(playerPrefab);
    }

    public void DebugPlayers()
    {
        Debug.Log("players = " + players);
    }

}
