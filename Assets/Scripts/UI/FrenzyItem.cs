using UnityEngine;
using System.Collections;

public class FrenzyItem : MonoBehaviour
{
    public float frenzyDuration = 10f;
    public float fireRateMultiplier = 4f; // 2-3x arasýnda

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerShooting playerShooting = other.GetComponent<PlayerShooting>();
            if (playerShooting != null)
                playerShooting.StartCoroutine(FrenzyBoost(playerShooting));

            Destroy(gameObject);
        }
    }

    private IEnumerator FrenzyBoost(PlayerShooting playerShooting)
    {
        playerShooting.MultiplyFireRates(fireRateMultiplier);
        yield return new WaitForSeconds(frenzyDuration);
        playerShooting.ResetFireRates();
    }
}
