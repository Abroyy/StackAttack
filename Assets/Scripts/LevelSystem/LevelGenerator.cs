using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public LevelData levelData;

    [Header("Frenzy Settings")]
    public GameObject frenzyItemPrefab;
    [Range(0f, 1f)] public float frenzySpawnChance = 0.8f;

    private int gridWidth;
    private List<GameObject> spawnedObjects = new List<GameObject>();
    private HashSet<Vector2Int> occupiedCells = new HashSet<Vector2Int>();

    private void Start()
    {
        if (levelData != null)
            GenerateLevel();
    }

    public void GenerateLevel()
    {
        ClearLevel();
        occupiedCells.Clear();

        gridWidth = Mathf.FloorToInt(levelData.levelSize.x / levelData.blockSize.x);

        foreach (var seg in levelData.segments)
        {
            float startY = seg.startY;
            float endY = seg.endY;

            for (float y = startY; y < endY; y++)
            {
                for (int x = 0; x < gridWidth; x++)
                {
                    Vector2Int cell = new Vector2Int(x, Mathf.RoundToInt(y / levelData.blockSize.y));

                    // Hücre doluysa geç
                    if (occupiedCells.Contains(cell))
                        continue;

                    // Segment boþluk tipi ise geç
                    if (seg.type == SegmentType.Empty)
                        continue;

                    // Yoðunluk kontrolü
                    if (Random.value > seg.density)
                        continue;

                    Vector3 pos = new Vector3(
                        x * levelData.blockSize.x,
                        y * levelData.blockSize.y,
                        0f
                    ) + transform.position;

                    // Tipine göre spawn
                    if (seg.type == SegmentType.Block)
                        SpawnBlock(pos, seg, Mathf.RoundToInt(y));
                    else if (seg.type == SegmentType.Coin)
                        SpawnCoin(pos, seg);

                    // Hücreyi iþaretle
                    occupiedCells.Add(cell);
                }
            }

            // Segment bazlý frenzy item
            if (seg.spawnFrenzyHere && frenzyItemPrefab != null)
                TrySpawnFrenzyItem(seg);
        }
    }

    private void SpawnBlock(Vector3 pos, LevelSegment seg, int yIndex)
    {
        if (seg.possibleBlocks == null || seg.possibleBlocks.Count == 0)
            return;

        BlockData blockData = seg.possibleBlocks[Random.Range(0, seg.possibleBlocks.Count)];
        GameObject block = Instantiate(blockData.prefab, pos, Quaternion.identity, transform);

        if (block.TryGetComponent(out Block blockComp))
        {
            int health = Random.Range(seg.minHealth, seg.maxHealth + 1);
            blockComp.Setup(health);
        }

        spawnedObjects.Add(block);
    }

    private void SpawnCoin(Vector3 pos, LevelSegment seg)
    {
        if (seg.coinPrefab == null) return;
        GameObject coin = Instantiate(seg.coinPrefab, pos, Quaternion.identity, transform);
        spawnedObjects.Add(coin);
    }

    private void TrySpawnFrenzyItem(LevelSegment seg)
    {
        if (frenzySpawnChance <= 0f || Random.value > frenzySpawnChance)
            return;

        Vector2Int cell = new Vector2Int(
            Random.Range(0, gridWidth),
            Mathf.FloorToInt(Random.Range(seg.startY, seg.endY) / levelData.blockSize.y)
        );

        if (occupiedCells.Contains(cell))
            return;

        Vector3 frenzyPos = new Vector3(
            cell.x * levelData.blockSize.x,
            cell.y * levelData.blockSize.y,
            0f
        ) + transform.position;

        Instantiate(frenzyItemPrefab, frenzyPos, Quaternion.identity);
        occupiedCells.Add(cell);
    }

    public void ClearLevel()
    {
        foreach (var go in spawnedObjects)
            if (go != null) DestroyImmediate(go);
        spawnedObjects.Clear();
    }
}
