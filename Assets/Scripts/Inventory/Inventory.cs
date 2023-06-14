using System;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private static GameObject prefab;

    private static float MaxWeight = 70f;
    private static Item helmet;
    private static Item armor;
    private static Weapon currentWeapon;

    private static Item[] backPackItems;

    public static Item[] BackPackItems => backPackItems;

    public static void AddItem(Item item)
    {

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
