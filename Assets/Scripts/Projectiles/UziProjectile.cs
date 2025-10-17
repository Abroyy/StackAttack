using UnityEngine;

public class UziProjectile : ProjectileBase
{
    private Vector3 direction;

    protected override void Start()
    {
        base.Start();
        float angle = Random.Range(-10f, 10f);
        direction = Quaternion.Euler(0, 0, angle) * Vector3.up;
    }

    protected override void Move()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }
}
