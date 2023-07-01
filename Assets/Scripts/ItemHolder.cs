using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHolder : ItemContainerBase
{
    [SerializeField] private string holderName;
    [SerializeField] private Sprite standardImage;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Collider2D collider;
    [SerializeField] private bool isTakeByApproach;
    public List<Item> Items => items;
    public bool IsTakeByApproach => isTakeByApproach;
    public string HolderName => holderName;

    private void Awake()
    {
        if (items == null) items = new List<Item>();
        EventsBus.Subscribe<OnItemPickedUp>(OnItemPickedUp);
        EventsBus.Subscribe<OnItemDropped>(OnItemDropped);
        if (string.IsNullOrEmpty(holderName)) SetName();
    }

    private void OnDestroy()
    {
        EventsBus.Unsubscribe<OnItemPickedUp>(OnItemPickedUp);
        EventsBus.Unsubscribe<OnItemDropped>(OnItemDropped);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (IsTakeByApproach)
            {
                EventsBus.Publish(new OnItemPickedUp { ItemHolder = this });
                return;
            }
            EventsBus.Publish(new OnApproachingItemHolder { ItemHolder = this });
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            EventsBus.Publish(new OnLeavingItemHolder { ItemHolder = this });
        }
    }

    private void OnItemPickedUp(OnItemPickedUp data)
    {
        if (data.ItemHolder != this)
            return;
        // TODO: Reverse apply Item based on OnItemDropped
        Destroy(collider);
        gameObject.transform.DOScale(0, 0.3f).OnComplete(() =>
         Destroy(gameObject)
         );
    }

    private void OnItemDropped(OnItemDropped data)
    {
        if (data.ItemHolder != this)
            return;

        if (data.Item != null)
        {
            AddItem(data.Item);
        }
    }

    private void RemoveItem()
    {

    }

    public void SetName()
    {
        if (items.Count == 1)
            holderName = items[0].Name;
        else
            holderName = "Sack";
    }
}
