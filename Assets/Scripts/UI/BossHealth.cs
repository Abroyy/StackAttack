using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class BossHealth : MonoBehaviour
{
    [Header("Boss Stats")]
    public string bossName = "Default Boss";
    public float maxHealth = 1000f;
    [HideInInspector] public float currentHealth;

    [Header("Phases (optional)")]
    public float phase2Threshold = 0.6f; // HP %60 altýna düþünce
    public float phase3Threshold = 0.3f; // HP %30 altýna düþünce

    private bool phase2Triggered = false;
    private bool phase3Triggered = false;

    public event Action<float, float> OnHealthChanged;
    public event Action OnBossDead;
    public event Action<int> OnPhaseChanged; // 1, 2, 3 gibi fazlar

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (currentHealth <= 0f) return;

        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0f);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        CheckPhases();

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    private void CheckPhases()
    {
        float ratio = currentHealth / maxHealth;

        if (!phase2Triggered && ratio <= phase2Threshold)
        {
            phase2Triggered = true;
            OnPhaseChanged?.Invoke(2);
        }

        if (!phase3Triggered && ratio <= phase3Threshold)
        {
            phase3Triggered = true;
            OnPhaseChanged?.Invoke(3);
        }
    }

    private void Die()
    {
        // Eventi tetikle
        GameEvents.InvokeBossDead();

        Debug.Log($"{bossName} has been defeated!");
        Destroy(gameObject, 1f);
    }
}
