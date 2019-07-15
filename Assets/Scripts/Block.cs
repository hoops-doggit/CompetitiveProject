using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Block : MonoBehaviour{

    [SerializeField]
    public int health { get; private set; }
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
            GoodBye();
        }        
    }

    public void GoodBye()
    {
        if (particles != null)
        {
            ParticleSystem p = Instantiate(particles);
            p.transform.position = particleSpawnPos.position;
        }

        Destroy(gameObject);
    }

}
