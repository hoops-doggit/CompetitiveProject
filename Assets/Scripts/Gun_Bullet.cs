using UnityEngine;

public class Gun_Bullet : MonoBehaviour {

    float age = 0;
    public float maxAge = 800;

	
	// Update is called once per frame
	void FixedUpdate () {
        age++;
        if (age > maxAge)
        {
            Destroy(gameObject);
        }
	}
}
