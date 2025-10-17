using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Upgrade")]
public class AbilityUpgradeData : ScriptableObject
{
    public ProjectileType type;
    public Sprite upgradeIcon;         // Upgrade ikonu
    public string description;         // Açýklama
    public int addDamage;
    public float fireRateMultiplier = 1f;
    public int addProjectile;
    public int piercing;
}
