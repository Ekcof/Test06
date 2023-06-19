using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Responsible for storing, collecting and leaving items of player
/// </summary>
public class Inventory : ItemContainerBase
{
    [SerializeField] private GameObject prefab;

    private float MaxWeight = 70f;
    private Item helmet;
    private Item armor;
    private Weapon currentWeapon;

    private ItemHolder nearestHolder;
    private List<ItemHolder> holdersAround = new();

    public bool IsHoldersAround => holdersAround.Count > 0;
    public List<Item> BackPackItems => items;
    private void Awake()
    {
        EventsBus.Subscribe<OnApproachingItemHolder>(OnApproachingItemHolder);
        EventsBus.Subscribe<OnLeavingItemHolder>(OnLeavingItemHolder);
    }

    private void OnDestroy()
    {
        EventsBus.Unsubscribe<OnApproachingItemHolder>(OnApproachingItemHolder);
        EventsBus.Unsubscribe<OnLeavingItemHolder>(OnLeavingItemHolder);
    }

    /// <summary>
    /// Event for approaching the pickable distance of an itemholder 
    /// </summary>
    /// <param name="data"></param>
    private void OnApproachingItemHolder(OnApproachingItemHolder data)
    {
        holdersAround.Add(data.ItemHolder);
        nearestHolder = data.ItemHolder;
    }

    /// <summary>
    /// Event for leaving the pickable distance of an itemholder 
    /// </summary>
    /// <param name="data"></param>
    private void OnLeavingItemHolder(OnLeavingItemHolder data)
    {
        holdersAround.Remove(data.ItemHolder);
        if (holdersAround.Count == 0)
            nearestHolder = null;
        else if (holdersAround.Count > 0)
            nearestHolder = holdersAround[0];
    }

    /// <summary>
    /// Add items from a nearest itemholder to the inventory 
    /// </summary>
    public void AddItemsFromHolder()
    {
        holdersAround.Remove(nearestHolder);
        if (nearestHolder.Items != null)
        {
            if (items == null)
                items = new List<Item>();
            foreach(Item item in nearestHolder.Items)
            {
                items.Add(item);
            }
            EventsBus.Publish(new OnItemPickedUp { ItemHolder = nearestHolder });
            if (holdersAround.Count == 0)
                nearestHolder = null;
            else if (holdersAround.Count > 0)
                nearestHolder = holdersAround[0];
        }
        else
        {
            EventsBus.Publish(new OnItemPickedUp { ItemHolder = nearestHolder });
        }
    }

    /// <summary>
    /// Remove item from the inventory
    /// </summary>
    /// <param name="number"></param>
    public void RemoveItem(int number)
    {
        items.RemoveAt(number);
    }

    /// <summary>
    /// Drop an item from inventory on the ground
    /// </summary>
    /// <param name="num">number in a item list</param>
    /// <param name="position">position for a item holder</param>
    public void DropItem(int num, Vector3 position)
    {
        //check if there is no item holder under the player
        if (nearestHolder == null)
        {
            GameObject newHolderGO = Instantiate(prefab, position, Quaternion.identity);
            nearestHolder = newHolderGO.GetComponent<ItemHolder>();
        }
        EventsBus.Publish(new OnItemDropped() { Item = BackPackItems[num], ItemHolder = nearestHolder });
        items.RemoveAt(num);
    }
}
