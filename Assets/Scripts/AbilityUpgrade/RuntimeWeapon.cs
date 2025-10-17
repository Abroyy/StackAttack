using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Runtime için silah instance
[System.Serializable]
public class RuntimeWeapon
{
    public ProjectileType type;
    public GameObject projectilePrefab;
    public float fireRate;
    public int damage;
    public int projectileAmount;
    public int piercingAmount;
    public float damageMultiplier;
    public float specialInterval;

    public RuntimeWeapon(WeaponData template)
    {
        type = template.type;
        projectilePrefab = template.projectilePrefab;
        fireRate = template.fireRate;
        damage = template.damage;
        projectileAmount = template.projectileAmount;
        piercingAmount = template.piercingAmount;
        damageMultiplier = template.damageMultiplier;
        specialInterval = template.specialInterval;
    }
}
