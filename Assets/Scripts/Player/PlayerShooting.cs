using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;

public class PlayerShooting : MonoBehaviour
{
    [Header("Weapons")]
    public List<WeaponData> allWeapons;
    public Transform firePoint;

    [Header("Unlock States")]
    public bool isUnlockDefault = true;
    public bool isUnlockUzi = false;
    public bool isUnlockRocket = false;
    public bool isUnlockBoomerang = false;
    public bool canAttack = true;

    [Header("Frenzy UI")]
    public GameObject frenzyIndicator; 
    private bool isFrenzyActive = false;

    private List<RuntimeWeapon> runtimeWeapons = new List<RuntimeWeapon>();
    private Dictionary<RuntimeWeapon, float> frenzyBackupFireRates = new Dictionary<RuntimeWeapon, float>();
    private Dictionary<ProjectileType, float> fireTimers = new();


    private void OnEnable()
    {
        GameEvents.OnWeaponUnlock += UnlockWeapon;
        GameEvents.OnAbilityUpgrade += ApplyUpgrade;
    }

    private void OnDisable()
    {
        GameEvents.OnWeaponUnlock -= UnlockWeapon;
        GameEvents.OnAbilityUpgrade -= ApplyUpgrade;
    }
    // Upgrades 
    public void ApplyUpgrade(ProjectileType type, int damage, float fireRateMultiplier, int extraProjectile, int piercingAmount)
    {
        var weapon = runtimeWeapons.Find(w => w.type == type);
        if (weapon == null) return;

        weapon.damage += damage;
        weapon.fireRate += fireRateMultiplier;
        weapon.projectileAmount += extraProjectile;
        weapon.piercingAmount += piercingAmount;
    }

    private void UnlockWeapon(ProjectileType type)
    {
        switch (type)
        {
            case ProjectileType.Rocket: isUnlockRocket = true; break;
            case ProjectileType.Uzi: isUnlockUzi = true; break;
            case ProjectileType.Boomerang: isUnlockBoomerang = true; break;
        }
    }
    private void Start()
    {
        foreach (var w in allWeapons)
        {
            runtimeWeapons.Add(new RuntimeWeapon(w));
            fireTimers[w.type] = 0f;
        }

        if (frenzyIndicator != null)
            frenzyIndicator.SetActive(false);
    }

    private void Update()
    {
        foreach (var weapon in runtimeWeapons)
        {
            if (!IsWeaponUnlocked(weapon.type))
                continue;

            fireTimers[weapon.type] -= Time.deltaTime;

            if (weapon.type == ProjectileType.Rocket)
            {
                if (fireTimers[weapon.type] <= 0f && canAttack)
                {
                    StartCoroutine(FireMultiple(weapon, FireRocketSingle));
                    fireTimers[weapon.type] = weapon.specialInterval;
                }
            }
            else
            {
                if ((Input.GetMouseButton(0) || Input.touchCount > 0) && fireTimers[weapon.type] <= 0f && canAttack)
                {
                    switch (weapon.type)
                    {
                        case ProjectileType.Default:
                            FireDefaultSpread(weapon);
                            break;
                        case ProjectileType.Uzi:
                            StartCoroutine(FireMultiple(weapon, FireUziSingle));
                            break;
                        case ProjectileType.Boomerang:
                            FireDefaultSpread(weapon);
                            break;
                    }

                    fireTimers[weapon.type] = 1f / weapon.fireRate;
                }
            }
        }
    }
    private void FireDefaultSpread(RuntimeWeapon weapon)
    {
        AudioManager.Instance.PlayProjectileSFX(weapon.type);
        float spacing = 0.4f; // mermiler arasýndaki yatay mesafe
        float startOffset = -(weapon.projectileAmount - 1) * spacing * 0.5f;

        for (int i = 0; i < weapon.projectileAmount; i++)
        {
            Vector3 offset = new Vector3(startOffset + i * spacing, 0, 0);
            GameObject go = Instantiate(weapon.projectilePrefab, firePoint.position + offset, firePoint.rotation);
            ApplyWeaponStats(go, weapon);
        }
    }
    private IEnumerator FireMultiple(RuntimeWeapon weapon, System.Action<RuntimeWeapon> fireMethod)
    {
        for (int i = 0; i < weapon.projectileAmount; i++)
        {
            AudioManager.Instance.PlayProjectileSFX(weapon.type);
            fireMethod.Invoke(weapon);
            yield return new WaitForSeconds(0.075f); 
        }
    }
    //Tekli default atýþý yapýlýrsa kullanýlabilir
    private void FireDefaultSingle(RuntimeWeapon weapon)
    {
        float spacing = 0.3f;
        float startOffset = -(weapon.projectileAmount - 1) * spacing * 0.5f;

        GameObject go = Instantiate(weapon.projectilePrefab, firePoint.position + new Vector3(startOffset, 0, 0), firePoint.rotation);
        ApplyWeaponStats(go, weapon);
    }

    private void FireUziSingle(RuntimeWeapon weapon)
    {
        AudioManager.Instance.PlayProjectileSFX(weapon.type);

        float angle = Random.Range(-15f, 15f);
        Quaternion rot = firePoint.rotation * Quaternion.Euler(0, 0, angle);

        GameObject go = Instantiate(weapon.projectilePrefab, firePoint.position, rot);
        if (go.TryGetComponent<ProjectileBase>(out var proj))
        {
            proj.damage = Mathf.RoundToInt(weapon.damage * weapon.damageMultiplier);
            proj.piercingAmount = weapon.piercingAmount;
            float speedVar = Random.Range(0.6f, 1.2f);
            float sizeVar = Random.Range(0.4f, 1f);
            proj.speed *= speedVar;
            proj.transform.localScale *= sizeVar;
        }
    }

    private void FireRocketSingle(RuntimeWeapon weapon)
    {
        AudioManager.Instance.PlayProjectileSFX(weapon.type);
        GameObject go = Instantiate(weapon.projectilePrefab, firePoint.position, firePoint.rotation);
        ApplyWeaponStats(go, weapon);
    }

    //Tekli boomerang atýþý yapýlýrsa kullanýlabilir
    private void FireBoomerangSingle(RuntimeWeapon weapon)
    {
        AudioManager.Instance.PlayProjectileSFX(weapon.type);
        GameObject go = Instantiate(weapon.projectilePrefab, firePoint.position, firePoint.rotation);
        ApplyWeaponStats(go, weapon);
    }

    private void ApplyWeaponStats(GameObject go, RuntimeWeapon weapon)
    {
        if (go.TryGetComponent<ProjectileBase>(out var proj))
        {
            proj.damage = Mathf.RoundToInt(weapon.damage * weapon.damageMultiplier);
            proj.piercingAmount = weapon.piercingAmount;
        }
    }

    private bool IsWeaponUnlocked(ProjectileType type)
    {
        return type switch
        {
            ProjectileType.Default => isUnlockDefault,
            ProjectileType.Uzi => isUnlockUzi,
            ProjectileType.Rocket => isUnlockRocket,
            ProjectileType.Boomerang => isUnlockBoomerang,
            _ => false
        };
    }

    // Frenzy aktif olduðunda geçici çarpan uygula
    public void MultiplyFireRates(float multiplier)
    {
        if (isFrenzyActive) return;
        isFrenzyActive = true;

        frenzyBackupFireRates.Clear();

        foreach (var weapon in runtimeWeapons)
        {
            frenzyBackupFireRates[weapon] = weapon.fireRate;

            weapon.fireRate *= multiplier;
        }
        if (frenzyIndicator != null)
            frenzyIndicator.SetActive(true);
    }
    public void ResetFireRates()
    {
        foreach (var kvp in frenzyBackupFireRates)
        {
            kvp.Key.fireRate = kvp.Value;
        }

        frenzyBackupFireRates.Clear();
        if (frenzyIndicator != null)
            frenzyIndicator.SetActive(false);

        isFrenzyActive = false;
    }
}
