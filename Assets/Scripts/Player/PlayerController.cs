using UnityEngine;
using UnityEngine.EventSystems;
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 10f;
    public float boundary = 3f;

    private Vector3 targetPos;

    private void Update()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        HandleInput();
        Move();
    }

    private void HandleInput()
    {
        // Touch
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, Camera.main.nearClipPlane));
            targetPos = new Vector3(pos.x, transform.position.y, 0f);
        }

        // Mouse
        if (Input.GetMouseButton(0))
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
            targetPos = new Vector3(pos.x, transform.position.y, 0f);
        }
    }

    private void Move()
    {
        float clampedX = Mathf.Clamp(targetPos.x, -boundary, boundary);
        Vector3 target = new Vector3(clampedX, transform.position.y, 0f);
        transform.position = Vector3.Lerp(transform.position, target, moveSpeed * Time.deltaTime);
    }
}
