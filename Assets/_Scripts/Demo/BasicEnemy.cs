using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : Enemy
{
    protected override void SetUpVariables()
    {
        health = 2;
        armour = 5;
        gold = 500;
        base.SetUpVariables();
    }
}
