using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GM_MatchMaster : MonoBehaviour
{
    public static GM_MatchMaster instance = null;
    [SerializeField] private List<GameObject> ballStartPos = new List<GameObject>();
    [SerializeField] private List<GameObject> players;
    public int NumberOfPlayers;
    [SerializeField] private GameObject ball;
    [SerializeField] private GameObject centreBlocks;
    [SerializeField] private List<Block_Destructable> destructibleBlocks;
    private GameObject blocksClone;
    public List<GameObject> ballClones = new List<GameObject>();
    public List<GameObject> bullets = new List<GameObject>();
    public UnityEvent StartPlayerInitialisation;
    public bool gamePaused;


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

        StartPlayerInitialisation.Invoke();
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
     

        foreach(GameObject go in players)
        {
            go.GetComponent<CH_RoundReset>().ResetPlayer();
        }

        foreach(Block_Destructable b in destructibleBlocks)
        {
            b.RegenerateBlock();
        }

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

    public void TogglePause()
    {
        if (!gamePaused)
        {
            PauseGame();
            gamePaused = true;
        }
        else
        {
            UnPauseGame();
            gamePaused = false;
        }
    }

    public void PauseGame()
    {
        foreach (GameObject player in players)
        {
            player.GetComponent<CH_Movement2>().PausePlayer();
            player.GetComponent<GunControl>().ToggleInputPause();
        }

        foreach(GameObject ball in ballClones)
        {
            ball.GetComponent<RigidbodyPause>().PauseRigidbody();
        }

        foreach (GameObject bullet in bullets)
        {
            bullet.GetComponent<RigidbodyPause>().PauseRigidbody();
        }
    }

    public void UnPauseGame()
    {
        foreach (GameObject player in players)
        {
            player.GetComponent<CH_Movement2>().UnPausePlayer();
            player.GetComponent<GunControl>().ToggleInputPause();
        }

        foreach(GameObject ball in ballClones)
        {
            ball.GetComponent<RigidbodyPause>().UnPauseRigidbody();
        }

        foreach (GameObject bullet in bullets)
        {
            bullet.GetComponent<RigidbodyPause>().UnPauseRigidbody();
        }
    }

    public void TurnOnTeamBounds()
    {
        //turn on visibility of walls
        //clamp player movement to within wall bounds
    }

    public void TurnOffTeamBounds()
    {
        //turn off visibility of walls
        //turn off clamp player movement 
    }
}
