using System;
using UnityEngine.Events;

public static class GameEvents
{
    public static Action<ProjectileType> OnWeaponUnlock;
    public static Action<ProjectileType, int, float, int, int> OnAbilityUpgrade;
    public static Action OnBlockDestroyed;
    public static Action OnGameOver;

    public static Action<ItemData, int> OnBlueprintGained;
    public static Action<ItemData> OnItemEquipped;
    public static Action<ItemData> OnItemUnequipped;
    public static Action<ItemData, int> OnLevelReward;

    public static Action OnBossDead;
    public static void UnlockWeapon(ProjectileType type)
    {
        OnWeaponUnlock?.Invoke(type);
    }

    public static void UpgradeAbility(ProjectileType type, int damage, float fireRateMultiplier, int extraProjectile, int piercingAmount)
    {
        OnAbilityUpgrade?.Invoke(type, damage, fireRateMultiplier, extraProjectile, piercingAmount);
    }

    public static void NotifyBlockDestroyed()
    {
        OnBlockDestroyed?.Invoke();
    }

    public static void InvokeGameOver() => OnGameOver?.Invoke();

    public static void InvokeBlueprintGained(ItemData item, int amount) => OnBlueprintGained?.Invoke(item, amount);
    public static void InvokeItemEquipped(ItemData item) => OnItemEquipped?.Invoke(item);
    public static void InvokeItemUnequipped(ItemData item) => OnItemUnequipped?.Invoke(item);
    public static void InvokeLevelReward(ItemData item, int blueprintAmount) => OnLevelReward?.Invoke(item, blueprintAmount);
    public static void InvokeBossDead() => OnBossDead?.Invoke();
}
