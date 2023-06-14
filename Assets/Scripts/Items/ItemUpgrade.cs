using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUpgrade : Item
{
    [SerializeField] private Item[] suitableItems;
    public int DamageMinBonus;
    public int DamageMaxBonus;
    public float RangeBonus;
    public float DispersionBonus;
    public float AccuracyBonus;

    public int ArmorBonus;
    public float SpeedBonus;

    public bool IsItemSuitable(string id)
    {
        for (int i = 0; i < suitableItems.Length; i++)
        {
            if (id == suitableItems[i].Id)
                return true;
        }
        return false;
    }
}
