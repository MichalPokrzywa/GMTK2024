using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AllMapsData", menuName = "Custom/All Maps Data")]
public class MapTable : ScriptableObject
{
    public List<GameMap> maps = new List<GameMap>();
}

[System.Serializable]
public class GameMap
{
    public string mapName;
    public int xSize = 6;
    public int ySize = 7;
    public List<int> values;  // Changed from int[,] to List<int>

    public void OnValidate()
    {
        int expectedSize = xSize * ySize;
        if (values == null || values.Count != expectedSize)
        {
            values = new List<int>(new int[expectedSize]);
        }
    }

    public int GetValue(int x, int y)
    {
        return values[y * xSize + x];
    }

    public void SetValue(int x, int y, int value)
    {
        values[y * xSize + x] = value;
    }
}
