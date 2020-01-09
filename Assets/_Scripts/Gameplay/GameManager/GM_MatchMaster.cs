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
    [SerializeField] private GameObject teamBounds;
    private GameObject blocksClone;
    public List<GameObject> ballClones = new List<GameObject>();
    public List<GameObject> bullets = new List<GameObject>();
    public UnityEvent StartPlayerInitialisation;
    public bool gamePaused;
    private GM_UI ui;
    [SerializeField] private UI_MatchStartingText uiMatchText;
    private GM_ScoreKeeper scoreKeeper;

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

        scoreKeeper = GetComponent<GM_ScoreKeeper>();
        ui = GetComponent<GM_UI>();



        StartPlayerInitialisation.Invoke();
    }


    public void AddPlayer(GameObject player)
    {
        players.Add(player);
    }

    public void ResetRound()
    {
        //play match starting text;
        if (scoreKeeper.team1score + scoreKeeper.team2score != 0)
        {
            uiMatchText.Goal();
        }
        SetupPlayField();
        TurnOnTeamBounds();

        if (ballClones.Count != 0)
        {
            foreach (GameObject i in ballClones)
            {
                Destroy(i);
            }
            ballClones.Clear();
        }

        SpawnBall();


        foreach (GameObject go in players)
        {
            go.GetComponent<CH_RoundReset>().ResetPlayer();
        }

        foreach (Block_Destructable b in destructibleBlocks)
        {
            b.RegenerateBlock();
        }

    }

    public void StartRound()
    {
        TurnOffTeamBounds();
    }

    public void SpawnBall()
    {
        for (int i = 0; i < ballStartPos.Count; i++)
        {
            ballClones.Add(Instantiate(ball, ballStartPos[i].transform.position, Quaternion.identity));
            ballClones[i].GetComponent<Collider>().enabled = true;
        }
    }

    public void SetupPlayField()
    {
        if (blocksClone != null)
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
            Debug.Log("game is paused");
        }
        else
        {
            UnPauseGame();
            gamePaused = false;
            Debug.Log("game has resumed");
        }
    }

    public void PauseGame()
    {
        foreach (GameObject player in players)
        {
            player.GetComponent<CH_Movement2>().PausePlayer();
            player.GetComponent<GunControl>().ToggleInputPause();
        }

        foreach (GameObject ball in ballClones)
        {
            ball.GetComponent<RigidbodyPause>().PauseRigidbody();
        }

        foreach (GameObject bullet in bullets)
        {
            bullet.GetComponent<RigidbodyPause>().PauseRigidbody();
        }

        ui.PauseMenuStuff();
        //display the pause menu
    }

    public void UnPauseGame()
    {
        foreach (GameObject player in players)
        {
            player.GetComponent<CH_Movement2>().UnPausePlayer();
            player.GetComponent<GunControl>().ToggleInputPause();
        }

        foreach (GameObject ball in ballClones)
        {
            ball.GetComponent<RigidbodyPause>().UnPauseRigidbody();
        }

        foreach (GameObject bullet in bullets)
        {
            bullet.GetComponent<RigidbodyPause>().UnPauseRigidbody();
        }

        ui.ResumeMenuStuff();

        //turn off pause menu
    }

    public void TurnOnTeamBounds()
    {
        teamBounds.SetActive(true);
    }

    public void TurnOffTeamBounds()
    {
        teamBounds.SetActive(false);
    }
}