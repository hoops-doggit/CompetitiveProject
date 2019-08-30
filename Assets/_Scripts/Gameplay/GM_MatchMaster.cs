using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM_MatchMaster : MonoBehaviour
{
    public static GM_MatchMaster instance = null;
    [SerializeField] private GameObject ballStartPos;
    [SerializeField] private List<GameObject> players;
    [SerializeField] private GameObject ball;
    [SerializeField] private GameObject destructibleBlocks;
    private GameObject blocksClone;
    private GameObject ballClone;


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

    private void Start()
    {
        ballStartPos.SetActive(false);
        ResetRound();
    }

    public void ResetRound()
    {
        SetupPlayField();
        Destroy(ballClone);
        SpawnBall();
        Debug.Log("ResettingRound");
        ballClone.GetComponent<Collider>().enabled = true;

        foreach(GameObject go in players)
        {
            go.GetComponent<CH_RoundReset>().ResetPlayer();
        }
        Debug.Log("Just reset players");
    }

    public void SpawnBall()
    {
        ballClone = Instantiate(ball, ballStartPos.transform.position, Quaternion.identity);
    }

    public void SetupPlayField()
    {
        blocksClone = Instantiate(destructibleBlocks);
    }
}
