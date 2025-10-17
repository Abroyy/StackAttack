using UnityEngine;
using System.Collections.Generic;

public enum SegmentType
{
    Block,
    Coin,
    Empty,
    Boss
}

[System.Serializable]
public class LevelSegment
{
    public string segmentName;
    public SegmentType type;

    [Header("Y Aral���")]
    public float startY;
    public float endY;

    [Header("Yo�unluk & H�z")]
    [Range(0f, 1f)] public float density = 0.5f;
    public float flowSpeedMultiplier = 1f;
    public bool showWarningUI = false;

    [Header("Blok Ayarlar�")]
    public List<BlockData> possibleBlocks;
    public int minHealth = 3;
    public int maxHealth = 10;

    [Header("Coin Ayarlar�")]
    public GameObject coinPrefab;

    [Header("�zel Ayarlar (Opsiyonel)")]
    public bool spawnFrenzyHere = false;

    [Header("Boss")]
    public BossHealth bossPrefab;

    public bool disableAttack = false;
}
