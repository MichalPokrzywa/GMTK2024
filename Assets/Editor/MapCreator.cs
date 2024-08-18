using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapTable))]
public class MapCreator : Editor
{
    private MapTable mapTable;
    private int selectedMapIndex = 0;

    private Vector2 scrollPosition;

    private const string SelectedMapIndexKey = "MapTableEditor_SelectedMapIndex";

    private void OnEnable()
    {
        mapTable = (MapTable)target;

        // Restore the previously selected map index
        selectedMapIndex = EditorPrefs.GetInt(SelectedMapIndexKey, 0);
        selectedMapIndex = Mathf.Clamp(selectedMapIndex, 0, mapTable.maps.Count - 1);
    }

    private void OnDisable()
    {
        // Save the selected map index when the editor is disabled
        EditorPrefs.SetInt(SelectedMapIndexKey, selectedMapIndex);
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Display the list of maps
        if (mapTable.maps == null || mapTable.maps.Count == 0)
        {
            EditorGUILayout.HelpBox("No maps available. Add a new map to get started.", MessageType.Info);
        }
        else
        {
            // Dropdown to select a map
            string[] mapNames = GetMapNames();
            selectedMapIndex = EditorGUILayout.Popup("Select Map", selectedMapIndex, mapNames);

            // Display the selected map's data
            if (selectedMapIndex >= 0 && selectedMapIndex < mapTable.maps.Count)
            {
                DrawMapEditor(mapTable.maps[selectedMapIndex]);
            }
        }

        EditorGUILayout.Space();

        // Add/Remove map buttons
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add New Map"))
        {
            AddNewMap();
        }
        if (GUILayout.Button("Remove Selected Map") && mapTable.maps.Count > 0)
        {
            RemoveSelectedMap();
        }
        EditorGUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();

        // Mark the MapTable as dirty if any changes were made
        if (GUI.changed)
        {
            EditorUtility.SetDirty(mapTable);
        }
    }

    private void DrawMapEditor(GameMap map)
    {
        EditorGUILayout.LabelField("Map Details", EditorStyles.boldLabel);
        map.mapName = EditorGUILayout.TextField("Map Name", map.mapName);

        // Handle resizing of the grid
        int newXSize = EditorGUILayout.IntField("X Size", map.xSize);
        int newYSize = EditorGUILayout.IntField("Y Size", map.ySize);

        if (newXSize != map.xSize || newYSize != map.ySize)
        {
            map.xSize = newXSize;
            map.ySize = newYSize;
            map.OnValidate(); // Resize the list if the size changes
        }

        // Track whether a change has occurred
        bool hasChanged = false;

        // Scrollable grid for large maps
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Height(200)); // Set a fixed height for the scroll view
        for (int y = 0; y < map.ySize; y++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int x = 0; x < map.xSize; x++)
            {
                int currentValue = map.GetValue(x, y);
                int newValue = EditorGUILayout.IntField(currentValue, GUILayout.Width(30));

                if (newValue != currentValue)
                {
                    map.SetValue(x, y, newValue);
                    hasChanged = true;  // Mark that a change has occurred
                }
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndScrollView();

        // If any changes were detected, mark the ScriptableObject as dirty
        if (hasChanged)
        {
            EditorUtility.SetDirty(mapTable);
        }
    }

    private void AddNewMap()
    {
        string tmp;
        GameMap newMap = new GameMap {};
        if (mapTable.maps.Count == 0)
        {
            tmp = "New Map";
            newMap.mapName = $"{tmp}";
        }
        else
        {
            newMap.mapName = $"{mapTable.maps[0].mapName}_{mapTable.maps.Count}";
        }

        newMap.OnValidate(); // Initialize the map's array
        mapTable.maps.Add(newMap);
        selectedMapIndex = mapTable.maps.Count - 1; // Select the new map

        // Save the new selection
        EditorPrefs.SetInt(SelectedMapIndexKey, selectedMapIndex);
    }

    private void RemoveSelectedMap()
    {
        if (selectedMapIndex >= 0 && selectedMapIndex < mapTable.maps.Count)
        {
            mapTable.maps.RemoveAt(selectedMapIndex);
            selectedMapIndex = Mathf.Clamp(selectedMapIndex - 1, 0, mapTable.maps.Count - 1);

            // Save the new selection
            EditorPrefs.SetInt(SelectedMapIndexKey, selectedMapIndex);
        }
    }

    private string[] GetMapNames()
    {
        string[] names = new string[mapTable.maps.Count];
        for (int i = 0; i < mapTable.maps.Count; i++)
        {
            names[i] = mapTable.maps[i].mapName;
        }
        return names;
    }
}
