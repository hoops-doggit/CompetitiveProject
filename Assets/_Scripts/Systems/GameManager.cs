using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager inst;
    public float lengthOfTimeFreeze;

    private void Awake()
    {
        if(inst == null)
        {
            inst = this;
        }
    }

    public void TimeFreeze()
    {
        StartCoroutine(TimeFreezeCo());
    }

    private IEnumerator TimeFreezeCo()
    {
        Time.timeScale = 0.001f;
        yield return new WaitForSecondsRealtime(lengthOfTimeFreeze);
        Time.timeScale = 1;
    }
}
