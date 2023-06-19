using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHolder : ItemContainerBase
{

    [SerializeField] private Sprite standardImage;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Collider2D collider;
    
    public List<Item> Items => items;

    private void Awake()
    {
        if (items == null) items = new List<Item>();
        EventsBus.Subscribe<OnItemPickedUp>(OnItemPickedUp);
        EventsBus.Subscribe<OnItemDropped>(OnItemDropped);
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
            Debug.Log("Player appeared");
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
}
