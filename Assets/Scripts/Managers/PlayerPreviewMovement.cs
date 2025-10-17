using UnityEngine;
using System.Collections;

public class PlayerPreviewMovement : MonoBehaviour
{
    [Header("Square Movement Settings")]
    public float sideLength = 2f;    
    public float moveSpeed = 2f;     

    private Vector3 startPos;

    private void OnEnable()
    {
        startPos = transform.position;
        StartCoroutine(MoveInSquare());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator MoveInSquare()
    {
        Vector3[] directions = new Vector3[]
        {
            Vector3.right,
            Vector3.up,
            Vector3.left,
            Vector3.down
        };

        int dirIndex = 0;
        Vector3 target = startPos + directions[dirIndex] * sideLength;

        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, target) < 0.01f)
            {
                dirIndex = (dirIndex + 1) % 4;
                target = startPos + directions[dirIndex] * sideLength;
            }
            yield return null;
        }
    }
}
