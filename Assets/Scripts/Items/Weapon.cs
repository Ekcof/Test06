using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Weapon : Item, IFixable
{
    [SerializeField] bool IsMelee;
    [SerializeField] private int maxAmmo;
    [SerializeField] private int basicDamageMin;
    [SerializeField] private int basicDamageMax;
    [SerializeField] private float basicRange;
    [SerializeField] private float basicAccuracy;
    [SerializeField] private float basicDispersion;

    private int currentDamageMin;
    private int currentDamageMax;
    private float currentRange;
    private float currentAccuracy;
    private float currentDispersion;

    private readonly List<ItemUpgrade> itemUpgrades;
    private int currentAmmoCount;
    private float currentState = 1f;

    private Item magazine;
    private Ammo currentAmmo;

    public void SetDefaultMetrics()
    {
        currentDamageMin = basicDamageMin;
        currentDamageMax = basicDamageMax;
        currentRange = basicRange;
        currentAccuracy = basicAccuracy;
        currentDispersion = basicDispersion;
    }

    public void ChangeAmmo(int ammo)
    {
        currentAmmoCount = Math.Clamp(ammo, 0, maxAmmo);
    }

    public bool IsEmpty() => (currentAmmoCount == 0);

    public void ChangeState(float change)
    {
        currentState = Mathf.Clamp(change, 0, 1f);
    }

    public bool IsBroken() => (currentState <= 0);

    public bool TryUpgradeItem(ItemUpgrade upgrade)
    {
        if (upgrade.IsItemSuitable(Id))
        {
            for (int i = 0; i < itemUpgrades.Count; i++)
            {
                if (upgrade.Id == itemUpgrades[i].Id)
                    return false;
            }
            itemUpgrades.Add(upgrade);
            ModifyStats(upgrade, true);
            return true;
        }
        else
            return false;
    }

    public bool TryRemoveItem(ItemUpgrade upgrade)
    {
        for (int i = 0; i < itemUpgrades.Count; i++)
        {
            if (upgrade.Id == itemUpgrades[i].Id)
            {
                ModifyStats(upgrade, false);
                itemUpgrades.RemoveAt(i);
            }
        }
        return false;
    }

    public ItemUpgrade[] RemoveAllUpgrades()
    {
        if (itemUpgrades.Count > 0)
        {
            ItemUpgrade[] upgradeIds = itemUpgrades.ToArray();
            itemUpgrades.Clear();
            SetDefaultMetrics();
            return upgradeIds;
        }
        else { return null; }
    }

    private void ModifyStats(ItemUpgrade upgrade, bool applyModifiers)
    {
        int damageMinBonus = applyModifiers ? upgrade.DamageMinBonus : -upgrade.DamageMinBonus;
        int damageMaxBonus = applyModifiers ? upgrade.DamageMaxBonus : -upgrade.DamageMaxBonus;
        float rangeBonus = applyModifiers ? upgrade.RangeBonus : -upgrade.RangeBonus;
        float accuracyBonus = applyModifiers ? upgrade.AccuracyBonus : -upgrade.AccuracyBonus;
        float dispersionBonus = applyModifiers ? upgrade.DispersionBonus : -upgrade.DispersionBonus;

        currentDamageMin = Mathf.Clamp(currentDamageMin + damageMinBonus, 0, int.MaxValue);
        currentDamageMax = Mathf.Clamp(currentDamageMax + damageMaxBonus, 0, int.MaxValue);

        if (currentDamageMin > currentDamageMax)
        {
            currentDamageMin = currentDamageMax;
        }
        else if (currentDamageMax < currentDamageMin)
        {
            currentDamageMax = currentDamageMin;
        }

        currentRange = Mathf.Clamp(currentRange + rangeBonus, 0, float.MaxValue);
        currentAccuracy = Mathf.Clamp(currentAccuracy + accuracyBonus, 0, float.MaxValue);
        currentDispersion = Mathf.Clamp(currentDispersion + dispersionBonus, 0, float.MaxValue);
    }
}