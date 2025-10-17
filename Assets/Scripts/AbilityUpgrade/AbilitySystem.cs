using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class AbilitySystem : MonoBehaviour
{
    public PlayerShooting playerShooting;
    public AbilityUI abilityUI;

    [Header("Unlock Icons")]
    public Sprite rocketUnlockIcon;
    public Sprite uziUnlockIcon;
    public Sprite boomerangUnlockIcon;

    [Header("Upgrade Pool")]
    public List<AbilityUpgradeData> upgradePool;

    public void ShowChoices()
    {

        List<AbilityUpgradeData> available = new List<AbilityUpgradeData>();

        // Default her zaman upgrade seçeneði eklenebilir
        available.AddRange(upgradePool.Where(u => u.type == ProjectileType.Default));

        // Her silah için ayrý kontrol
        AddWeaponChoices(ProjectileType.Rocket, ref available, playerShooting.isUnlockRocket);
        AddWeaponChoices(ProjectileType.Uzi, ref available, playerShooting.isUnlockUzi);
        AddWeaponChoices(ProjectileType.Boomerang, ref available, playerShooting.isUnlockBoomerang);

        List<AbilityUpgradeData> randomChoices = available.OrderBy(x => Random.value).Take(3).ToList();
        abilityUI.Show(randomChoices);
    }
    private void AddWeaponChoices(ProjectileType type, ref List<AbilityUpgradeData> available, bool isUnlocked)
    {
        if (isUnlocked)
        {
            available.AddRange(upgradePool.Where(u => u.type == type));
        }
        else
        {
            AbilityUpgradeData data = ScriptableObject.CreateInstance<AbilityUpgradeData>();
            data.type = type;
            data.description = $"Unlock {type}!";
            data.addDamage = 0;
            data.fireRateMultiplier = 1f;
            data.addProjectile = 0;
            data.piercing = 0;
            data.upgradeIcon = GetUnlockIcon(type);

            available.Add(data);
        }
    }
    private Sprite GetUnlockIcon(ProjectileType type)
    {
        switch (type)
        {
            case ProjectileType.Rocket: return rocketUnlockIcon;
            case ProjectileType.Uzi: return uziUnlockIcon;
            case ProjectileType.Boomerang: return boomerangUnlockIcon;
            default: return null;
        }
    }
}
