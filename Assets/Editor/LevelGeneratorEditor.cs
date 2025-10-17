using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelGenerator))]
public class LevelGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        LevelGenerator generator = (LevelGenerator)target;

        if (GUILayout.Button("Generate Level"))
        {
            generator.GenerateLevel();
        }

        if (GUILayout.Button("Clear Level"))
        {
            generator.ClearLevel();
        }
    }

    private void OnSceneGUI()
    {
        LevelGenerator generator = (LevelGenerator)target;
        if (generator.levelData == null) return;

        Handles.color = Color.gray;
        int gridW = Mathf.FloorToInt(generator.levelData.levelSize.x / generator.levelData.blockSize.x);
        int gridH = Mathf.FloorToInt(generator.levelData.levelSize.y / generator.levelData.blockSize.y);

        for (int x = 0; x <= gridW; x++)
        {
            Vector3 start = generator.transform.position + new Vector3(x * generator.levelData.blockSize.x, 0, 0);
            Vector3 end = start + new Vector3(0, generator.levelData.levelSize.y, 0);
            Handles.DrawLine(start, end);
        }

        for (int y = 0; y <= gridH; y++)
        {
            Vector3 start = generator.transform.position + new Vector3(0, y * generator.levelData.blockSize.y, 0);
            Vector3 end = start + new Vector3(generator.levelData.levelSize.x, 0, 0);
            Handles.DrawLine(start, end);
        }
    }
}
