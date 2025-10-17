using UnityEngine;

public enum ProjectileType { Default, Uzi, Rocket, Boomerang }

[CreateAssetMenu(menuName = "Game/WeaponData")]
public class WeaponData : ScriptableObject
{
    [Header("Info")]
    public ProjectileType type;
    public GameObject projectilePrefab;

    [Header("Stats")]
    public float fireRate = 0.5f;       // ateþ süresi (saniye)
    public int damage = 1;              // hasar
    public int projectileAmount = 1;    // ayný anda çýkan mermi sayýsý
    public int piercingAmount = 0;      // kaç objeden geçebilir
    public float damageMultiplier = 1f; // damage çarpaný
    public float specialInterval = 5f;  // rocket gibi özel silahlarýn intervali
}
