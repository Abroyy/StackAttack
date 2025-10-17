using UnityEngine;

public class DefaultProjectile : ProjectileBase
{
    protected override void Move()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }
}
