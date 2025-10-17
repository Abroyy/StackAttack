using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossHealthUI : MonoBehaviour
{
    [Header("UI References")]
    public Image healthFillImage;     
    public TextMeshProUGUI bossNameText;

    private BossHealth currentBoss;

    private void OnEnable()
    {
        HideUI();
    }

    public void BindBoss(BossHealth boss)
    {
        if (currentBoss != null)
        {
            currentBoss.OnHealthChanged -= UpdateHealthBar;
            currentBoss.OnBossDead -= HideUI;
        }

        currentBoss = boss;

        bossNameText.text = boss.bossName;
        UpdateHealthBar(boss.currentHealth, boss.maxHealth);

        boss.OnHealthChanged += UpdateHealthBar;
        boss.OnBossDead += HideUI;

        ShowUI();
    }

    private void UpdateHealthBar(float current, float max)
    {
        if (healthFillImage != null)
        {
            healthFillImage.fillAmount = Mathf.Clamp01(current / max);
        }
    }

    private void HideUI()
    {
        gameObject.SetActive(false);
    }

    private void ShowUI()
    {
        gameObject.SetActive(true);
    }
}
