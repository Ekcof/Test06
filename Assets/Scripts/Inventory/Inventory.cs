using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Responsible for storing, collecting and leaving items of player
/// </summary>
public class Inventory : MonoBehaviour
{
    [SerializeField] private static GameObject prefab;

    private float MaxWeight = 70f;
    private Item helmet;
    private Item armor;
    private Weapon currentWeapon;

    private Item[] backPackItems;
    private ItemHolder nearestHolder;
    private List<ItemHolder> holdersAround = new();

    public bool IsHoldersAround => holdersAround.Count > 0;
    public Item[] BackPackItems => backPackItems;
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

    private void OnApproachingItemHolder(OnApproachingItemHolder data)
    {
        holdersAround.Add(data.ItemHolder);
        nearestHolder = data.ItemHolder;
    }

    private void OnLeavingItemHolder(OnLeavingItemHolder data)
    {
        holdersAround.Remove(data.ItemHolder);
        if (holdersAround.Count == 0)
            nearestHolder = null;
        else if (holdersAround.Count > 0)
            nearestHolder = holdersAround[0];
    }

    public void AddItemsFromHolder()
    {
        holdersAround.Remove(nearestHolder);
        if (nearestHolder.Items != null)
        {
            List<Item> itemList = backPackItems != null ? new List<Item>(backPackItems) : new List<Item>();
            itemList.AddRange(nearestHolder.Items);
            backPackItems = itemList.ToArray();
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

    public void RemoveItem(int number)
    {
        backPackItems[number] = null;
    }

    public void DropItem(int num, Vector3 position)
    {
        GameObject newHolderGO = Instantiate(prefab, position, Quaternion.identity);
        var newHolder = newHolderGO.GetComponent<ItemHolder>();
        EventsBus.Publish(new OnItemDropped() { Item = BackPackItems[num], ItemHolder = newHolder });

    }

    public void RefreshBackPack()
    {
        if (backPackItems != null)
            backPackItems = Array.FindAll(backPackItems, item => item != null);
    }
}
