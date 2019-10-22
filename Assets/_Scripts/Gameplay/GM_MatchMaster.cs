using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM_MatchMaster : MonoBehaviour
{
    public static GM_MatchMaster instance = null;
    [SerializeField] private List<GameObject> ballStartPos = new List<GameObject>();
    [SerializeField] private List<GameObject> players;
    [SerializeField] private GameObject ball;
    [SerializeField] private GameObject centreBlocks;
    [SerializeField] private List<Block_Destructable> destructibleBlocks;
    private GameObject blocksClone;
    public List<GameObject> ballClones = new List<GameObject>();


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
        foreach (GameObject i in ballStartPos)
        {
            i.SetActive(false);
        }
    }

    public void AddPlayer(GameObject player)
    {
        players.Add(player);
    }

    public void ResetRound()
    {

        SetupPlayField();

        if(ballClones.Count !=0)
        {
            foreach (GameObject i in ballClones)
            {
                Destroy(i);
            }
            ballClones.Clear();
        }        

        SpawnBall();

        Debug.Log("ResettingRound");        

        foreach(GameObject go in players)
        {
            go.GetComponent<CH_RoundReset>().ResetPlayer();
        }

        foreach(Block_Destructable b in destructibleBlocks)
        {
            b.RegenerateBlock();
        }

        Debug.Log("Just reset players");
    }

    public void SpawnBall()
    {
        for(int i = 0; i < ballStartPos.Count; i++)
        {
            ballClones.Add(Instantiate(ball, ballStartPos[i].transform.position, Quaternion.identity));
            ballClones[i].GetComponent<Collider>().enabled = true;
        }
    }

    public void SetupPlayField()
    {
        if(blocksClone != null)
        {
            Destroy(blocksClone);
        }
        //blocksClone = Instantiate(centreBlocks);
    }
}
