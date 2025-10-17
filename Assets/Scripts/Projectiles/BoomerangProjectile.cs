using UnityEngine;

public class BoomerangProjectile : ProjectileBase
{
    public float maxDistance = 5f;
    public float curveAmplitude = 1f; 
    public float curveFrequency = 3f;
    private bool returning = false;

    private Transform player;
    private Vector3 startPos;
    private Vector3 direction;
    private float traveledDistance = 0f;
    private float angle = 0f;

    protected override void Start()
    {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        startPos = transform.position;
        direction = transform.up.normalized;

        GetComponent<SpriteRenderer>().transform.Rotate(0f, 0f, 0f);
    }

    protected override void Move()
    {
        if (!returning)
        {
            traveledDistance += speed * Time.deltaTime;
            angle += Time.deltaTime * curveFrequency * 360f;

            Vector3 offset = transform.right * Mathf.Sin(angle * Mathf.Deg2Rad) * curveAmplitude;
            transform.position += direction * speed * Time.deltaTime + offset * Time.deltaTime;

            if (Vector3.Distance(startPos, transform.position) >= maxDistance)
            {
                returning = true;
            }
        }
        else
        {
            // geri dönüþte hedefe yönelME
            Vector3 toPlayer = (player.position - transform.position).normalized;
            traveledDistance += speed * Time.deltaTime;
            angle += Time.deltaTime * curveFrequency * 360f;

            Vector3 offset = Vector3.Cross(toPlayer, Vector3.forward).normalized * Mathf.Sin(angle * Mathf.Deg2Rad) * curveAmplitude;
            transform.position += (toPlayer * speed * Time.deltaTime) + offset * Time.deltaTime;
        }

        // rotasyon efekti
        transform.Rotate(0f, 0f, speed * Time.deltaTime * 500f);
    }
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Block"))
        {
            if (other.TryGetComponent(out IDestructible destructible))
                destructible.TakeDamage(damage);
        }

        if (returning && other.CompareTag("Player"))
            Destroy(gameObject);
    }
}
