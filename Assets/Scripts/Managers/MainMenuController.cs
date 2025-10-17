using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenuController : MonoBehaviour
{
    [Header("UI References")]
    public Button[] menuButtons;          
    public GameObject[] panels;           
    public RectTransform indicatorImage;  

    [Header("Settings")]
    public float indicatorMoveSpeed = 10f; // Ýþaretleyici hareket hýzý

    private int currentIndex = 0;
    private Coroutine moveRoutine;

    private void Start()
    {
        OpenPanel(0);
        MoveIndicatorTo(menuButtons[0].transform as RectTransform, true);

        for (int i = 0; i < menuButtons.Length; i++)
        {
            int index = i;
            menuButtons[i].onClick.AddListener(() => OnMenuButtonClicked(index));
        }
    }

    private void OnMenuButtonClicked(int index)
    {
        if (index == currentIndex) return; 

        currentIndex = index;
        OpenPanel(index);
        MoveIndicatorTo(menuButtons[index].transform as RectTransform);
    }

    private void OpenPanel(int index)
    {
        for (int i = 0; i < panels.Length; i++)
            panels[i].SetActive(i == index);
    }

    private void MoveIndicatorTo(RectTransform target, bool instant = false)
    {
        if (indicatorImage == null || target == null) return;

        if (moveRoutine != null)
            StopCoroutine(moveRoutine);

        if (instant)
        {
            indicatorImage.position = new Vector3(
                target.position.x,
                indicatorImage.position.y,
                indicatorImage.position.z
            );
        }
        else
        {
            moveRoutine = StartCoroutine(SmoothMoveIndicator(target));
        }
    }

    private IEnumerator SmoothMoveIndicator(RectTransform target)
    {
        while (Vector3.Distance(indicatorImage.position, target.position) > 0.1f)
        {
            indicatorImage.position = Vector3.Lerp(
                indicatorImage.position,
                new Vector3(target.position.x, indicatorImage.position.y, indicatorImage.position.z),
                Time.deltaTime * indicatorMoveSpeed
            );
            yield return null;
        }

        indicatorImage.position = new Vector3(target.position.x, indicatorImage.position.y, indicatorImage.position.z);
    }
}
