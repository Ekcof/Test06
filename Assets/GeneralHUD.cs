using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

/// <summary>
/// The main HUD for playing
/// </summary>
public class GeneralHUD : MonoBehaviour
{
    [SerializeField] private Button takeButton;
    [SerializeField] private Button inventoryButton;
    [SerializeField] private Button fireButton;
    [SerializeField] private Inventory inventory;
    [SerializeField] private JoyStickController joyStickController;
    [SerializeField] private GameObject namePanel;
    [SerializeField] private TMP_Text nameText;

    private void Start()
    {
        EventsBus.Subscribe<OnApproachingItemHolder>(OnApproachingItemHolder);
        EventsBus.Subscribe<OnLeavingItemHolder>(OnLeavingItemHolder);
        EventsBus.Subscribe<OnOpenUIWindow>(OnOpenUIWindow);
        EventsBus.Subscribe<OnHideUIWindow>(OnHideUIWindow);
        takeButton.onClick.AddListener(OnTake);
        inventoryButton.onClick.AddListener(OpenInventory);
        fireButton.onClick.AddListener(OnFire);
    }

    private void OnDestroy()
    {
        takeButton.onClick.RemoveAllListeners();
        inventoryButton.onClick.RemoveAllListeners();
        fireButton.onClick.RemoveAllListeners();
        EventsBus.Unsubscribe<OnApproachingItemHolder>(OnApproachingItemHolder);
        EventsBus.Unsubscribe<OnLeavingItemHolder>(OnLeavingItemHolder);
        EventsBus.Unsubscribe<OnOpenUIWindow>(OnOpenUIWindow);
        EventsBus.Unsubscribe<OnHideUIWindow>(OnHideUIWindow);
        DOTween.Kill(namePanel.transform);
    }

    private void OpenInventory()
    {
        EventsBus.Publish(new OnOpenUIWindow { name = "InventoryUI" });
    }

    private void OnApproachingItemHolder(OnApproachingItemHolder data)
    {
        if (!namePanel.activeSelf)
        {
            DOTween.Kill(namePanel.transform);
            namePanel.SetActive(true);
            namePanel.transform.DOScale(1, 0.3f).OnComplete(() => OnShowName(data.ItemHolder));
        }
        nameText.text = data.ItemHolder.HolderName;
        takeButton.gameObject.SetActive(true);
    }

    private void OnShowName(ItemHolder holder)
    {
        if (holder.IsTakeByApproach)
        {
            namePanel.transform.DOScale(0, 0.3f).SetDelay(2f).OnComplete(OnCloseNamePanel);
        }
    }

    private void OnCloseNamePanel()
    {

        DOTween.Kill(namePanel.transform);
        namePanel?.SetActive(false);
    }

    private void OnLeavingItemHolder(OnLeavingItemHolder data)
    {
        if (!inventory.HasNearestHolder && !data.ItemHolder.IsTakeByApproach)
            namePanel.transform.DOScale(0, 0.3f).OnComplete(OnCloseNamePanel);
        takeButton.gameObject.SetActive(false);
    }

    private void OnOpenUIWindow(OnOpenUIWindow data)
    {
        takeButton.gameObject.SetActive(false);
        inventoryButton.gameObject.SetActive(false);
        joyStickController.gameObject.SetActive(false);
        fireButton.gameObject.SetActive(false);
    }

    private void OnHideUIWindow(OnHideUIWindow data)
    {
        if (data.EnableGeneralHUD)
        {
            inventoryButton.gameObject.SetActive(true);
            joyStickController.gameObject.SetActive(true);
            if (inventory.IsHoldersAround)
                takeButton.gameObject.SetActive(true);

            fireButton.gameObject.SetActive(true);
        }
    }

    private void OnTake()
    {
        takeButton.gameObject.SetActive(false);
        inventory.AddItemsFromHolder();
    }

    private void OnFire()
    {

    }
}
