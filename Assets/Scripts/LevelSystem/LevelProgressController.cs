using UnityEngine;
using UnityEngine.UI;

public class LevelProgressController : MonoBehaviour
{
    [Header("UI")]
    public Image progressBar;

    [Header("References")]
    public Transform player;
    public Transform levelStart;
    public Transform levelEnd;

    private float totalDistance;

    void Start()
    {
        if (levelStart != null && levelEnd != null)
        {
            totalDistance = Mathf.Abs(levelEnd.position.y - levelStart.position.y);
        }
    }

    void Update()
    {
        if (player == null || levelStart == null || levelEnd == null || progressBar == null)
            return;

        float currentDistance = Mathf.Abs(player.position.y - levelStart.position.y);
        float progress = Mathf.Clamp01(currentDistance / totalDistance);

        progressBar.fillAmount = progress;
    }
}
