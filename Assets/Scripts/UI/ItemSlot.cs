using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

[Serializable]
public class ItemSlot : MonoBehaviour
{
    [SerializeField] private Button _slotButton;
    [SerializeField] private Image logoImage;
    [SerializeField] private Image smallImage;
    [SerializeField] private Text titleText;
    [SerializeField] private Text numberText;

    private Item currentItem;
    public Item CurrentItem => currentItem;
    private int currentSlotNumber;

    private void Awake()
    {
        _slotButton.onClick.RemoveAllListeners();
        _slotButton.onClick.AddListener(OnSlotClick);
    }

    private void OnDestroy()
    {
        _slotButton.onClick.RemoveAllListeners();
    }

    public void ApplyItemDataToSlot(Item item, int slotNum)
    {
        currentItem = item;
        titleText.text = item.Name;
        logoImage.sprite = item.Logo;
        if (item.IsMultiply)
            numberText.text = item.Count > 1 ? $"x{item.Count}" : string.Empty;
        else
            numberText.text = string.Empty;
        currentSlotNumber = slotNum;
    }

    public void RefreshSlotData()
    {
        currentItem = null;
        titleText.text = string.Empty;
        numberText.text = string.Empty;
        logoImage.sprite = null;
    }

    private void OnSlotClick()
    {
        if (logoImage.sprite != null)
        {
            DOTween.Kill(logoImage.transform);
            logoImage.transform.DOScale(1.2f, 0.24f).OnComplete(() => logoImage.transform.DOScale(1f, 0.24f));
            EventsBus.Publish(new OnItemSlotSelected() { ItemSlot = this, Item = currentItem, SlotNumber = currentSlotNumber });
        }
    }

    private void DrawState()
    {
        if (currentItem is IFixable fixable)
        {
            if (fixable.IsBroken())
            {
                logoImage.color = new Color(1f, 0.5f, 0.5f, 1f);
            }
            else
            {
                logoImage.color = Color.white;
            }
        }
    }
    //TODO: LongPress click for dragging
}
