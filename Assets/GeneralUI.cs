using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GeneralUI : MonoBehaviour
{
    [SerializeField] private Button takeButton;
    [SerializeField] private Button inventoryButton;
    [SerializeField] private Inventory inventory;

    private void Start()
    {
        EventsBus.Subscribe<OnApproachingItemHolder>(OnApproachingItemHolder);
        EventsBus.Subscribe<OnLeavingItemHolder>(OnLeavingItemHolder);
        takeButton.onClick.AddListener(OnTake);
        inventoryButton.onClick.AddListener(OpenInventory);
    }

    private void OnDestroy()
    {
        takeButton.onClick.RemoveAllListeners();
        inventoryButton.onClick.RemoveAllListeners();
    }

    private void OpenInventory()
    {
        EventsBus.Unsubscribe<OnApproachingItemHolder>(OnApproachingItemHolder);
    }

    private void OnApproachingItemHolder(OnApproachingItemHolder data)
    {
        takeButton.gameObject.SetActive(true);
    }

    private void OnLeavingItemHolder(OnLeavingItemHolder data)
    {

    }

    private void OnTake()
    {
        takeButton.gameObject.SetActive(false);
        inventory.AddItemsFromHolder();
    }
}
