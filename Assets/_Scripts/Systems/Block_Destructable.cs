using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_Destructable : MonoBehaviour{

    public int health;
    [SerializeField] bool destructible;
    [SerializeField] bool regeneratable;
    [SerializeField] ParticleSystem particles;
    [SerializeField] Transform particleSpawnPos;
    [SerializeField] float timeTillRespawn = 12;


    public void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "bullet" && destructible)
        {
            health--;

            if (health < 1)
            {
                DestroyBlock();
                col.gameObject.GetComponent<Gun_Bullet>().DestroyBullet();
            }
        }
    }

    public void DestroyBlock( )
    {
        if (particles != null)
        {
            ParticleSystem p = Instantiate(particles);
            p.transform.position = particleSpawnPos.position;
        }

        foreach (Collider c in GetComponents<Collider>())
        {
            c.enabled = false;
        }

        if (regeneratable)
        {
            GetComponentInChildren<MeshRenderer>().enabled = false;
            StartCoroutine(BlockRegenerationTimer());
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator BlockRegenerationTimer()
    {
        yield return new WaitForSeconds(timeTillRespawn);
        RegenerateBlock();
    }

    public void RegenerateBlock()
    {
        foreach (Collider c in GetComponents<Collider>())
        {
            c.enabled = true;
        }
        GetComponentInChildren<MeshRenderer>().enabled = true;
    }

    

}
