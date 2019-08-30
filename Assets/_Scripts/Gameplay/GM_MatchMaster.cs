using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM_MatchMaster : MonoBehaviour
{
    public static GM_MatchMaster instance = null;
    [SerializeField] private Transform ballStartPos;
    [SerializeField] private List<GameObject> players;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }
    }

    public void ResetBall(GameObject ball)
    {
        Debug.Log("ResettingRound");
        ball.transform.parent = null;
        ball.GetComponent<Rigidbody>().Sleep();
        ball.transform.position = ballStartPos.position;
        ball.GetComponent<Collider>().enabled = true;

        foreach(GameObject go in players)
        {
            go.GetComponent<CH_RoundReset>().ResetPlayer();
        }
        Debug.Log("Just reset players");
    }
}
