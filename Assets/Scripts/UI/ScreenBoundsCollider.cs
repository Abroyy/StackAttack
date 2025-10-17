using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ScreenBoundsCollider : MonoBehaviour
{
    [Header("Collider Settings")]
    public float thickness = 0.1f; // Collider kalýnlýðý
    public bool showGizmos = false;

    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
        CreateScreenColliders();
    }

    private void CreateScreenColliders()
    {
        Vector2 bottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
        Vector2 topRight = cam.ViewportToWorldPoint(new Vector3(1, 1, cam.nearClipPlane));

        float width = topRight.x - bottomLeft.x;
        float height = topRight.y - bottomLeft.y;

        // Kenarlarýn pozisyonlarýný hesapla
        Vector2 topPos = new Vector2(0, topRight.y + thickness / 2);
        Vector2 bottomPos = new Vector2(0, bottomLeft.y - thickness / 2);
        Vector2 leftPos = new Vector2(bottomLeft.x - thickness / 2, 0);
        Vector2 rightPos = new Vector2(topRight.x + thickness / 2, 0);

        // Collider oluþtur
        CreateEdgeCollider("Top", topPos, new Vector2(width + thickness * 2, thickness));
    }

    private void CreateEdgeCollider(string name, Vector2 position, Vector2 size)
    {
        GameObject edge = new GameObject(name);
        edge.transform.SetParent(transform);
        edge.transform.position = position;

        BoxCollider2D col = edge.AddComponent<BoxCollider2D>();
        col.size = size;
        col.isTrigger = true; // çarpýnca objeyi yok edeceðiz
        edge.tag = "Boundary";
        edge.layer = 8;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!showGizmos || cam == null) return;
        Gizmos.color = Color.green;
        Vector3 bottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
        Vector3 topRight = cam.ViewportToWorldPoint(new Vector3(1, 1, cam.nearClipPlane));
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(topRight.x - bottomLeft.x, topRight.y - bottomLeft.y, 0));
    }
#endif
}
