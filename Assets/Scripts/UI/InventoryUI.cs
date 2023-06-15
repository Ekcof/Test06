using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class InventoryUI : MonoBehaviour, IOpenUI
{
    [SerializeField] private GameObject playerGO;
    [SerializeField] private GameObject prefab;
    [SerializeField] private static Text descriptionText;
    [SerializeField] private Button useButton;
    [SerializeField] private Button dropButton;
    [SerializeField] private GameObject row;
    [SerializeField] private List<ItemSlot> slots;
    [SerializeField] private List<InventoryRow>[] rows;
    [SerializeField] private GameObject content;

    private List<ItemSlot> currentSlots;

    private int currentItemNum;

    private void Awake()
    {
        EventsBus.Subscribe<OnItemSlotSelected>(OnItemSlotSelected);
        useButton.onClick.AddListener(OnUseButtonClick);
        dropButton.onClick.AddListener(OnDropButtonClick);

        foreach (var slot in slots)
        {
            slot.gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        EventsBus.Unsubscribe<OnItemSlotSelected>(OnItemSlotSelected);
    }

    private void OnUseButtonClick()
    {
        // TODO: Make a dropped item gameobject under the player
    }

    private void OnDropButtonClick()
    {
        if (Inventory.BackPackItems.Length > 0)
        {
            Inventory.DropItem(currentItemNum, playerGO.transform.position);
            OnItemDeselect();
            RecalculateSlots();
        }
    }

    private void Open()
    {
        gameObject.SetActive(true);
        OnItemDeselect();
        RecalculateSlots();
    }

    private void Hide()
    {
        Inventory.RefreshBackPack();
        gameObject.SetActive(false);
    }

    private void OnItemSlotSelected(OnItemSlotSelected data)
    {
        descriptionText.text = data.Item.Description;
        if (data.Item.Type == ItemType.disposable)
            useButton.gameObject.SetActive(true);
        dropButton.gameObject.SetActive(true);
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
        int length = Inventory.BackPackItems.Length;
        while (slots.Count < length)
        {
            AddSlotRow(length);
        }

        for (int i = 0; i < length; i++)
        {
            slots[i].gameObject.SetActive(true);
            slots[i].ApplyItemDataToSlot(Inventory.BackPackItems[i], i);
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
