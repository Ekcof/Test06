using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHolder : MonoBehaviour, IItemStorage
{
    [SerializeField] private Sprite standardImage;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private List<Item> items;

    private void Awake()
    {
        EventsBus.Subscribe<OnItemPickedUp>(OnItemPickedUp);
        EventsBus.Subscribe<OnItemDropped>(OnItemDropped);
    }

    private void OnDestroy()
    {
        EventsBus.Unsubscribe<OnItemPickedUp>(OnItemPickedUp);
        EventsBus.Unsubscribe<OnItemDropped>(OnItemDropped);
    }

    private void OnItemPickedUp(OnItemPickedUp data)
    {
        if (data.ItemHolder != this)
            return;
        // TODO: Reverse apply Item based on OnItemDropped
    }

    private void OnItemDropped(OnItemDropped data)
    {
        if (data.ItemHolder != this)
            return;

        if (data.Item != null)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].Id == data.Item.Id)
                {
                    if (!items[i].IsMultiply)
                    {
                        items.Add(data.Item);
                        return;
                    }
                    else
                    {
                        int countHas = items[i].Count;
                        int maxCountIs = items[i].MaxCount;

                        int countToGive = data.Item.Count;

                        if (maxCountIs - countHas < countToGive)
                        {
                            items[i].Count = items[i].MaxCount;
                            data.Item.Count = countToGive - (maxCountIs - countHas);
                            items.Add(data.Item);
                        }
                        else
                        {

                        }
                    }
                }
                else
                {
                    items.Add(data.Item);
                    return;
                }
            }
        }
    }

    private void AddItem()
    {

    }

    private void RemoveItem()
    {

    }
}
