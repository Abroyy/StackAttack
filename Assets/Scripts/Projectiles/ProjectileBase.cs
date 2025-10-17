using UnityEngine;

public abstract class ProjectileBase : MonoBehaviour
{
    public int damage = 1;
    public float speed = 5f;
    public float lifetime = 5f;
    public int piercingAmount = 0; 

    private int piercedCount = 0;
    protected float timer;

    protected virtual void Start()
    {
        Destroy(gameObject, lifetime);
    }

    protected virtual void Update()
    {
        timer += Time.deltaTime;
        Move();
    }

    protected abstract void Move();

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Boundary"))
        {
            Destroy(gameObject);
            return;
        }

        if (other.CompareTag("Block"))
        {
            if (other.TryGetComponent(out IDestructible destructible))
            {
                destructible.TakeDamage(damage);
            }

            piercedCount++;

            // Eðer piercingAmount dolduysa yok et
            if (piercedCount > piercingAmount)
            {
                Destroy(gameObject);
            }
            return;
        }

        if (other.TryGetComponent(out BossHealth boss))
        {
            boss.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
