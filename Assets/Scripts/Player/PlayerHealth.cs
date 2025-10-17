using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHearts = 3;
    private int currentHearts;

    [Header("UI")]
    public Image[] heartImages;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    [Header("Invincibility")]
    public float invincibleTime = 1f; // Çarptýktan sonra kýsa bir süre hasar almama
    private bool isInvincible = false;

    private void Start()
    {
        currentHearts = maxHearts;
        UpdateHeartsUI();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Block"))
        {
            Debug.Log("Block ile çarpýþtý");
            TakeDamage(1);
        }
    }

    public void TakeDamage(int amount)
    {
        if (isInvincible) return;

        currentHearts -= amount;
        currentHearts = Mathf.Clamp(currentHearts, 0, maxHearts);
        UpdateHeartsUI();

        if (currentHearts <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(InvincibilityFrames());
        }
    }

    private void UpdateHeartsUI()
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            heartImages[i].sprite = i < currentHearts ? fullHeart : emptyHeart;
        }
    }

    private void Die()
    {
        Debug.Log("Player öldü!");
        GameEvents.InvokeGameOver();
        gameObject.SetActive(false);
    }

    private System.Collections.IEnumerator InvincibilityFrames()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibleTime);
        isInvincible = false;
    }
}
