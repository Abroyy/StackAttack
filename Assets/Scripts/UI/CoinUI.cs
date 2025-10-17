using UnityEngine;
using TMPro;

public class CoinUI : MonoBehaviour
{
    public TextMeshProUGUI coinText;

    private void OnEnable()
    {
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.OnCoinChanged += UpdateUI;
            UpdateUI(CurrencyManager.Instance.GetCoin());
        }
    }

    private void OnDisable()
    {
        if (CurrencyManager.Instance != null)
            CurrencyManager.Instance.OnCoinChanged -= UpdateUI;
    }

    private void UpdateUI(int coinAmount)
    {
        if (coinText != null)
            coinText.text = coinAmount.ToString();
    }
}
