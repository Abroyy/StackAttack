using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BlockData
{
    public GameObject prefab;
    public BlockGroupType groupType = BlockGroupType.Static;
}

[CreateAssetMenu(fileName = "LevelData", menuName = "Level/LevelData")]
public class LevelData : ScriptableObject
{
    public Vector2 levelSize = new Vector2(5.2f, 200f);
    public Vector2 blockSize = new Vector2(1f, 1f);
    public List<LevelSegment> segments;
}
