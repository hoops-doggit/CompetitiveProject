using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Inheritance in object oriented programming works the same way as inheritance in real life.

// Lets say that:
// All wolves are animals
// All dogs are wolves
// All pomeranians are dogs

// So in this example a wolf receives every trait that an animal has. For example age, intelligence level, friendliness to humans value.
// Because wolves inherit the traits of animals, they receive all of the variables and funcitons that animals contain.
// And becaues a dog inherits from the wolf family, it receives the traits of that wolves have (which also contain all the traits of animals). 

// So he difference between a Labrador and a Pomeranian are just the values that their variables are set to.

// What this looks like:
//Using the abstract keyword ensures that this class can't itself be instantiated into the viewport but classes that derive from Enemy can
public abstract class Turret : MonoBehaviour
{
    public float fireRate;
    private float time;

    private void Update()
    {
        if(time >= fireRate)
        {
            if (Fire())
            {
                time -= fireRate;

            }
        } else
        {
            time += Time.deltaTime;
        }
    }

    public abstract bool Fire();

}