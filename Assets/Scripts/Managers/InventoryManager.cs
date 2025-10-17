using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("Inventory")]
    public List<ItemData> ownedItems = new();
    public List<ItemData> equippedItems = new();
    public Dictionary<string, int> blueprintCounts = new(); // key = itemName

    [Header("Weapons")]
    public List<WeaponData> allWeapons;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }
        DontDestroyOnLoad(gameObject);
        LoadInventory();
    }

    public void AddBlueprint(ItemData item, int amount)
    {
        if (item == null) return;
        string key = item.name;
        if (!blueprintCounts.ContainsKey(key)) blueprintCounts[key] = 0;
        blueprintCounts[key] += amount;

        GameEvents.InvokeBlueprintGained(item, amount);
        SaveInventory();
    }

    public void AddItem(ItemData item)
    {
        if (item == null) return;
        if (!ownedItems.Contains(item))
            ownedItems.Add(item);
        SaveInventory();
    }

    public void EquipItem(ItemData item)
    {
        if (item == null) return;
        if (!ownedItems.Contains(item) || equippedItems.Contains(item)) return;
        equippedItems.Add(item);
        ApplyItemStats(item);
        GameEvents.InvokeItemEquipped(item);
        SaveInventory();
    }

    public void UnequipItem(ItemData item)
    {
        if (item == null) return;
        if (!equippedItems.Contains(item)) return;
        equippedItems.Remove(item);
        RemoveItemStats(item);
        GameEvents.InvokeItemUnequipped(item);
        SaveInventory();
    }
    private void ApplyItemStats(ItemData item)
    {
        foreach (var weapon in allWeapons)
        {
            weapon.damage += item.bonusDamage;
            weapon.projectileAmount += item.bonusProjectile;
        }
    }

    private void RemoveItemStats(ItemData item)
    {
        foreach (var weapon in allWeapons)
        {
            weapon.damage -= item.bonusDamage;
            weapon.projectileAmount -= item.bonusProjectile;
        }
    }
    #region PlayerPrefs
    private void SaveInventory()
    {
        // Owned items (store asset names)
        PlayerPrefs.SetInt("OwnedCount", ownedItems.Count);
        for (int i = 0; i < ownedItems.Count; i++)
            PlayerPrefs.SetString("Owned_" + i, ownedItems[i].name);

        // Equipped items (store asset names)
        PlayerPrefs.SetInt("EquippedCount", equippedItems.Count);
        for (int i = 0; i < equippedItems.Count; i++)
            PlayerPrefs.SetString("Equipped_" + i, equippedItems[i].name);

        // Blueprints (key = asset name)
        PlayerPrefs.SetInt("BlueprintCount", blueprintCounts.Count);
        int j = 0;
        foreach (var kvp in blueprintCounts)
        {
            PlayerPrefs.SetString("BlueprintKey_" + j, kvp.Key);
            PlayerPrefs.SetInt("BlueprintValue_" + j, kvp.Value);
            j++;
        }

        PlayerPrefs.Save();
    }
    private void LoadInventory()
    {
        ownedItems.Clear();
        equippedItems.Clear();
        blueprintCounts.Clear();

        // Owned items
        int ownedCount = PlayerPrefs.GetInt("OwnedCount", 0);
        for (int i = 0; i < ownedCount; i++)
        {
            string assetName = PlayerPrefs.GetString("Owned_" + i, "");
            if (string.IsNullOrEmpty(assetName)) continue;
            ItemData item = Resources.Load<ItemData>("Items/" + assetName);
            if (item != null) ownedItems.Add(item);
        }

        // Equipped
        int equippedCount = PlayerPrefs.GetInt("EquippedCount", 0);
        for (int i = 0; i < equippedCount; i++)
        {
            string assetName = PlayerPrefs.GetString("Equipped_" + i, "");
            if (string.IsNullOrEmpty(assetName)) continue;
            ItemData item = Resources.Load<ItemData>("Items/" + assetName);
            if (item != null)
            {
                equippedItems.Add(item);
                ApplyItemStats(item);
            }
        }

        // Blueprints
        int bpCount = PlayerPrefs.GetInt("BlueprintCount", 0);
        for (int i = 0; i < bpCount; i++)
        {
            string key = PlayerPrefs.GetString("BlueprintKey_" + i, "");
            int value = PlayerPrefs.GetInt("BlueprintValue_" + i, 0);
            if (!string.IsNullOrEmpty(key))
                blueprintCounts[key] = value;
        }
    }
    #endregion
}
