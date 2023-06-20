using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemContainerBase : MonoBehaviour
{
    [SerializeField] protected List<Item> items;

    protected void AddItem(Item item)
    {
        if (items.Count == 0 || !item.IsMultiply)
            items.Add(item);
        else
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].Id == item.Id)
                {
                    if (items[i].IsMultiply)
                    {
                        int countHas = items[i].Count;
                        int maxCountIs = items[i].MaxCount;

                        int countToGive = item.Count;

                        if (maxCountIs - countHas < countToGive)
                        {
                            items[i].Count = items[i].MaxCount;
                            item.Count = countToGive - (maxCountIs - countHas);
                        }
                    }
                }
            }
            if (items.Count > 0)
                items.Add(item);
        }
    }
}
