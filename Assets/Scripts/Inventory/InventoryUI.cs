using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    [Header("Arcana Slots UI")]
    public Image[] arcanaSlots;

    [Header("UI References")]
    public Image itemIcon;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDescText;
    public Button equipButton;
    public Button levelUpButton;
    public TextMeshProUGUI levelUpCostText;
    public GameObject itemDetailPanel;

    [Header("Item Lists")]
    public Transform ownedItemsParent;
    public Transform blueprintsParent;
    public GameObject itemSlotPrefab;
    public GameObject blueprintSlotPrefab;

    private ItemData selectedItem;

    private void OnEnable()
    {
        UpdateUI();
        GameEvents.OnBlueprintGained += OnBlueprintGained;
        GameEvents.OnItemEquipped += OnItemEquipped;
        GameEvents.OnItemUnequipped += OnItemUnequipped;
    }

    private void OnDisable()
    {
        GameEvents.OnBlueprintGained -= OnBlueprintGained;
        GameEvents.OnItemEquipped -= OnItemEquipped;
        GameEvents.OnItemUnequipped -= OnItemUnequipped;
    }

    private void OnItemEquipped(ItemData item)
    {
        UpdateEquipButton();
    }

    private void OnItemUnequipped(ItemData item)
    {
        UpdateEquipButton();
    }
    private void OnBlueprintGained(ItemData item, int amount)
    {
        UpdateUI();
    }
    private void PopulateInventoryLists()
    {
        // Temizle
        foreach (Transform t in ownedItemsParent) Destroy(t.gameObject);
        foreach (Transform t in blueprintsParent) Destroy(t.gameObject);

        // Item listesi oluþtur
        foreach (var item in InventoryManager.Instance.ownedItems)
        {
            GameObject slot = Instantiate(itemSlotPrefab, ownedItemsParent);
            slot.transform.Find("Icon").GetComponent<Image>().sprite = item.icon;
            //slot.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = item.itemName;
            slot.GetComponent<Button>().onClick.AddListener(() => SelectItem(item, true));
        }

        // Blueprint listesi oluþtur
        foreach (var kvp in InventoryManager.Instance.blueprintCounts)
        {
            // Blueprint için item verisini bul
            ItemData itemData = Resources.Load<ItemData>("Items/" + kvp.Key);
            if (itemData == null) continue;

            GameObject bpSlot = Instantiate(blueprintSlotPrefab, blueprintsParent);
            bpSlot.transform.Find("Icon").GetComponent<Image>().sprite = itemData.icon;
            bpSlot.transform.Find("Count").GetComponent<TextMeshProUGUI>().text = kvp.Value.ToString();
        }
    }
    public void SelectItem(ItemData item, bool openPanel = true)
    {
        selectedItem = item;
        itemIcon.sprite = item.icon;
        itemNameText.text = item.itemName;
        itemDescText.text = item.description;

        if (openPanel && itemDetailPanel != null)
            itemDetailPanel.SetActive(true);

        UpdateEquipButton();
        UpdateLevelUpButton();
    }

    private void UpdateUI()
    {
        PopulateInventoryLists();
        if (InventoryManager.Instance.ownedItems.Count > 0)
            SelectItem(InventoryManager.Instance.ownedItems[0], false);
    }
    private void UpdateEquipButton()
    {
        if (selectedItem == null) return;

        equipButton.onClick.RemoveAllListeners();

        if (InventoryManager.Instance.equippedItems.Contains(selectedItem))
        {
            equipButton.GetComponentInChildren<TextMeshProUGUI>().text = "Unequip";
            equipButton.onClick.AddListener(() =>
            {
                InventoryManager.Instance.UnequipItem(selectedItem);
                RemoveFromArcanaUI(selectedItem);
                UpdateEquipButton();
            });
        }
        else
        {
            equipButton.GetComponentInChildren<TextMeshProUGUI>().text = "Equip";
            equipButton.onClick.AddListener(() =>
            {
                InventoryManager.Instance.EquipItem(selectedItem);
                AddToArcanaUI(selectedItem);
                UpdateEquipButton();
            });
        }
    }
    private void AddToArcanaUI(ItemData item)
    {
        if (item == null || item.icon == null) return;

        for (int i = 0; i < arcanaSlots.Length; i++)
        {
            if (arcanaSlots[i].sprite == null)
            {
                arcanaSlots[i].sprite = item.icon;
                arcanaSlots[i].color = Color.white; // alpha=1
                arcanaSlots[i].gameObject.SetActive(true); // görünür yap
                break;
            }
        }
    }
    private void RemoveFromArcanaUI(ItemData item)
    {
        for (int i = 0; i < arcanaSlots.Length; i++)
        {
            if (arcanaSlots[i].sprite == item.icon)
            {
                arcanaSlots[i].sprite = null;
                arcanaSlots[i].color = Color.clear;
                arcanaSlots[i].gameObject.SetActive(false);
                break;
            }
        }
    }
    private void UpdateLevelUpButton()
    {
        if (selectedItem == null) return;

        int playerGold = CurrencyManager.Instance.GetCoin();
        int blueprintCount = InventoryManager.Instance.blueprintCounts.ContainsKey(selectedItem.name)
            ? InventoryManager.Instance.blueprintCounts[selectedItem.name]
            : 0;

        levelUpCostText.text = $"Gold: {selectedItem.costGold} | BP: {selectedItem.costBlueprint}";
        levelUpButton.interactable = playerGold >= selectedItem.costGold && blueprintCount >= selectedItem.costBlueprint;

        levelUpButton.onClick.RemoveAllListeners();
        levelUpButton.onClick.AddListener(() =>
        {
            if (playerGold >= selectedItem.costGold && blueprintCount >= selectedItem.costBlueprint)
            {
                // Gold ve blueprint harcama
                CurrencyManager.Instance.RemoveCoin(selectedItem.costGold);
                InventoryManager.Instance.AddBlueprint(selectedItem, -selectedItem.costBlueprint);

                // Item stats level up
                selectedItem.bonusDamage += 1;
                selectedItem.bonusProjectile += 1;

                if (InventoryManager.Instance.equippedItems.Contains(selectedItem))
                {
                    InventoryManager.Instance.UnequipItem(selectedItem);
                    InventoryManager.Instance.EquipItem(selectedItem);
                }

                UpdateLevelUpButton();
                PopulateInventoryLists();
            }
        });
    }
}
