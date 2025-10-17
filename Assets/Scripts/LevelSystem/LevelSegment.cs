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

    [Header("Y Aralýðý")]
    public float startY;
    public float endY;

    [Header("Yoðunluk & Hýz")]
    [Range(0f, 1f)] public float density = 0.5f;
    public float flowSpeedMultiplier = 1f;
    public bool showWarningUI = false;

    [Header("Blok Ayarlarý")]
    public List<BlockData> possibleBlocks;
    public int minHealth = 3;
    public int maxHealth = 10;

    [Header("Coin Ayarlarý")]
    public GameObject coinPrefab;

    [Header("Özel Ayarlar (Opsiyonel)")]
    public bool spawnFrenzyHere = false;

    [Header("Boss")]
    public BossHealth bossPrefab;

    public bool disableAttack = false;
}
