using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NotificationTrigger : MonoBehaviour
{
    public Image goldRush;
    public Image boss;

    public bool isBoss;
    public bool isGoldRush;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && isBoss)
        {
            boss.gameObject.SetActive(true);
            StartCoroutine(CloseNotification());
        }
        else if(collision.CompareTag("Player") && isGoldRush)
        {
            goldRush.gameObject.SetActive(true);
            StartCoroutine(CloseNotification());
        }
    }

    private IEnumerator CloseNotification()
    {
        yield return new WaitForSeconds(1);
        goldRush.gameObject.SetActive(false);
        boss.gameObject.SetActive(false);
        
    }
}
