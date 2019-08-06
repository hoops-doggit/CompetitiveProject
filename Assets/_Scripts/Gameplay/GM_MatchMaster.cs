using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM_MatchMaster : MonoBehaviour
{
    public static GM_MatchMaster instance = null;
    [SerializeField] private Transform ballStartPos;


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
        ball.transform.parent = null;
        ball.GetComponent<B_Behaviour>().FreezeAllRigidbodyConstraints();
        ball.transform.position = ballStartPos.position;
    }
}
