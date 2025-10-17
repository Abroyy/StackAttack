using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AbilityBar : MonoBehaviour
{
    [Header("UI")]
    public Image fillImage;

    [Header("Settings")]
    public float fillPerBlock = 0.1f;
    public float minFillPerBlock = 0.02f;
    public float reduction = 0.8f;
    public float fillSpeed = 3f;

    [Header("References")]
    public AbilitySystem abilitySystem;

    private float targetFill = 0f;
    private Coroutine fillRoutine;
    private Coroutine triggerRoutine;
    private bool isTriggering = false;

    private void OnEnable()
    {
        GameEvents.OnBlockDestroyed += HandleBlockDestroyed;
    }

    private void OnDisable()
    {
        GameEvents.OnBlockDestroyed -= HandleBlockDestroyed;
    }

    private void HandleBlockDestroyed()
    {
        // Sadece bar dolma aþamasýndaysa güncelle
        if (isTriggering) return;

        targetFill += fillPerBlock;
        targetFill = Mathf.Clamp01(targetFill);

        if (fillRoutine != null)
            StopCoroutine(fillRoutine);
        fillRoutine = StartCoroutine(SmoothFill());

        // Eðer bar dolduysa, sadece 1 kez tetikle
        if (targetFill >= 1f && !isTriggering)
        {
            triggerRoutine = StartCoroutine(ResetAndTriggerAbility());
        }
    }

    private IEnumerator SmoothFill()
    {
        float start = fillImage.fillAmount;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * fillSpeed;
            fillImage.fillAmount = Mathf.Lerp(start, targetFill, t);
            yield return null;
        }

        fillImage.fillAmount = targetFill;
    }

    private IEnumerator ResetAndTriggerAbility()
    {
        if (isTriggering) yield break;
        isTriggering = true;

        yield return new WaitUntil(() => Mathf.Approximately(fillImage.fillAmount, 1f));
        yield return new WaitForSeconds(0.1f);

        abilitySystem.ShowChoices();

        // Barý sýfýrla
        targetFill = 0f;

        fillPerBlock -= reduction;
        fillPerBlock = Mathf.Max(fillPerBlock, minFillPerBlock);

        if (fillRoutine != null)
            StopCoroutine(fillRoutine);
        fillRoutine = StartCoroutine(SmoothFill());

        // Biraz gecikme ile yeniden dolmaya izin ver
        yield return new WaitForSeconds(0.3f);
        isTriggering = false;
    }
}
