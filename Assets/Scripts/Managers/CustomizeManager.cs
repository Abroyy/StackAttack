using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CustomizeManager : MonoBehaviour
{
    [Header("References")]
    public SpriteRenderer playerRenderer;
    public TrailRenderer playerTrail;

    [Header("Panels")]
    public GameObject skinPanel;
    public GameObject trailPanel;

    [Header("Buttons")]
    public Button skinTabButton;
    public Button trailTabButton;

    [Header("Skin Renkleri")]
    public List<Button> skinButtons;
    public List<Color> skinColors;

    [Header("Trail Renkleri")]
    public List<Button> trailButtons;
    public List<Color> trailColors;

    private const string SelectedSkinKey = "SelectedSkinColorIndex";
    private const string SelectedTrailKey = "SelectedTrailColorIndex";

    private int currentSkinIndex = 0;
    private int currentTrailIndex = 0;

    private void Start()
    {
        LoadSavedSelections();

        ShowSkinPanel();
        skinTabButton.onClick.AddListener(ShowSkinPanel);
        trailTabButton.onClick.AddListener(ShowTrailPanel);

        SetupButtons();
        ApplySkin(currentSkinIndex);
        ApplyTrailColor(currentTrailIndex);
        UpdateHighlights();
    }
    public void ShowSkinPanel()
    {
        skinPanel.SetActive(true);
        trailPanel.SetActive(false);

        SetActiveTab(skinTabButton, true);
        SetActiveTab(trailTabButton, false);
    }
    public void ShowTrailPanel()
    {
        skinPanel.SetActive(false);
        trailPanel.SetActive(true);

        SetActiveTab(skinTabButton, false);
        SetActiveTab(trailTabButton, true);
    }
    private void SetActiveTab(Button btn, bool isActive)
    {
        btn.transform.localScale = isActive ? Vector3.one * 1.1f : Vector3.one;
    }
    private void SetupButtons()
    {
        // Skin butonlarý
        for (int i = 0; i < skinButtons.Count; i++)
        {
            int index = i;
            Image img = skinButtons[i].GetComponent<Image>();
            if (img != null && i < skinColors.Count)
                img.color = skinColors[i];

            skinButtons[i].onClick.RemoveAllListeners();
            skinButtons[i].onClick.AddListener(() => OnSkinSelected(index));
        }

        // Trail butonlarý
        for (int i = 0; i < trailButtons.Count; i++)
        {
            int index = i;
            Image img = trailButtons[i].GetComponent<Image>();
            if (img != null && i < trailColors.Count)
                img.color = trailColors[i];

            trailButtons[i].onClick.RemoveAllListeners();
            trailButtons[i].onClick.AddListener(() => OnTrailSelected(index));
        }
    }

    private void OnSkinSelected(int index)
    {
        currentSkinIndex = index;
        ApplySkin(index);
        SaveSelections();
        UpdateHighlights();
    }

    private void OnTrailSelected(int index)
    {
        currentTrailIndex = index;
        ApplyTrailColor(index);
        SaveSelections();
        UpdateHighlights();
    }

    private void ApplySkin(int index)
    {
        if (playerRenderer != null && index < skinColors.Count)
            playerRenderer.color = skinColors[index];
    }
    private void ApplyTrailColor(int index)
    {
        if (playerTrail != null && index < trailColors.Count)
        {
            Gradient oldGradient = playerTrail.colorGradient;
            GradientColorKey[] oldColorKeys = oldGradient.colorKeys;
            GradientAlphaKey[] oldAlphaKeys = oldGradient.alphaKeys;

            Color newColor = trailColors[index];

            GradientColorKey[] newColorKeys = new GradientColorKey[oldColorKeys.Length];
            for (int i = 0; i < oldColorKeys.Length; i++)
            {
                newColorKeys[i] = new GradientColorKey(newColor, oldColorKeys[i].time);
            }

            Gradient newGradient = new Gradient();
            newGradient.SetKeys(newColorKeys, oldAlphaKeys);

            playerTrail.colorGradient = newGradient;
        }
    }


    private void SaveSelections()
    {
        PlayerPrefs.SetInt(SelectedSkinKey, currentSkinIndex);
        PlayerPrefs.SetInt(SelectedTrailKey, currentTrailIndex);
        PlayerPrefs.Save();
    }

    private void LoadSavedSelections()
    {
        currentSkinIndex = PlayerPrefs.GetInt(SelectedSkinKey, 0);
        currentTrailIndex = PlayerPrefs.GetInt(SelectedTrailKey, 0);
    }

    private void UpdateHighlights()
    {
        // Skin highlight
        for (int i = 0; i < skinButtons.Count; i++)
        {
            var colors = skinButtons[i].colors;
            colors.normalColor = (i == currentSkinIndex) ? new Color(0.8f, 0.9f, 1f) : Color.white;
            skinButtons[i].colors = colors;
        }

        // Trail highlight
        for (int i = 0; i < trailButtons.Count; i++)
        {
            var colors = trailButtons[i].colors;
            colors.normalColor = (i == currentTrailIndex) ? new Color(0.8f, 1f, 0.8f) : Color.white;
            trailButtons[i].colors = colors;
        }
    }

    public static void ApplySavedCustomization(SpriteRenderer spriteRenderer, TrailRenderer trailRenderer,
                                               List<Color> skinColors, List<Color> trailColors)
    {
        int skinIndex = PlayerPrefs.GetInt(SelectedSkinKey, 0);
        int trailIndex = PlayerPrefs.GetInt(SelectedTrailKey, 0);

        if (spriteRenderer != null && skinIndex < skinColors.Count)
            spriteRenderer.color = skinColors[skinIndex];

        if (trailRenderer != null && trailIndex < trailColors.Count)
        {
            Gradient oldGradient = trailRenderer.colorGradient;
            GradientColorKey[] oldColorKeys = oldGradient.colorKeys;
            GradientAlphaKey[] oldAlphaKeys = oldGradient.alphaKeys;

            Color newColor = trailColors[trailIndex];

            GradientColorKey[] newColorKeys = new GradientColorKey[oldColorKeys.Length];
            for (int i = 0; i < oldColorKeys.Length; i++)
            {
                newColorKeys[i] = new GradientColorKey(newColor, oldColorKeys[i].time);
            }

            Gradient newGradient = new Gradient();
            newGradient.SetKeys(newColorKeys, oldAlphaKeys);
            trailRenderer.colorGradient = newGradient;
        }
    }
}
