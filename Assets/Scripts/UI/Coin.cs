using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour
{
    public int value = 1;
    public float flyDuration = 0.5f;

    [Header("Scene UI Reference")]
    public Transform coinTargetUI;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (coinTargetUI != null)
                StartCoroutine(FlyToUI());
            else
            {
                CurrencyManager.Instance.AddCoin(value);
                Destroy(gameObject);
            }
        }
    }

    private IEnumerator FlyToUI()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = coinTargetUI.position;

        float elapsed = 0f;
        while (elapsed < flyDuration)
        {
            float t = elapsed / flyDuration;
            transform.position = Vector3.Lerp(startPos, endPos, t);
            transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos;
        CurrencyManager.Instance.AddCoin(value);
        Destroy(gameObject);
    }
}
