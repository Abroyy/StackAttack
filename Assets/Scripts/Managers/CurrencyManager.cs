using UnityEngine;
using UnityEngine.Events;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;

    public UnityAction<int> OnCoinChanged;

    private int coin;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadCoin();
        }
        else Destroy(gameObject);
    }

    public int GetCoin() => coin;

    public void AddCoin(int amount)
    {
        coin += amount;
        OnCoinChanged?.Invoke(coin);
        SaveCoin();
    }

    public void RemoveCoin(int amount)
    {
        coin = Mathf.Max(0, coin - amount);
        OnCoinChanged?.Invoke(coin);
        SaveCoin();
    }

    private void SaveCoin()
    {
        PlayerPrefs.SetInt("Coin", coin);
        PlayerPrefs.Save();
    }

    private void LoadCoin()
    {
        coin = PlayerPrefs.GetInt("Coin", 0);
        OnCoinChanged?.Invoke(coin);
    }
}
