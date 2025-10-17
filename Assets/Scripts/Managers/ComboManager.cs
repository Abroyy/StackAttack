using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ComboManager : MonoBehaviour
{
    [Header("Combo Settings")]
    public float comboResetTime = 2f;
    public int comboPerCoin = 10;

    [Header("UI")]
    public TextMeshProUGUI comboText;
    public Image comboBarImage;

    private int currentCombo = 0;
    private Coroutine resetCoroutine;

    private void OnEnable()
    {
        GameEvents.OnBlockDestroyed += HandleBlockDestroyed;
    }

    private void OnDisable()
    {
        GameEvents.OnBlockDestroyed -= HandleBlockDestroyed;
    }

    private void HandleBlockDestroyed()
    {
        currentCombo++;
        UpdateComboUI();

        // Coin kontrol�
        if (currentCombo % comboPerCoin == 0)
        {
            GiveCoin(1);
        }

        // Reset coroutine ba�lat / resetle
        if (resetCoroutine != null) StopCoroutine(resetCoroutine);
        resetCoroutine = StartCoroutine(ComboTimer());
    }

    private IEnumerator ComboTimer()
    {
        float timer = comboResetTime;

        while (timer > 0)
        {
            timer -= Time.deltaTime;

            // Bar doluluk oran�n� g�ncelle
            if (comboBarImage != null)
                comboBarImage.fillAmount = timer / comboResetTime;

            yield return null;
        }

        // S�re dolunca combo s�f�rlan�r
        currentCombo = 0;
        UpdateComboUI();

        if (comboBarImage != null)
            comboBarImage.fillAmount = 0f;
    }

    private void UpdateComboUI()
    {
        if (comboText != null)
        {
            if (currentCombo >= 2)
            {
                comboText.gameObject.SetActive(true);
                comboText.text = $"{currentCombo}x";
            }
            else
            {
                comboText.gameObject.SetActive(false);
            }
        }

        if (comboBarImage != null)
        {
            // Bar sadece combo 2x ve �st� i�in g�sterilsin
            comboBarImage.gameObject.SetActive(currentCombo >= 2);
        }
    }

    private void GiveCoin(int amount)
    {
        Debug.Log($"Coin kazand�n! +{amount}");
        //GameEvents.NotifyCoinGained(amount);
    }
}
