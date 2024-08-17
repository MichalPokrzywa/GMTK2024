#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Verpha.HierarchyDesigner
{
    public class HierarchySeparatorWindow : EditorWindow
    {
        #region Separator Fields
        private string newSeparatorName = "";
        private Color newSeparatorTextColor = Color.white;
        private Color newSeparatorBackgroundColor = Color.black;
        private FontStyle newFontStyle = FontStyle.Normal;
        private int newFontSize = 12;
        private readonly int[] fontSizeOptions = new int[15];
        private TextAnchor newTextAlignment = TextAnchor.MiddleCenter;
        private HierarchyDesigner_Info_Separator.BackgroundImageType newBackgroundImageType = HierarchyDesigner_Info_Separator.BackgroundImageType.Classic;
        #endregion
        #region OnGUI Properties
        private Vector2 separatorScrollPosition;
        private static bool hasModifiedChanges = false;
        #endregion
        private const string SeparatorPrefKey = "HierarchySeparators";
        public static Dictionary<string, HierarchyDesigner_Info_Separator> separators = new Dictionary<string, HierarchyDesigner_Info_Separator>();

        [MenuItem("Hierarchy Designer/Hierarchy Separator/Hierarchy Separator Manager")]
        private static void OpenWindow()
        {
            LoadSeparators();
            HierarchySeparatorWindow window = GetWindow<HierarchySeparatorWindow>("Hierarchy Separator Manager");
            window.minSize = new Vector2(500, 300);
            window.InitFontSizeOptions();
        }

        private void InitFontSizeOptions()
        {
            for (int i = 0; i < fontSizeOptions.Length; i++)
            {
                fontSizeOptions[i] = 7 + i;
            }
        }

        private void OnGUI()
        {
            GUIStyle customSettingsStyle = HierarchyDesigner_Info_OnGUI.CreateCustomStyle();
            GUILayout.BeginVertical(customSettingsStyle);

            GUILayout.Space(5);
            GUILayout.Label("Hierarchy Separators Manager", EditorStyles.boldLabel);
            GUILayout.Space(5);

            #region Separator Creation Fields
            newSeparatorName = EditorGUILayout.TextField("Name", newSeparatorName);
            newSeparatorTextColor = EditorGUILayout.ColorField("Text Color", newSeparatorTextColor);
            newSeparatorBackgroundColor = EditorGUILayout.ColorField("Background Color", newSeparatorBackgroundColor);
            newFontStyle = (FontStyle)EditorGUILayout.EnumPopup("Font Style", newFontStyle);
            string[] newFontSizeOptionsStrings = Array.ConvertAll(fontSizeOptions, x => x.ToString());
            int newFontSizeIndex = Array.IndexOf(fontSizeOptions, newFontSize);
            newFontSize = fontSizeOptions[EditorGUILayout.Popup("Font Size", newFontSizeIndex, newFontSizeOptionsStrings)];
            newTextAlignment = (TextAnchor)EditorGUILayout.EnumPopup("Text Alignment", newTextAlignment);
            newBackgroundImageType = (HierarchyDesigner_Info_Separator.BackgroundImageType)EditorGUILayout.EnumPopup("Background Image Type", newBackgroundImageType);
            #endregion

            #region Add Separator
            GUILayout.Space(10);
            if (GUILayout.Button("Add Separator", GUILayout.Height(25)))
            {
                if (IsSeparatorNameValid(newSeparatorName))
                {
                    HierarchyDesigner_Info_Separator newSeparator = (new HierarchyDesigner_Info_Separator(newSeparatorName, newSeparatorTextColor, newSeparatorBackgroundColor, newFontStyle, newFontSize, newTextAlignment, newBackgroundImageType));
                    separators[newSeparatorName] = newSeparator;
                    newSeparatorName = "";
                    GUI.FocusControl(null);
                    hasModifiedChanges = true;
                }
                else
                {
                    EditorUtility.DisplayDialog("Invalid Separator Name", "Separator name is either duplicate or invalid.", "OK");
                }
            }
            GUILayout.Space(10);
            #endregion

            #region Separators List
            if (separators.Count > 0)
            {
                GUIStyle boldLabelStyle = new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Bold, fontSize = 12 };
                GUILayout.Label("Separators:", boldLabelStyle);
                GUILayout.Space(10);
                separatorScrollPosition = EditorGUILayout.BeginScrollView(separatorScrollPosition, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

                List<string> keys = new List<string>(separators.Keys);
                for (int i = 0; i < keys.Count; i++)
                {
                    string key = keys[i];
                    HierarchyDesigner_Info_Separator separator = separators[key];

                    EditorGUI.BeginChangeCheck();
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(10);

                    GUILayout.Label(separator.Name);
                    separator.TextColor = EditorGUILayout.ColorField(separator.TextColor, GUILayout.Width(150));
                    separator.BackgroundColor = EditorGUILayout.ColorField(separator.BackgroundColor, GUILayout.Width(150));
                    separator.FontStyle = (FontStyle)EditorGUILayout.EnumPopup(separator.FontStyle, GUILayout.Width(100));
                    string[] fontSizeOptionsStrings = Array.ConvertAll(fontSizeOptions, x => x.ToString());
                    int fontSizeIndex = Array.IndexOf(fontSizeOptions, separator.FontSize);
                    separator.FontSize = fontSizeOptions[EditorGUILayout.Popup(fontSizeIndex, fontSizeOptionsStrings, GUILayout.Width(40))];
                    separator.TextAlignment = (TextAnchor)EditorGUILayout.EnumPopup(separator.TextAlignment, GUILayout.Width(110));
                    separator.ImageType = (HierarchyDesigner_Info_Separator.BackgroundImageType)EditorGUILayout.EnumPopup(separator.ImageType, GUILayout.Width(100));

                    if (GUILayout.Button("Create", GUILayout.Width(60)))
                    {
                        HierarchyDesigner_Utility_Separator.CreateSeparator(separator);
                    }
                    if (GUILayout.Button("Remove", GUILayout.Width(60)))
                    {
                        separators.Remove(key);
                        hasModifiedChanges = true;
                        GUIUtility.ExitGUI();
                    }
                    GUILayout.EndHorizontal();
                    if (EditorGUI.EndChangeCheck())
                    {
                        hasModifiedChanges = true;
                    }
                }
                EditorGUILayout.EndScrollView();

                GUILayout.Space(10);
                if (GUILayout.Button("Update Separators", GUILayout.Height(30)))
                {
                    HierarchyDesigner_Visual_Separator.UpdateSeparatorVisuals();
                }
                GUILayout.Space(2);
                if (GUILayout.Button("Save Separators", GUILayout.Height(30)))
                {
                    SaveSeparators();
                    HierarchyDesigner_Visual_Separator.UpdateSeparatorVisuals();
                }
                GUILayout.Space(5);
            }
            #endregion
            GUILayout.EndVertical();
        }

        private bool IsSeparatorNameValid(string separatorName)
        {
            return !string.IsNullOrEmpty(separatorName) && !separators.ContainsKey(separatorName);
        }

        public static void SaveSeparators()
        {
            List<string> serializedParts = new List<string>();
            foreach (var kvp in separators)
            {
                HierarchyDesigner_Info_Separator s = kvp.Value;
                string serializedSeparator = $"{s.Name},{HierarchyDesigner_Shared_ColorUtility.ColorToString(s.TextColor)},{HierarchyDesigner_Shared_ColorUtility.ColorToString(s.BackgroundColor)},{s.FontStyle},{s.FontSize},{s.TextAlignment},{s.ImageType}";
                serializedParts.Add(serializedSeparator);
            }
            string serialized = string.Join(";", serializedParts);
            EditorPrefs.SetString(SeparatorPrefKey, serialized);
            hasModifiedChanges = false;
        }

        public static void LoadSeparators()
        {
            string serialized = EditorPrefs.GetString(SeparatorPrefKey, "");
            separators.Clear();

            foreach (var serializedSeparator in serialized.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
            {
                var parts = serializedSeparator.Split(',');
                if (parts.Length == 7)
                {
                    string name = parts[0];
                    Color textColor = HierarchyDesigner_Shared_ColorUtility.ParseColor(parts[1]);
                    Color backgroundColor = HierarchyDesigner_Shared_ColorUtility.ParseColor(parts[2]);
                    FontStyle fontStyle = (FontStyle)Enum.Parse(typeof(FontStyle), parts[3]);
                    int fontSize = int.Parse(parts[4]);
                    TextAnchor textAlignment = (TextAnchor)Enum.Parse(typeof(TextAnchor), parts[5]);
                    HierarchyDesigner_Info_Separator.BackgroundImageType backgroundImageType = (HierarchyDesigner_Info_Separator.BackgroundImageType)Enum.Parse(typeof(HierarchyDesigner_Info_Separator.BackgroundImageType), parts[6]);
                    separators[name] = new HierarchyDesigner_Info_Separator(name, textColor, backgroundColor, fontStyle, fontSize, textAlignment, backgroundImageType);
                }
            }
            hasModifiedChanges = false;
        }

        private void OnDestroy()
        {
            if (hasModifiedChanges)
            {
                bool shouldSave = EditorUtility.DisplayDialog("Separator(s) Have Been Modified",
                    "Do you want to save the changes you made in the separators?",
                    "Save", "Don't Save");

                if (shouldSave)
                {
                    SaveSeparators();
                }
            }
            hasModifiedChanges = false;
        }
    }

    public class HierarchyDesigner_Utility_Separator
    {
        private const string separatorPrefix = "//";
        private const string separatorName = "Separator";

        [MenuItem("Hierarchy Designer/Hierarchy Separator/Create Default Separator", false, 2)]
        private static void CreateDefaultSeparator()
        {
            GameObject separator = new GameObject($"{separatorPrefix}{separatorName}");
            Undo.RegisterCreatedObjectUndo(separator, $"Create Default Separator");

            separator.tag = "EditorOnly";
            EditorGUIUtility.PingObject(separator);

            SetSeparatorState(separator, false);
            separator.SetActive(false);

            HierarchyDesigner_Visual_Separator.UpdateSeparatorVisuals();
        }

        [MenuItem("Hierarchy Designer/Hierarchy Separator/Create All Separators", false, 1)]
        private static void CreateAllSeparatorsFromList()
        {
            foreach (HierarchyDesigner_Info_Separator separatorInfo in HierarchySeparatorWindow.separators.Values)
            {
                CreateSeparator(separatorInfo);
            }
        }

        [MenuItem("Hierarchy Designer/Hierarchy Separator/Create Missing Separators", false, 2)]
        private static void CreateMissingSeparators()
        {
            foreach (HierarchyDesigner_Info_Separator separatorInfo in HierarchySeparatorWindow.separators.Values)
            {
                if (!SeparatorExists(separatorInfo.Name))
                {
                    CreateSeparator(separatorInfo);
                }
            }
        }

        public static void CreateSeparator(HierarchyDesigner_Info_Separator separatorInfo)
        {
            GameObject separator = new GameObject($"{separatorPrefix}{separatorInfo.Name}");
            Undo.RegisterCreatedObjectUndo(separator, $"Create {separatorInfo.Name}");

            separator.tag = "EditorOnly";
            EditorGUIUtility.PingObject(separator);

            SetSeparatorState(separator, false);
            separator.SetActive(false);

            HierarchyDesigner_Visual_Separator.UpdateSeparatorVisuals();
        }

        private static bool SeparatorExists(string separatorName)
        {
            GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
            string fullSeparatorName = $"{separatorPrefix}{separatorName}";
            foreach (GameObject obj in allObjects)
            {
                if (obj.name.Equals(fullSeparatorName))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsSeparator(GameObject gameObject)
        {
            return gameObject != null && gameObject.name.StartsWith(separatorPrefix) && gameObject.tag == "EditorOnly";
        }

        private static void SetSeparatorState(GameObject gameObject, bool editable)
        {
            foreach (Component component in gameObject.GetComponents<Component>())
            {
                if (component)
                {
                    component.hideFlags = editable ? HideFlags.None : HideFlags.NotEditable;
                }
            }

            gameObject.hideFlags = editable ? HideFlags.None : HideFlags.NotEditable;
            EditorUtility.SetDirty(gameObject);
        }
    }
}
#endif