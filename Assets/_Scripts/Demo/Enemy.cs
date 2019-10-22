using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;
    public int armour;
    public int gold;

    private void Start()
    {
        SetUpVariables();
    }

    protected virtual void SetUpVariables()
    {

    }

    public virtual void ImDead()
    {
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("oaksdf;laks");
        if(other.gameObject.tag == "bullet")
        {
            GotHit();
        }        
    }

    public virtual void GotHit()
    {
        health--;
        CheckIfDead();
    }

    private void CheckIfDead()
    {
        if (health <= 0)
        {
            ImDead();
        }
    }

    public static void Dance()
    {
        Debug.Log("you know its time to dance");
    }
}
