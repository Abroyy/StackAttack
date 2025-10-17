using UnityEngine;

public class PlayerShootingControl : MonoBehaviour
{
    public PlayerShooting playerShooting;
    public bool canAttack;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            playerShooting.canAttack = canAttack;
        }
    }
}
