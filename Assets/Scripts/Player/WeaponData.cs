using UnityEngine;

public enum ProjectileType { Default, Uzi, Rocket, Boomerang }

[CreateAssetMenu(menuName = "Game/WeaponData")]
public class WeaponData : ScriptableObject
{
    [Header("Info")]
    public ProjectileType type;
    public GameObject projectilePrefab;

    [Header("Stats")]
    public float fireRate = 0.5f;       // ate� s�resi (saniye)
    public int damage = 1;              // hasar
    public int projectileAmount = 1;    // ayn� anda ��kan mermi say�s�
    public int piercingAmount = 0;      // ka� objeden ge�ebilir
    public float damageMultiplier = 1f; // damage �arpan�
    public float specialInterval = 5f;  // rocket gibi �zel silahlar�n intervali
}
