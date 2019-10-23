using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGunT : Turret
{
    public bool inRange;

    public override bool Fire()
    {
        if (inRange)
            return true;
        else
        {
            return false;
        }
    }
}
