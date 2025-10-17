using UnityEngine;
using System.Collections;

public class FlowController : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Transform cam;
    public CanvasGroup startUI;
    public RectTransform fingerIcon;
    public PlayerShooting playerShooting;
    public WarningManager warningManager;

    [Header("Level Data")]
    public LevelData levelData;

    [Header("Default Settings")]
    public float baseSpeed = 5f;
    public float deceleration = 2f;
    public float startAccelerationTime = 1f;
    public float fingerMoveDistance = 100f;
    public float fingerMoveSpeed = 1.5f;

    private float currentSpeed;
    private bool started = false;
    private Vector2 fingerStartPos;
    private int currentSegmentIndex = -1;

    [Header("Boss Info")]
    public BossHealthUI bossUI;
    public Transform bossSpawnPoint;

    private void Start()
    {
        currentSpeed = 0f;

        if (startUI != null)
        {
            startUI.alpha = 1f;
            startUI.blocksRaycasts = true;
            startUI.interactable = true;
        }

        if (fingerIcon != null)
            fingerStartPos = fingerIcon.anchoredPosition;
    }
    private void Update()
    {
        if (!started)
        {
            AnimateFinger();
            CheckForStartInput();
            return;
        }

        MovePlayer();
        if (levelData == null || levelData.segments.Count == 0)
            return;

        if (currentSegmentIndex + 1 < levelData.segments.Count)
        {
            LevelSegment nextSeg = levelData.segments[currentSegmentIndex + 1];

            if (player.position.y >= nextSeg.startY)
            {
                currentSegmentIndex++;
                OnEnterSegment(nextSeg);
            }
        }
    }
    private void AnimateFinger()
    {
        if (fingerIcon == null) return;

        float offset = Mathf.Sin(Time.time * fingerMoveSpeed) * fingerMoveDistance;
        fingerIcon.anchoredPosition = fingerStartPos + new Vector2(offset, 0);
    }
    private void CheckForStartInput()
    {
        bool pressed = Input.GetMouseButtonDown(0) || Input.touchCount > 0;

        if (pressed)
        {
            StartCoroutine(StartFlowSmooth());
        }
    }
    private IEnumerator StartFlowSmooth()
    {
        started = true;

        if (startUI != null)
        {
            float fade = 1f;
            while (fade > 0)
            {
                fade -= Time.deltaTime * 2f;
                startUI.alpha = fade;
                yield return null;
            }
            startUI.alpha = 0f;
            startUI.blocksRaycasts = false;
        }

        if (fingerIcon != null)
            fingerIcon.gameObject.SetActive(false);

        float t = 0f;
        float target = baseSpeed;
        while (t < 1f)
        {
            t += Time.deltaTime / startAccelerationTime;
            currentSpeed = Mathf.Lerp(0, target, t);
            MovePlayer();
            yield return null;
        }

        currentSpeed = target;

        if (levelData != null && levelData.segments.Count > 0)
        {
            currentSegmentIndex = 0;
            OnEnterSegment(levelData.segments[0]);
        }
    }
    private void MovePlayer()
    {
        Vector3 move = Vector3.up * currentSpeed * Time.deltaTime;
        if (player != null) player.position += move;
        if (cam != null) cam.position += move;
    }
    private void OnEnterSegment(LevelSegment seg)
    {
        StopAllCoroutines(); // hýz geçiþlerini net tutmak için
        StartCoroutine(AdjustSpeed(baseSpeed * seg.flowSpeedMultiplier));

        if (seg.disableAttack)
            playerShooting.canAttack = false;
        else
            playerShooting.canAttack = true;

        if (seg.bossPrefab != null)
        {
            BossHealth boss = Instantiate(seg.bossPrefab, bossSpawnPoint.position, Quaternion.identity);
            bossUI.BindBoss(boss);
            bossUI.gameObject.SetActive(true);
        }

        if (seg.showWarningUI)
            warningManager.ShowSegmentWarning(seg.segmentName);
    }
    private IEnumerator AdjustSpeed(float target)
    {
        float start = currentSpeed;
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime;
            currentSpeed = Mathf.Lerp(start, target, t);
            yield return null;
        }
    }
    //Trigger ile tetiklemek isterseniz kullanabilirsiniz
    public void Boost(float amount) => currentSpeed += amount;
    public void SlowDown(float amount) => currentSpeed = Mathf.Max(0, currentSpeed - amount);
}
