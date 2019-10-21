using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuOptionSelector : MonoBehaviour
{
    public int currentPosition;
    public List<MenuAsset> menuAssets;

    // Update is called once per frame
    void Update()
    {
        menuAssets[currentPosition].Selected();


        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            PreviousOption();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            NextOption();
        }
    }


    public void NextOption()
    {
        currentPosition++;
        if(currentPosition > menuAssets.Count)
        {
            currentPosition = 0;
        }
    }


    public void PreviousOption()
    {
        currentPosition--;
        if (currentPosition < 0)
        {
            currentPosition = menuAssets.Count;
        }
    }



}
