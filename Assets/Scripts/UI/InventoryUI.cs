using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class InventoryUI : MonoBehaviour, IOpenUI
{
    [SerializeField] private GameObject playerGO;
    [SerializeField] private GameObject prefab;
    [SerializeField] private Text descriptionText;
    [SerializeField] private Button useButton;
    [SerializeField] private Button dropButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private GameObject row;
    [SerializeField] private List<ItemSlot> slots;
    [SerializeField] private List<InventoryRow>[] rows;
    [SerializeField] private GameObject content;
    [SerializeField] private Inventory inventory;

    private List<ItemSlot> currentSlots;

    private int currentItemNum;

    private void Awake()
    {
        EventsBus.Subscribe<OnItemSlotSelected>(OnItemSlotSelected);
        EventsBus.Subscribe<OnOpenUIWindow>(OnOpenUIWindow);
        useButton.onClick.AddListener(OnUseButtonClick);
        dropButton.onClick.AddListener(OnDropButtonClick);
        closeButton.onClick.AddListener(Hide);

        foreach (var slot in slots)
        {
            slot.gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        EventsBus.Unsubscribe<OnItemSlotSelected>(OnItemSlotSelected);
        EventsBus.Unsubscribe<OnOpenUIWindow>(OnOpenUIWindow);
    }

    private void OnOpenUIWindow(OnOpenUIWindow data)
    {
        Debug.Log("Hey, Open Inventory UI!");
        if (data.name == "InventoryUI")
            Open();
    }

    private void OnUseButtonClick()
    {
        // TODO: Make an action for disposable items
    }

    private void OnDropButtonClick()
    {
        if (inventory.BackPackItems.Count > 0)
        {
            inventory.DropItem(currentItemNum, playerGO.transform.position);
            OnItemDeselect();
            RecalculateSlots();
        }
    }

    private void Open()
    {
        transform.DOScale(1, 0.3f).OnComplete(() => DOTween.Kill(transform));
        OnItemDeselect();
        RecalculateSlots();
    }

    private void Hide()
    {
        EventsBus.Publish(new OnHideUIWindow { EnableGeneralHUD = true });
        transform.DOScale(0, 0.3f).OnComplete(() => DOTween.Kill(transform));

        if (currentSlots != null)
        {
            foreach (var slot in currentSlots)
            {
                slot.RefreshSlotData();
                slot.gameObject.SetActive(false);
            }
            currentSlots.Clear();
        }
    }

    private void OnItemSlotSelected(OnItemSlotSelected data)
    {
        descriptionText.text = data.Item.Description;
        if (data.Item != null)
        {
            if (data.Item.Type == ItemType.disposable)
                useButton.gameObject.SetActive(true);
            dropButton.gameObject.SetActive(true);
        }
        currentItemNum = data.SlotNumber;
    }

    private void OnItemDeselect()
    {
        descriptionText.text = string.Empty;
        useButton.gameObject.SetActive(false);
        dropButton.gameObject.SetActive(false);
    }

    private void RecalculateSlots()
    {
        if (inventory.BackPackItems != null)
        {
            if (currentSlots != null)
            {
                if (currentSlots.Count > 0)
                {
                    foreach (var currentSlot in currentSlots)
                    {
                        currentSlot.RefreshSlotData();
                    }
                }
            }

            int length = inventory.BackPackItems.Count;
            Debug.Log($"____Length is {length}");
            while (slots.Count < length)
            {
                AddSlotRow(length);
            }

            if (currentSlots != null)
                currentSlots.Clear();
            else
                currentSlots = new List<ItemSlot>();

            if (length > 0)
            {
                for (int i = 0; i < length; i++)
                {
                    currentSlots.Add(slots[i]);
                    currentSlots[i].gameObject.SetActive(true);
                    currentSlots[i].ApplyItemDataToSlot(inventory.BackPackItems[i], i);
                }
            }
        }
        //TODO : remove useless new rows!
    }

    private void AddSlotRow(int length)
    {
        GameObject newRow = Instantiate(row, content.transform);
        newRow.transform.position = new Vector2(0, 120f - 160f * (3 + rows.Length));
        InventoryRow inventoryRow = GetComponent<InventoryRow>();

        for (int i = 0; i < inventoryRow.slots.Length; i++)
        {
            if (slots.Count < length)
            {
                inventoryRow.slots[i].gameObject.SetActive(true);
                slots.Add(inventoryRow.slots[i]);
            }
        }
    }
}
