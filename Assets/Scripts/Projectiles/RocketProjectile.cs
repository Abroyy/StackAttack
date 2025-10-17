using UnityEngine;

public class RocketProjectile : ProjectileBase
{
    public float swayAmount = 1f;
    public float swaySpeed = 2f;
    private float time;

    public float explosionRadius = 2f;
    public LayerMask blockLayer;

    private float phaseOffset;   // her roket için farklý offset
    private float swayVar;       // isteðe baðlý küçük varyasyon

    protected override void Start()
    {
        base.Start();
        phaseOffset = Random.Range(0f, Mathf.PI * 2f);
        swayVar = Random.Range(0.8f, 1.2f);
    }

    protected override void Move()
    {
        time += Time.deltaTime;

        float sway = Mathf.Sin(time * swaySpeed * swayVar + phaseOffset) * swayAmount;

        Vector3 direction = transform.up + new Vector3(sway, 0, 0);
        transform.position += direction.normalized * speed * Time.deltaTime;
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Block"))
        {
            Explode();
        }
    }

    private void Explode()
    {
        Collider2D[] hitBlocks = Physics2D.OverlapCircleAll(transform.position, explosionRadius, blockLayer);
        foreach (var hit in hitBlocks)
        {
            if (hit.TryGetComponent(out IDestructible destructible))
                destructible.TakeDamage(damage);
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
