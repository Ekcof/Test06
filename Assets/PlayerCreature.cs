using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCreature : Creature, IItemTaker
{
    private void Initialize()
    {
    }


    private void PickFromItemHolder(ItemHolder holder)
    {
        EventsBus.Publish<OnShutDownWindows>(new OnShutDownWindows());

    }
}
