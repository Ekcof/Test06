using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private static GameObject prefab;

    private static float MaxWeight = 70f;
    private static Item helmet;
    private static Item armor;
    private static Weapon currentWeapon;

    private static Item[] backPackItems;
    private ItemHolder nearestHolder;
    private int holdersAround;


    public static Item[] BackPackItems => backPackItems;
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
        ++holdersAround;
        nearestHolder = data.ItemHolder;
    }

    private void OnLeavingItemHolder(OnLeavingItemHolder data)
    {
        --holdersAround;
        if (holdersAround == 0)
            nearestHolder = null;
    }

    public void AddItemsFromHolder()
    {
        --holdersAround;
        List<Item> itemList = new List<Item>(backPackItems);
        itemList.AddRange(nearestHolder.Items);
        backPackItems = itemList.ToArray();
    }

    public static void RemoveItem(int number)
    {
        backPackItems[number] = null;
    }

    public static void DropItem(int num, Vector3 position)
    {
        GameObject newHolderGO = Instantiate(prefab, position, Quaternion.identity);
        var newHolder = newHolderGO.GetComponent<ItemHolder>();
        EventsBus.Publish(new OnItemDropped() { Item = BackPackItems[num], ItemHolder = newHolder });

    }

    public static void RefreshBackPack()
    {
        backPackItems = Array.FindAll(backPackItems, item => item != null);
    }
}
