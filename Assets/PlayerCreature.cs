using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCreature : Creature, IItemTaker
{
    [SerializeField] private Inventory inventory;
    private void Initialize()
    {
        EventsBus.Subscribe<OnApproachingItemHolder>(OnApproachingItemHolder);
        EventsBus.Subscribe<OnLeavingItemHolder>(OnLeavingItemHolder);
        EventsBus.Subscribe<OnItemPickedUp>(PickFromItemHolder);
    }

    private void Deinitialize()
    {
        EventsBus.Unsubscribe<OnApproachingItemHolder>(OnApproachingItemHolder);
        EventsBus.Unsubscribe<OnLeavingItemHolder>(OnLeavingItemHolder);
        EventsBus.Unsubscribe<OnItemPickedUp>(PickFromItemHolder);
    }

    private void OnApproachingItemHolder(OnApproachingItemHolder data)
    {

    }

    private void OnLeavingItemHolder(OnLeavingItemHolder data)
    {

    }

    private void PickFromItemHolder(OnItemPickedUp data)
    {
        EventsBus.Publish(new OnShutDownWindows());
    }
}
