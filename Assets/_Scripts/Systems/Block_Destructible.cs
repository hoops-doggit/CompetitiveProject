using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_Destructible : Block {


    public override void Bumped()
    {
        base.Bumped();
    }
    public override void GotBumpedBy(Tags tag)
    {
        if(tag.tags == TileTags.Bullet)
        {
            
        }
        base.GotBumpedBy(tag);
    }
}
