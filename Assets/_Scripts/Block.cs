using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Block : MonoBehaviour{

    public int health;
    [SerializeField]
    private bool destructible;
    [SerializeField]
    private ParticleSystem particles;
    [SerializeField]
    private Transform particleSpawnPos;


    public virtual void Bumped() { }
    public virtual void GotBumpedBy(Tags tag) { }


    public void OnCollisionEnter(Collision col)
    {
        Bumped();
        //GotBumpedBy(col.gameObject.GetComponent<Tags>());

        if(col.gameObject.tag == "bullet" && destructible)
        {
            health--;
            if (health < 1)
            {
                GoodBye(col.gameObject);
            }
        }
    }

    public void GoodBye(GameObject collidedWith)
    {
        collidedWith.GetComponent<Collider>().enabled = false;
        if (particles != null)
        {
            ParticleSystem p = Instantiate(particles);
            p.transform.position = particleSpawnPos.position;
        }
        Destroy(gameObject);
    }

}
