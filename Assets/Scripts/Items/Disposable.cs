using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Disposable : Item
{
    [SerializeField] private Item[] suitableItems;
    [SerializeField] private ItemEffect[] effects;

    public bool IsItemSuitable(string id)
    {
        for (int i = 0; i < suitableItems.Length; i++)
        {
            if (id == suitableItems[i].Id)
                return true;
        }
        return false;
    }

    public bool ApplyAllEffectsOn(IEffectApplicable appObject)
    {
        if (effects.Length > 0)
        {
            foreach (var effect in effects)
            {
                appObject.OnApplying(effect.Modifier, effect.Duration, effect.Type);
            }
        }
        return false;
    }

}

