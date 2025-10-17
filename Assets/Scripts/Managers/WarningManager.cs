using UnityEngine;
using System.Collections;

public class WarningManager : MonoBehaviour
{
    public CanvasGroup segmentWarningUI;
    public TMPro.TextMeshProUGUI warningText;

    public void ShowSegmentWarning(string segmentName)
    {
        StartCoroutine(ShowWarningCoroutine(segmentName));
    }

    private IEnumerator ShowWarningCoroutine(string text)
    {
        warningText.text = $"Entering {text}!";
        segmentWarningUI.alpha = 1;
        yield return new WaitForSeconds(2f);
        segmentWarningUI.alpha = 0;
    }
}
