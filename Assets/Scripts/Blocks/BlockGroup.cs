using UnityEngine;

public enum BlockGroupType
{
    Static,
    VerticalMove,
    HorizontalMove,
    CircularRotate
}

public class BlockGroup : MonoBehaviour
{
    public BlockGroupType groupType;
    public float speed = 2f;
    public float radius = 2f;
    public Vector3 moveDirection = Vector3.down;

    private Vector3 startPos;
    private float angle;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        switch (groupType)
        {
            case BlockGroupType.Static:
                break;

            case BlockGroupType.VerticalMove:
                transform.position = startPos + Vector3.up * Mathf.Sin(Time.time * speed);
                break;

            case BlockGroupType.HorizontalMove:
                transform.position = startPos + Vector3.right * Mathf.Sin(Time.time * speed);
                break;

            case BlockGroupType.CircularRotate:
                angle += speed * Time.deltaTime;
                transform.position = startPos + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * radius;
                break;
        }
    }
}
