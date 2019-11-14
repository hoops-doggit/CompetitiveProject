using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TutorialState : MonoBehaviour
{
    public bool requirementsMet;

    public abstract void CheckForRequirements();

    
}
