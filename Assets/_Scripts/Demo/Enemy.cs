using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health;
    public int goldReward;
    public float speed;
    public Transform target;

    public void Damaged(int damage)
    {
        health-=damage;

        if(health <= 0)
        {
            ImDead();
        }
    }

    public void ImDead()
    {
        Destroy(gameObject);
    }


}
