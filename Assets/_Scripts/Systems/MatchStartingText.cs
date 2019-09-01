using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchStartingText : MonoBehaviour
{
    private Text startGameText;
    private bool start;
    [SerializeField] private GM_MatchMaster mm;

    public void StartGame()
    {
        startGameText = GetComponent<Text>();
        if (!start)
        {
            StartCoroutine(UpdateText());
            start = true;
        }
    }

    private IEnumerator UpdateText()
    {
        yield return new WaitForSeconds(4);
        startGameText.text = "3";
        yield return new WaitForSeconds(1);
        startGameText.text = "2";
        yield return new WaitForSeconds(1);
        startGameText.text = "1";
        yield return new WaitForSeconds(1);
        startGameText.text = "GO!";
        mm.ResetRound();
        yield return new WaitForSeconds(0.2f);
        startGameText.text = null;
    }


}
