using System;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Item Data", menuName = "Assets/Item Data")]
public class ItemData : ScriptableObject
{
    [SerializeField] private Item[] items;
    [SerializeField] private Weapon[] weapons;
    [SerializeField] private Ammo[] ammo;

    public Item GetItemByName(string name)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (name == items[i].Name)
                return items[i];
        }
        return null;
    }

    public Item GetItemByID(string id)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (id == items[i].Id)
                return items[i];
        }
        return null;
    }

}

[Serializable]
public class Item : IEffectApplicable
{
    public string Id;
    public ItemType Type;
    public string Name;
    public string Description;
    public Sprite Logo;
    public Sprite BigImage;
    public bool IsMultiply;
    public int Count;
    public int MaxCount;
    public float Weight;
    public int BaseCost;

    public virtual void OnApplying(float modificator, float duration, DisposableFXType type) { }

    public virtual bool IsItSuitable(string Id)
    {
        return false;
    }
}

public enum ItemType
{
    weapon,
    ammo,
    upgrade,
    clothes,
    disposable,
    valuable,
    simple
}