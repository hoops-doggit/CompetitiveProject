using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[System.Flags]
public enum TileTags
{
    Idle = 0x01,
    Moving = 0x02,
    Deccelerating = 0x04,
    Dashing = 0x08,
    SwingAttacking = 0x10,
    Stunned = 0x20,
    Weapon = 0x40,
    Exit = 0x80,
    Consumable = 0x100,
    Wearable = 0x200,
    Money = 0x400,
    Dateable = 0x800,
    Dirt = 0x1000,
    Water = 0x2000,
    Plant = 0x4000,
    Flammable = 0x8000,
    Merchant = 0x10000
}


public class Tags : MonoBehaviour {   

    [SerializeField]
    [EnumFlags]
    public TileTags tags = 0;

}
