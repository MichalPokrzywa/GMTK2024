#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Verpha.HierarchyDesigner
{
    public class HierarchyFolderWindow : EditorWindow
    {
        #region Folder Fields
        private string newFolderName = "";
        private Color newFolderIconColor = Color.white;
        private HierarchyDesigner_Info_Folder.FolderImageType newFolderImageType = HierarchyDesigner_Info_Folder.FolderImageType.Default;
        #endregion
        #region OnGUI Properties
        private Vector2 folderScrollPosition;
        private static bool hasModifiedChanges = false;
        #endregion
        private const string FolderPrefKey = "HierarchyFolders";
        public static Dictionary<string, HierarchyDesigner_Info_Folder> folders = new Dictionary<string, HierarchyDesigner_Info_Folder>();

        [MenuItem("Hierarchy Designer/Hierarchy Folder/Hierarchy Folder Manager")]
        private static void OpenWindow()
        {
            LoadFolders();
            HierarchyFolderWindow window = GetWindow<HierarchyFolderWindow>("Hierarchy Folder Manager");
            window.minSize = new Vector2(525, 325);
        }

        private void OnGUI()
        {
            GUIStyle customSettingsStyle = HierarchyDesigner_Info_OnGUI.CreateCustomStyle();
            GUILayout.BeginVertical(customSettingsStyle);
            
            GUILayout.Space(5);
            GUILayout.Label("Hierarchy Folders Manager", EditorStyles.boldLabel);
            GUILayout.Space(5);

            #region Folder Creation Fields
            newFolderName = EditorGUILayout.TextField("Folder Name", newFolderName);
            newFolderIconColor = EditorGUILayout.ColorField("Folder Color", newFolderIconColor);
            newFolderImageType = (HierarchyDesigner_Info_Folder.FolderImageType)EditorGUILayout.EnumPopup("Folder Image Type", newFolderImageType);
            #endregion

            #region Add Folder
            GUILayout.Space(10);
            if (GUILayout.Button("Add Folder", GUILayout.Height(25)))
            {
                if (IsFolderNameValid(newFolderName))
                {
                    HierarchyDesigner_Info_Folder newFolder = new HierarchyDesigner_Info_Folder(newFolderName, newFolderIconColor, newFolderImageType);
                    folders[newFolderName] = newFolder;
                    newFolderName = "";
                    GUI.FocusControl(null);
                    hasModifiedChanges = true;
                }
                else
                {
                    EditorUtility.DisplayDialog("Invalid Folder Name", "Folder name is either duplicate or invalid.", "OK");
                }
            }
            GUILayout.Space(10);
            #endregion

            #region Folders List
            if (folders.Count > 0)
            {
                GUIStyle boldLabelStyle = new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Bold, fontSize = 12 };
                GUILayout.Label("Folders:", boldLabelStyle);               
                GUILayout.Space(10);
                folderScrollPosition = EditorGUILayout.BeginScrollView(folderScrollPosition, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

                List<string> keys = new List<string>(folders.Keys);
                for (int i = 0; i < keys.Count; i++)
                {
                    string key = keys[i];
                    HierarchyDesigner_Info_Folder folder = folders[key];

                    EditorGUI.BeginChangeCheck();
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(10);

                    GUILayout.Label(folder.Name);
                    folder.FolderColor = EditorGUILayout.ColorField(folder.FolderColor, GUILayout.Width(150));
                    folder.ImageType = (HierarchyDesigner_Info_Folder.FolderImageType)EditorGUILayout.EnumPopup(folder.ImageType, GUILayout.Width(125));

                    if (GUILayout.Button("Create", GUILayout.Width(60)))
                    {
                        HierarchyDesigner_Utility_Folder.CreateFolder(folder);
                    }
                    if (GUILayout.Button("Remove", GUILayout.Width(60)))
                    {
                        folders.Remove(key);
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
                if (GUILayout.Button("Update Folders", GUILayout.Height(30)))
                {
                    HierarchyDesigner_Visual_Folder.UpdateFolderVisuals();
                }
                GUILayout.Space(2);
                if (GUILayout.Button("Save Folders", GUILayout.Height(30)))
                {
                    SaveFolders();
                    HierarchyDesigner_Visual_Folder.UpdateFolderVisuals();
                }
                GUILayout.Space(5);
            }
            #endregion
            GUILayout.EndVertical();
        }

        private bool IsFolderNameValid(string folderName)
        {
            return !string.IsNullOrEmpty(folderName) && !folders.ContainsKey(folderName);
        }

        public static void SaveFolders()
        {
            List<string> serializedParts = new List<string>();
            foreach (var folder in folders)
            {
                var f = folder.Value;
                serializedParts.Add($"{f.Name},{HierarchyDesigner_Shared_ColorUtility.ColorToString(f.FolderColor)},{f.ImageType}");
            }
            string serialized = string.Join(";", serializedParts);
            EditorPrefs.SetString(FolderPrefKey, serialized);
            hasModifiedChanges = false;
        }

        public static void LoadFolders()
        {
            string serialized = EditorPrefs.GetString(FolderPrefKey, "");
            folders.Clear();

            foreach (var serializedFolder in serialized.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
            {
                var parts = serializedFolder.Split(',');
                if (parts.Length == 3)
                {
                    string name = parts[0];
                    Color iconColor = HierarchyDesigner_Shared_ColorUtility.ParseColor(parts[1]);
                    HierarchyDesigner_Info_Folder.FolderImageType folderImageType = (HierarchyDesigner_Info_Folder.FolderImageType)Enum.Parse(typeof(HierarchyDesigner_Info_Folder.FolderImageType), parts[2]);
                    folders[name] = new HierarchyDesigner_Info_Folder(name, iconColor, folderImageType);
                }
            }
            hasModifiedChanges = false;
        }

        private void OnDestroy()
        {
            if (hasModifiedChanges)
            {
                bool shouldSave = EditorUtility.DisplayDialog("Folder(s) Have Been Modified",
                    "Do you want to save the changes you made in the folders?",
                    "Save", "Don't Save");

                if (shouldSave)
                {
                    SaveFolders();
                }
            }
            hasModifiedChanges = false;
        }
    }

    public class HierarchyDesigner_Utility_Folder
    {
        #region Default Folder
        [MenuItem("Hierarchy Designer/Hierarchy Folder/Create Default Folder", false, 2)]
        private static void CreateDefaultFolder()
        {
            CreateFolderObject("New Folder", HierarchyDesigner_Info_Folder.FolderImageType.Default);
        }

        private static void CreateFolderObject(string folderName, HierarchyDesigner_Info_Folder.FolderImageType folderImageType)
        {
            GameObject folder = new GameObject(folderName);
            folder.AddComponent<HierarchyDesignerFolder>();

            Texture2D folderIcon = HierarchyDesigner_Manager_Folder.GetFolderIcon(folderImageType);
            if (folderIcon != null)
            {
                EditorGUIUtility.SetIconForObject(folder, folderIcon);
            }

            Undo.RegisterCreatedObjectUndo(folder, $"Create {folderName}");
            EditorGUIUtility.PingObject(folder);
        }
        #endregion

        [MenuItem("Hierarchy Designer/Hierarchy Folder/Create All Folders", false, 1)]
        private static void CreateAllFoldersFromList()
        {
            foreach (HierarchyDesigner_Info_Folder folderInfo in HierarchyFolderWindow.folders.Values)
            {
                CreateFolder(folderInfo);
            }
        }

        [MenuItem("Hierarchy Designer/Hierarchy Folder/Create Missing Folders", false, 2)]
        private static void CreateMissingFolders()
        {
            foreach (HierarchyDesigner_Info_Folder folderInfo in HierarchyFolderWindow.folders.Values)
            {
                if (!FolderExists(folderInfo.Name))
                {
                    CreateFolder(folderInfo);
                }
            }
        }

        public static void CreateFolder(HierarchyDesigner_Info_Folder folderInfo)
        {
            GameObject folder = new GameObject(folderInfo.Name);

            folder.AddComponent<HierarchyDesignerFolder>();

            Undo.RegisterCreatedObjectUndo(folder, $"Create {folderInfo.Name}");

            Texture2D inspectorIcon = HierarchyDesigner_Manager_Folder.InspectorFolderIcon;
            if (inspectorIcon != null)
            {
                EditorGUIUtility.SetIconForObject(folder, inspectorIcon);
            }

            EditorGUIUtility.PingObject(folder);
        }

        private static bool FolderExists(string folderName)
        {
            return HierarchyFolderWindow.folders.ContainsKey(folderName);
        }
    }
}
#endif