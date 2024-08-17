#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Verpha.HierarchyDesigner
{
    public class HierarchyDesigner_Window_Presets : EditorWindow
    {
        private int selectedPresetIndex = 0;
        private string[] presetNames;
        private bool applyToFolders = true;
        private bool applyToSeparators = true;
        private bool applyToTagAndLayer = true;
        private bool applyToTree = true;
        private Vector2 scrollPosition;

        [MenuItem("Hierarchy Designer/Hierarchy Helpers/Presets", false, 50)]
        private static void OpenWindow()
        {
            HierarchyDesigner_Window_Presets window = GetWindow<HierarchyDesigner_Window_Presets>("Hierarchy Presets");
            window.minSize = new Vector2(300, 220);
        }

        private void OnEnable()
        {
            presetNames = HierarchyDesigner_Utility_Presets.GetPresetNames();
        }

        private void OnGUI()
        {
            GUIStyle customSettingsStyle = HierarchyDesigner_Info_OnGUI.CreateCustomStyle();
            GUILayout.BeginVertical(customSettingsStyle);
            GUILayout.Space(5);

            GUILayout.Label("Hierarchy Presets", EditorStyles.boldLabel);
            GUILayout.Space(5);

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            selectedPresetIndex = EditorGUILayout.Popup("Choose A Preset:", selectedPresetIndex, presetNames);

            GUILayout.Space(10);
            GUILayout.Label("Apply To:", EditorStyles.boldLabel);
            GUILayout.Space(5);
            applyToFolders = EditorGUILayout.Toggle("Folders", applyToFolders);
            applyToSeparators = EditorGUILayout.Toggle("Separators", applyToSeparators);
            applyToTagAndLayer = EditorGUILayout.Toggle("Tag and Layer", applyToTagAndLayer);
            applyToTree = EditorGUILayout.Toggle("Hierarchy Tree", applyToTree);

            GUILayout.Space(10);
            if (GUILayout.Button("Apply Preset", GUILayout.Height(30)))
            {
                ApplySelectedPreset();
            }

            EditorGUILayout.EndScrollView();
            GUILayout.EndVertical();
        }

        private void ApplySelectedPreset()
        {
            if (selectedPresetIndex < 0 || selectedPresetIndex >= presetNames.Length) return;

            HierarchyDesigner_Info_Presets selectedPreset = HierarchyDesigner_Utility_Presets.Presets[selectedPresetIndex];

            string message = "Are you sure you want to override your current values for: ";
            List<string> changesList = new List<string>();
            if (applyToFolders) changesList.Add("Folders");
            if (applyToSeparators) changesList.Add("Separators");
            if (applyToTagAndLayer) changesList.Add("Tag and Layer");
            if (applyToTree) changesList.Add("Hierarchy Tree");
            message += string.Join(", ", changesList) + "?\n\n*If you select 'confirm' all values will be overridden and saved.*";

            if (EditorUtility.DisplayDialog("Confirm Preset Application", message, "Confirm", "Cancel"))
            {
                if (applyToFolders)
                {
                    HierarchyDesigner_Utility_Presets.ApplyPresetToFolders(selectedPreset);
                }
                if (applyToSeparators)
                {
                    HierarchyDesigner_Utility_Presets.ApplyPresetToSeparators(selectedPreset);
                }
                if (applyToTagAndLayer)
                {
                    HierarchyDesigner_Utility_Presets.ApplyPresetToTagLayer(selectedPreset);
                }
                if (applyToTree)
                {
                    HierarchyDesigner_Utility_Presets.ApplyPresetToTree(selectedPreset);
                }

                EditorApplication.RepaintHierarchyWindow();
            }
            else
            {
                return;
            }
        }
    }
}
#endif