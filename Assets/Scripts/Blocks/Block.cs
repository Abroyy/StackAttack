using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Block : MonoBehaviour, IDestructible
{
    [Header("Stats")]
    public int maxHealth = 5;
    private int currentHealth;

    [Header("Visual")]
    public List<Sprite> layerSprites;
    public GameObject spriteContainer;
    private List<GameObject> layers = new List<GameObject>();

    [Header("UI")]
    public TextMeshPro textPrefab;
    private TextMeshPro textInstance;

    [Header("Settings")]
    public float layerHeight = 0.1f;


    public void Setup(int health)
    {
        maxHealth = health;
        currentHealth = maxHealth;

        BuildVisual();
        SetupText();
        UpdateVisual();
    }
    private void BuildVisual()
    {
        if (spriteContainer == null) return;

        foreach (Transform child in spriteContainer.transform)
            DestroyImmediate(child.gameObject);
        layers.Clear();

        int layersToShow = Mathf.Min(maxHealth, 10);
        for (int i = 0; i < layersToShow; i++)
        {
            GameObject go = new GameObject($"Layer_{i + 1}");
            go.transform.SetParent(spriteContainer.transform, false);
            SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = layerSprites[Mathf.Min(i, layerSprites.Count - 1)];
            sr.sortingOrder = i;
            go.transform.localPosition = Vector3.up * i * layerHeight;

            // Random renk: alt bloklar daha koyu, üst bloklar daha açýk ton
            float t = (i + 1) / (float)layersToShow;
            sr.color = Color.Lerp(RandomDarkColor(), RandomBrightColor(), t);

            layers.Add(go);
        }
    }
    private Color RandomDarkColor()
    {
        return new Color(Random.Range(0f, 0.5f), Random.Range(0f, 0.5f), Random.Range(0f, 0.5f));
    }
    private Color RandomBrightColor()
    {
        return new Color(Random.Range(0.5f, 1f), Random.Range(0.5f, 1f), Random.Range(0.5f, 1f));
    }
    private void SetupText()
    {
        if (textPrefab == null) return;

        if (textInstance == null)
        {
            textInstance = Instantiate(textPrefab, transform);
            textInstance.alignment = TextAlignmentOptions.Center;
            textInstance.fontSize = 3;

            // Sorting Layer / Order
            textInstance.sortingLayerID = SortingLayer.NameToID("UI");
            textInstance.sortingOrder = 100;
        }

        UpdateTextPosition();
    }
    private void UpdateTextPosition()
    {
        if (textInstance == null) return;

        int activeLayers = Mathf.CeilToInt(Mathf.Clamp(currentHealth / (float)maxHealth * layers.Count, 0, layers.Count));
        textInstance.text = currentHealth.ToString();

        textInstance.gameObject.SetActive(activeLayers > 0);

        if (activeLayers > 0)
        {
            float normalized = activeLayers / (float)10;

            float totalHeight = layerHeight * 10;

            float yOffset = normalized * totalHeight - (layerHeight / 2f);

            textInstance.transform.localPosition = new Vector3(0f, yOffset, -0.01f);
        }
    }
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Max(0, currentHealth);
        UpdateVisual();

        if (currentHealth <= 0)
        {
            gameObject.SetActive(false);
            GameEvents.NotifyBlockDestroyed();
        }
            
    }
    private void UpdateVisual()
    {
        int layersToShow = Mathf.CeilToInt(currentHealth / (float)maxHealth * layers.Count);

        for (int i = 0; i < layers.Count; i++)
            layers[i].SetActive(i < layersToShow);

        UpdateTextPosition();
    }
}
